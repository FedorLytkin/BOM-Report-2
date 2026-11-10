using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace NSoft.Promo
{
    /// <summary>
    /// Содержимое промокода после разбора.
    /// </summary>
    public sealed class PromoToken
    {
        /// <summary>Битовая маска функций. Значения битов у каждой программы свои (PromoProducts).</summary>
        public ushort Features;
        /// <summary>Основная версия CAD: КОМПАС v23 -> 23, Inventor 2026 -> 26, SolidWorks 2024 -> 24.</summary>
        public int CadMajor;
        public DateTime IssueDate;
        /// <summary>Сколько дней после выдачи код можно активировать. 0 - без ограничения.</summary>
        public int ActivateDays;
        public byte[] Payload;
        public byte[] Signature;

        public DateTime? ActivateUntil => ActivateDays == 0 ? (DateTime?)null : IssueDate.AddDays(ActivateDays);
    }

    /// <summary>
    /// Формат промокода и файлов обмена. ОБЩИЙ файл для всех программ NSoft, LicManager2.0 и Tools\PromoKeyGen.
    /// Оригинал - DXFAuto2.0\PromoCods\PromoCodec.cs, в остальных проектах лежат копии: менять все одинаково.
    ///
    /// Промокод = Base32(payload + подпись ECDSA P-256).
    /// Подписывается домен программы + payload + хэш отпечатка компьютера, поэтому код подходит
    /// только той программе и тому компьютеру, для которых выписан.
    ///
    /// Обмен с клиентом идёт файлами (как у лицензии):
    ///   программа -> разработчику: PromoQuery_(...).datq - файл-запрос, gzip, cp1251, первая строка RequestPatternId;
    ///   разработчик -> клиенту:    promo_(...).dat - файл промокода, gzip, первая строка - заголовок программы.
    /// </summary>
    public static class PromoCodec
    {
        public const byte FormatVersion = 1;
        const int PayloadLength = 7;
        const int SignatureLength = 64;
        static readonly DateTime Epoch = new DateTime(2020, 1, 1);
        const string Alphabet = "0123456789ABCDEFGHJKMNPQRSTVWXYZ"; //Crockford Base32

        #region Файлы обмена
        /// <summary>PatternID файла-запроса промокода. 100/101 - запросы лицензии.</summary>
        public const string RequestPatternId = "201";
        public const string RequestFilePrefix = "PromoQuery_";
        public const string ResponseFilePrefix = "promo_";
        static Encoding FileEncoding => Encoding.GetEncoding(1251);

        /// <summary>
        /// Строки файла-запроса (pattern 201). Строки 0-10 совпадают с запросом лицензии (pattern 101),
        /// дальше: основная версия CAD, уже включённые функции (число), код программы (PromoProduct.Id).
        /// </summary>
        public sealed class PromoRequest
        {
            public DateTime Date;
            public string CadVersion;
            public string ComputerName;
            public string ShortProgramVersion;
            public string ProgramId;
            public string ProcessorId;
            public string VolumeSerial;
            public string MotherBoardId;
            public string FullProgramVersion;
            public bool IsVirtual;
            public int CadMajor;
            public ushort CurrentFeatures;
            /// <summary>PromoProduct.Id. В запросах DXF-Auto Kompas 25.112.26.265 строки нет - тогда PromoProducts.LegacyProductId.</summary>
            public string ProductId;

            public byte[] FingerprintHash() => HashFingerprint(MotherBoardId, ProcessorId);
        }

        public static void WriteRequestFile(string path, PromoRequest r)
        {
            WriteGzipLines(path, FileEncoding, new[]
            {
                RequestPatternId,
                r.Date.ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                r.CadVersion ?? "",
                r.ComputerName ?? "",
                r.ShortProgramVersion ?? "",
                r.ProgramId ?? "",
                r.ProcessorId ?? "",
                r.VolumeSerial ?? "",
                r.MotherBoardId ?? "",
                r.FullProgramVersion ?? "",
                r.IsVirtual.ToString(),
                r.CadMajor.ToString(),
                r.CurrentFeatures.ToString(),
                r.ProductId ?? "",
            });
        }

        /// <summary>Разбирает строки уже распакованного файла-запроса промокода.</summary>
        public static bool TryParseRequest(IList<string> lines, out PromoRequest r)
        {
            r = null;
            if (lines == null || lines.Count < 12 || lines[0] != RequestPatternId) return false;
            DateTime.TryParseExact(lines[1], "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime date);
            bool.TryParse(lines[10], out bool isVirtual);
            int.TryParse(lines[11], out int cad);
            ushort features = 0;
            if (lines.Count > 12) ushort.TryParse(lines[12], out features);
            string product = lines.Count > 13 ? lines[13].Trim() : "";
            if (cad <= 0) cad = CadMajorFromText(lines[2]);
            r = new PromoRequest
            {
                Date = date,
                CadVersion = lines[2],
                ComputerName = lines[3],
                ShortProgramVersion = lines[4],
                ProgramId = lines[5],
                ProcessorId = lines[6],
                VolumeSerial = lines[7],
                MotherBoardId = lines[8],
                FullProgramVersion = lines[9],
                IsVirtual = isVirtual,
                CadMajor = cad,
                CurrentFeatures = features,
                ProductId = product.Length > 0 ? product : PromoProducts.LegacyProductId,
            };
            return true;
        }

        public static List<string> ReadGzipLines(string path)
        {
            List<string> lines = new List<string>();
            using (FileStream fs = File.OpenRead(path))
            using (GZipStream gz = new GZipStream(fs, CompressionMode.Decompress))
            using (StreamReader reader = new StreamReader(gz, FileEncoding))
                while (!reader.EndOfStream) lines.Add(reader.ReadLine());
            return lines;
        }

        public static void WriteResponseFile(string path, PromoProduct product, string code)
        {
            WriteGzipLines(path, Encoding.ASCII, new[] { product.ResponseHeader, FormatVersion.ToString(), code });
        }

        /// <summary>
        /// Читает файл промокода. false - это не файл промокода (например, файл лицензии keyfile.dat или повреждённый файл).
        /// product - программа, для которой выписан файл (по заголовку).
        /// </summary>
        public static bool TryReadResponseFile(string path, out PromoProduct product, out string code)
        {
            product = null;
            code = null;
            try
            {
                List<string> lines = ReadGzipLines(path);
                if (lines.Count < 3) return false;
                product = PromoProducts.FindByHeader(lines[0]);
                if (product == null) return false;
                code = lines[2].Trim();
                return code.Length > 0;
            }
            catch
            {
                return false;
            }
        }

        static void WriteGzipLines(string path, Encoding encoding, IEnumerable<string> lines)
        {
            using (FileStream fs = File.Create(path))
            using (GZipStream gz = new GZipStream(fs, CompressionMode.Compress))
            using (StreamWriter writer = new StreamWriter(gz, encoding))
                foreach (string line in lines) writer.WriteLine(line);
        }

        /// <summary>
        /// Основная версия CAD из строки версии: "KOMPAS-3D v23.0.4" -> 23, "Inventor 2026" -> 26, "SolidWorks 2024" -> 24.
        /// 0 - не удалось определить.
        /// </summary>
        public static int CadMajorFromText(string text)
        {
            Match m = Regex.Match(text ?? "", @"v(\d+)");
            if (m.Success) return int.Parse(m.Groups[1].Value);
            m = Regex.Match(text ?? "", @"\b20(\d{2})\b");
            return m.Success ? int.Parse(m.Groups[1].Value) : 0;
        }
        #endregion

        #region Отпечаток компьютера
        public static byte[] HashFingerprint(string motherBoardId, string processorId)
        {
            string text = "MB=" + Normalize(motherBoardId) + "|CPU=" + Normalize(processorId);
            using (SHA256 sha = SHA256.Create())
                return sha.ComputeHash(Encoding.UTF8.GetBytes(text));
        }
        static string Normalize(string s) => (s ?? "").Trim().ToUpperInvariant();
        #endregion

        #region Промокод
        public static byte[] BuildPayload(ushort features, int cadMajor, DateTime issueDate, int activateDays)
        {
            if (cadMajor < 0 || cadMajor > 255) throw new ArgumentOutOfRangeException(nameof(cadMajor));
            if (activateDays < 0 || activateDays > 255) throw new ArgumentOutOfRangeException(nameof(activateDays));
            int day = (int)(issueDate.Date - Epoch).TotalDays;
            if (day < 0 || day > ushort.MaxValue) throw new ArgumentOutOfRangeException(nameof(issueDate));
            return new byte[]
            {
                FormatVersion,
                (byte)(features & 0xFF), (byte)(features >> 8),
                (byte)cadMajor,
                (byte)(day & 0xFF), (byte)(day >> 8),
                (byte)activateDays,
            };
        }

        /// <summary>Данные, которые подписываются: домен программы + payload + хэш отпечатка компьютера.</summary>
        public static byte[] SignedMessage(PromoProduct product, byte[] payload, byte[] fingerprintHash)
        {
            return Encoding.ASCII.GetBytes(product.Domain).Concat(payload).Concat(fingerprintHash).ToArray();
        }

        public static string EncodeCode(byte[] payload, byte[] signature)
        {
            return Group(ToBase32(payload.Concat(signature).ToArray()));
        }

        public static bool TryDecodeCode(string text, out PromoToken token)
        {
            token = null;
            byte[] data = FromBase32(text);
            if (data == null || data.Length != PayloadLength + SignatureLength) return false;
            if (data[0] != FormatVersion) return false;
            int day = data[4] | (data[5] << 8);
            token = new PromoToken
            {
                Features = (ushort)(data[1] | (data[2] << 8)),
                CadMajor = data[3],
                IssueDate = Epoch.AddDays(day),
                ActivateDays = data[6],
                Payload = data.Take(PayloadLength).ToArray(),
                Signature = data.Skip(PayloadLength).ToArray(),
            };
            return true;
        }

        /// <summary>
        /// Проверка подписи открытым ключом. Через CNG, а не ECDsa.Create(ECParameters), чтобы работало и на .NET Framework 4.6.1.
        /// </summary>
        public static bool Verify(PromoProduct product, PromoToken token, byte[] fingerprintHash, byte[] publicX, byte[] publicY)
        {
            try
            {
                //BCRYPT_ECCKEY_BLOB: "ECS1" (открытый ключ P-256), длина координаты, X, Y
                byte[] blob = new byte[8 + 64];
                Encoding.ASCII.GetBytes("ECS1").CopyTo(blob, 0);
                BitConverter.GetBytes(32).CopyTo(blob, 4);
                Coordinate(publicX).CopyTo(blob, 8);
                Coordinate(publicY).CopyTo(blob, 40);
                byte[] hash;
                using (SHA256 sha = SHA256.Create())
                    hash = sha.ComputeHash(SignedMessage(product, token.Payload, fingerprintHash));
                using (CngKey key = CngKey.Import(blob, CngKeyBlobFormat.EccPublicBlob))
                using (ECDsaCng ecdsa = new ECDsaCng(key))
                    return ecdsa.VerifyHash(hash, token.Signature);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Координата ровно 32 байта (без лишнего ведущего нуля и с дополнением нулями слева).</summary>
        static byte[] Coordinate(byte[] value)
        {
            byte[] result = new byte[32];
            int skip = Math.Max(0, value.Length - 32);
            Array.Copy(value, skip, result, 32 - (value.Length - skip), value.Length - skip);
            return result;
        }
        #endregion

        #region Base32
        static string ToBase32(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            int buffer = 0, bits = 0;
            foreach (byte b in data)
            {
                buffer = (buffer << 8) | b;
                bits += 8;
                while (bits >= 5)
                {
                    sb.Append(Alphabet[(buffer >> (bits - 5)) & 31]);
                    bits -= 5;
                }
            }
            if (bits > 0) sb.Append(Alphabet[(buffer << (5 - bits)) & 31]);
            return sb.ToString();
        }

        /// <summary>Разбор Base32. Пробелы и дефисы игнорируются, O читается как 0, I и L - как 1.</summary>
        static byte[] FromBase32(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            List<byte> bytes = new List<byte>();
            int buffer = 0, bits = 0;
            foreach (char raw in text.ToUpperInvariant())
            {
                char c = raw;
                if (c == '-' || char.IsWhiteSpace(c)) continue;
                if (c == 'O') c = '0';
                else if (c == 'I' || c == 'L') c = '1';
                int v = Alphabet.IndexOf(c);
                if (v < 0) return null;
                buffer = ((buffer << 5) | v) & 0xFFFF;
                bits += 5;
                if (bits >= 8)
                {
                    bytes.Add((byte)(buffer >> (bits - 8)));
                    bits -= 8;
                }
            }
            return bytes.ToArray();
        }

        static string Group(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                if (i > 0 && i % 5 == 0) sb.Append('-');
                sb.Append(s[i]);
            }
            return sb.ToString();
        }
        #endregion
    }
}
