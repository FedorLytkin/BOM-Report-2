using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows.Forms;

namespace NSoft.Promo
{
    public enum PromoCheckResult
    {
        Ok,
        /// <summary>Строка не похожа на промокод.</summary>
        BadFormat,
        /// <summary>Подпись не сошлась: код выписан для другого компьютера или повреждён.</summary>
        WrongMachine,
        /// <summary>Код выписан под другую версию CAD.</summary>
        WrongCad,
        /// <summary>Истёк срок, в который код можно было активировать.</summary>
        ActivationExpired,
    }

    /// <summary>
    /// Промокоды на стороне программы. ОБЩИЙ файл: копии во всех программах должны совпадать.
    /// Включённые функции не хранятся флагами: при каждом запуске они вычисляются заново из сохранённых кодов.
    /// Код работает, пока совпадают программа, компьютер (материнская плата + процессор) и основная версия CAD.
    /// </summary>
    public sealed class PromoClient
    {
        //Открытый ключ. Закрытая пара - только у разработчика (LicManager2.0 / Tools\PromoKeyGen).
        static readonly byte[] PublicX = Convert.FromBase64String("AIF9xe8MwnX5GBwOCRqZDsDAIkuN8TzzPLzkQE8+41Y=");
        static readonly byte[] PublicY = Convert.FromBase64String("T1Y+ucviX5AifP2/MsyV0QN9RRvY2ilj4n14ekbu8OU=");

        public PromoProduct Product { get; }
        readonly string programId;
        readonly Func<string> cadVersionText;
        readonly Func<bool> isVirtual;

        /// <param name="programId">ProgramID программы (как в запросе лицензии).</param>
        /// <param name="cadVersionText">Строка версии CAD, как в запросе лицензии. null, если CAD ещё не подключён.</param>
        /// <param name="isVirtual">Проверка виртуальной машины (как в запросе лицензии).</param>
        public PromoClient(PromoProduct product, string programId, Func<string> cadVersionText, Func<bool> isVirtual = null)
        {
            Product = product;
            this.programId = programId;
            this.cadVersionText = cadVersionText;
            this.isVirtual = isVirtual;
        }

        #region Компьютер
        static string motherBoardId, processorId;
        static byte[] machineHash;

        static void LoadMachineIds()
        {
            if (machineHash != null) return;
            //те же поля, что в License\CryptoClass (последнее найденное значение)
            motherBoardId = ReadWmi("Win32_BaseBoard", "SerialNumber");
            processorId = ReadWmi("Win32_Processor", "ProcessorId");
            machineHash = PromoCodec.HashFingerprint(motherBoardId, processorId);
        }

        public static byte[] MachineHash()
        {
            LoadMachineIds();
            return machineHash;
        }

        static string ReadWmi(string table, string property)
        {
            string value = "";
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(new SelectQuery(table)))
                    foreach (ManagementObject item in searcher.Get())
                        value = item[property]?.ToString().Trim() ?? value;
            }
            catch { }
            return value;
        }

        static string VolumeSerial()
        {
            try
            {
                using (ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"C:\""))
                {
                    disk.Get();
                    return disk["VolumeSerialNumber"]?.ToString().Trim() ?? "";
                }
            }
            catch { return ""; }
        }
        #endregion

        string CadText()
        {
            try { return cadVersionText?.Invoke(); }
            catch { return null; }
        }

        /// <summary>Основная версия запущенной CAD. Если CAD не определена - версия, под которую собрана программа.</summary>
        public int CurrentCadMajor()
        {
            int major = PromoCodec.CadMajorFromText(CadText());
            if (major > 0) return major;
            int.TryParse(Application.ProductVersion.Split('.')[0], out int build);
            return build;
        }

        #region Файл-запрос
        /// <summary>Имя файла-запроса без расширения, по образцу запроса лицензии.</summary>
        public string RequestFileName()
        {
            return $"{PromoCodec.RequestFilePrefix}({Environment.MachineName}_{programId}_{Application.ProductName}_v{Application.ProductVersion})";
        }

        public void WriteRequestFile(string path, IEnumerable<string> activeCodes)
        {
            LoadMachineIds();
            string[] v = Application.ProductVersion.Split('.');
            bool vm = false;
            try { vm = isVirtual != null && isVirtual(); } catch { }
            PromoCodec.WriteRequestFile(path, new PromoCodec.PromoRequest
            {
                Date = DateTime.Now,
                CadVersion = CadText(),
                ComputerName = Environment.MachineName,
                ShortProgramVersion = string.Join(".", v.Take(3)),
                ProgramId = programId,
                ProcessorId = processorId,
                VolumeSerial = VolumeSerial(),
                MotherBoardId = motherBoardId,
                FullProgramVersion = Application.ProductVersion,
                IsVirtual = vm,
                CadMajor = CurrentCadMajor(),
                CurrentFeatures = ActiveFeatures(activeCodes),
                ProductId = Product.Id,
            });
        }
        #endregion

        #region Проверка
        /// <param name="activation">true - код вводится впервые: дополнительно проверяется срок активации.</param>
        public PromoCheckResult Check(string code, bool activation, out PromoToken token)
        {
            if (!PromoCodec.TryDecodeCode(code, out token)) return PromoCheckResult.BadFormat;
            if (!PromoCodec.Verify(Product, token, MachineHash(), PublicX, PublicY)) return PromoCheckResult.WrongMachine;
            if (token.CadMajor != CurrentCadMajor()) return PromoCheckResult.WrongCad;
            if (activation && token.ActivateUntil.HasValue && DateTime.Today > token.ActivateUntil.Value) return PromoCheckResult.ActivationExpired;
            return PromoCheckResult.Ok;
        }

        public ushort ActiveFeatures(IEnumerable<string> codes)
        {
            ushort result = 0;
            foreach (string code in codes ?? Enumerable.Empty<string>())
                if (Check(code, false, out PromoToken token) == PromoCheckResult.Ok)
                    result |= token.Features;
            return result;
        }

        public bool Has(ushort features, PromoFeatureInfo feature) => (features & feature.Bit) != 0;
        public bool Has(ushort features, string featureKey)
        {
            PromoFeatureInfo f = Product.Features.FirstOrDefault(x => x.Key == featureKey);
            return f != null && Has(features, f);
        }

        public string ActiveFeaturesText(IEnumerable<string> codes)
        {
            ushort f = ActiveFeatures(codes);
            return f == 0 ? "Промокоды не активированы." : "Сейчас включено: " + Product.Describe(f) + ".";
        }
        #endregion

        #region Загрузка файла промокода
        /// <summary>
        /// Проверяет файл промокода (.dat) и добавляет код в список codes. Сохранять список - вызывающему.
        /// </summary>
        public bool ImportPromoFile(string path, List<string> codes, out string message)
        {
            message = null;
            if (string.Equals(Path.GetExtension(path), ".datq", StringComparison.OrdinalIgnoreCase))
            {
                message = "Это файл-запрос. Его нужно отправить разработчику, а в ответ вы получите файл промокода (.dat).";
                return false;
            }
            if (!PromoCodec.TryReadResponseFile(path, out PromoProduct fileProduct, out string code))
            {
                message = "Это не файл промокода.\nЕсли это файл лицензии (keyfile), загрузите его в Менеджере лицензии.";
                return false;
            }
            if (fileProduct != Product)
            {
                message = string.Format("Этот файл промокода выписан для программы {0}, а это {1}.", fileProduct.Name, Product.Name);
                return false;
            }

            PromoCheckResult result = Check(code, true, out PromoToken token);
            switch (result)
            {
                case PromoCheckResult.Ok:
                    break;
                case PromoCheckResult.WrongMachine:
                    message = "Файл промокода выписан для другого компьютера.";
                    return false;
                case PromoCheckResult.WrongCad:
                    message = string.Format("Промокод выписан для версии CAD {0}, а у вас {1}.\nДля новой версии CAD нужен новый промокод: создайте файл-запрос.",
                        token.CadMajor, CurrentCadMajor());
                    return false;
                case PromoCheckResult.ActivationExpired:
                    message = string.Format("Срок активации промокода истёк {0:dd.MM.yyyy}.\nСоздайте новый файл-запрос.", token.ActivateUntil.Value);
                    return false;
                default:
                    message = "Файл промокода повреждён.";
                    return false;
            }

            if (!codes.Contains(code)) codes.Add(code);
            message = string.Format("Промокод применён. Включено: {0}.", Product.Describe(token.Features));
            return true;
        }
        #endregion

        #region Хранение кодов
        /// <summary>Файл активированных кодов: %APPDATA%\NSoft\Promo\{Product.Id}.txt (по одному коду в строке).</summary>
        public string StorePath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NSoft", "Promo", Product.Id + ".txt");

        public List<string> LoadCodes()
        {
            try
            {
                if (File.Exists(StorePath))
                    return File.ReadAllLines(StorePath).Select(s => s.Trim()).Where(s => s.Length > 0).Distinct().ToList();
            }
            catch { }
            return new List<string>();
        }

        public void SaveCodes(IEnumerable<string> codes)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(StorePath));
            File.WriteAllLines(StorePath, codes.Distinct());
        }
        #endregion
    }
}
