using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DXF_AutoHelp.Licensing;
using VSNRM_Kompas;

namespace SaveDXF
{
    /// <summary>
    /// Лицензия BOM-Report Kompas - настройка общей <see cref="ProductLicense"/> из DXF-AutoHelp.Licensing:
    /// годовая подписанная лицензия, переходные 14 дней для старого keyfile.dat, зашифрованный файл-запрос.
    /// </summary>
    public static class ProgramLicense
    {
        /// <summary>Старый ключ (тот же путь, что CFG_Class.KeyFile_Puth).</summary>
        public static string KeyFilePath
        {
            get { return $@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments)}\NSoft\{Application.ProductName}\CFG\keyfile.dat"; }
        }

        // открытые ключи разработчика общие для всех программ и зашиты в библиотеку (PublicKeys.Default)
        private static ProductLicense _license;

        public static ProductLicense License
        {
            get
            {
                return _license ?? (_license = new ProductLicense(new ProductLicenseOptions
                {
                    ProductId = int.Parse(MainForm.ProgramID),
                    ProductName = Application.ProductName,
                    ProgramVersion = Application.ProductVersion,
                    ReleaseDate = LicenseValidator.ReleaseDateOf(Assembly.GetExecutingAssembly()),
                    // рядом со старым keyfile.dat
                    LicenseFile = Path.Combine(Path.GetDirectoryName(KeyFilePath), "license.lic"),
                    LegacyKeyVersion = () => File.Exists(KeyFilePath)
                                             ? new CryptoClass().LegacyKeyVersionAnyVersion(KeyFilePath) : null,
                    IsVirtualMachine = DetectVirtualMachine_Class.DetectVirtualMachine,
                    RequestExtra = () => new Dictionary<string, string> { { "cad", Body.KompasVersion ?? "" } }
                }));
            }
        }

        public static LicenseSource Source { get { return License.Source; } }
        public static bool IsLicensed { get { return License.IsLicensed; } }

        public static void Refresh() { License.Refresh(); }
        public static string DescribeCurrent() { return License.DescribeCurrent(); }
        public static string TakeUpdatesNotice() { return License.TakeExpiryNotice(); }
        internal static string T(string ru, string en) { return License.T(ru, en); }
    }
}
