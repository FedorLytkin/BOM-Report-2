using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Management;
using System.IO;
using VSNRM_Kompas;
using VSNRM_Kompas.Options;

namespace SaveDXF
{
    class CryptoClass
    {
        public static string MotherBoardID_;
        public static string ProcessorID_;
        public static string Volumeserialnumber_;
        public static string ProgramID_;
        public static string ProgramVersion_;
        public static string CADProgramVersion_;
        public static string LicKey_;
        public static string ComputerName_;
        public static string DelimerChar = "ANK";
        RijndaelManaged Rijndael;
        CFG_Class OptionClass_ = new CFG_Class();

        public CryptoClass()
        {
            Rijndael = new RijndaelManaged();
        }
        //Метод для получения MotherBoardID 
        string GetMotherBoardID()
        {
            string MotherBoardID = string.Empty;
            
            SelectQuery query = new SelectQuery("Win32_BaseBoard");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection.ManagementObjectEnumerator enumerator = searcher.Get().GetEnumerator();
            while (enumerator.MoveNext())
            {
                ManagementObject info = (ManagementObject)enumerator.Current;
                MotherBoardID = info["SerialNumber"].ToString().Trim();
            }
            MotherBoardID_ = MotherBoardID;
            return MotherBoardID;
        }
        //Метод возвращает версию программы(первые 3 цифры)
        string GetProgrammVersion()
        {
            string ProgrammVers = null;
            char versPoint = '.'; 
            string[] verDelArr = Application.ProductVersion.Split(versPoint);
            ProgrammVers = verDelArr[0] + versPoint + verDelArr[1] + versPoint + verDelArr[2];
            ProgramVersion_ = ProgrammVers;
            return ProgrammVers;
        }
        //Метод возвращает имя программы
        string GetProgramName(bool TranslateID)
        {
            string ProgramName = null;
            if (TranslateID) 
                ProgramName = MainForm.ProgramID; 
            else 
                ProgramName = System.Windows.Forms.Application.ProductName;
            ProgramID_ = ProgramName;
            return ProgramName;
        }
        //Метод для получения ProcessorID
        string GetProcessorID()
        {
            string ProcessorID = string.Empty;
            SelectQuery query = new SelectQuery("Win32_processor");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection.ManagementObjectEnumerator enumerator = searcher.Get().GetEnumerator();
            while (enumerator.MoveNext())
            {
                ManagementObject info = (ManagementObject)enumerator.Current;
                ProcessorID = info["processorId"].ToString().Trim();
            }
            ProcessorID_ = ProcessorID;
            return ProcessorID;
        }
        //Метод для получения VolumeSerial("C:\")
        string GetVolumeSerial(string strDriveLetter = "C")
        {
            ManagementObject VolumeSerial = new ManagementObject(string.Format("win32_logicaldisk.deviceid=\"{0}:\"", strDriveLetter));
            VolumeSerial.Get();
            string VolumeSeria = VolumeSerial["VolumeSerialNumber"].ToString().Trim();
            Volumeserialnumber_ = VolumeSeria;
            return VolumeSeria;
        }
        //Метод для получения имени компьютера
        string GetComputerName(bool EncodInByte)
        {
            string ComputerName = null;
            if (EncodInByte)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(Environment.MachineName);
                int result = BitConverter.ToInt32(bytes, 0);
                ComputerName = Convert.ToString(result);
            }
            else
                ComputerName = Environment.MachineName;
             
            ComputerName_ = ComputerName;
            return ComputerName;
        }
        //метод для получения названия CAD системы под которуб пишется приложение
        string GetCADProgramVersion()
        {
            string CADProgramVersion = null;
            CADProgramVersion = Body.KompasVersion;
            CADProgramVersion_ = CADProgramVersion;
            return CADProgramVersion;
        }
        //метод перемешивает данные
        private string Char_Mix(string MotherBoardID, string ProcID, string VolSerial)
        {
            string sOut = "";
            char[] MotherDoardArr = MotherBoardID.ToCharArray();
            char[] ProcIDArr = ProcID.ToCharArray();
            char[] VolSerialArr = VolSerial.ToCharArray();


            int[] arr_lenth = new int[] { MotherBoardID.Length, ProcID.Length, VolSerial.Length };
            int CharMinCount = arr_lenth.Max();
            for (int i = 0; i < CharMinCount; i++)
            {
                string MotherDoardArr_Ind = "0";
                string ProcIDArr_Ind = "1";
                string VolSerialArr_Ind = "2";
                try { MotherDoardArr_Ind = Convert.ToString(MotherDoardArr[i]); } catch (Exception ec) { }
                try { ProcIDArr_Ind = Convert.ToString(ProcIDArr[i]); } catch (Exception ec) { }
                try { VolSerialArr_Ind = Convert.ToString(VolSerialArr[i]); } catch (Exception ec) { }

                sOut += MotherDoardArr_Ind + ProcIDArr_Ind + VolSerialArr_Ind;
            }
            return sOut;
        }
        public string Char_Mix2(string MotherBoardID, string ProcID, string VolSerial, string ProgID, string ProgVersion)
        {
            string sOut = "";
            char[] MotherDoardArr = MotherBoardID.ToCharArray();
            char[] ProcIDArr = ProcID.ToCharArray();
            char[] VolSerialArr = VolSerial.ToCharArray();
            char[] ProgIDArr = ProgID.ToCharArray();
            char[] ProgVersionArr = ProgVersion.ToCharArray();


            int[] arr_lenth = new int[] { MotherBoardID.Length, ProcID.Length, VolSerial.Length };
            int CharMinCount = arr_lenth.Max();
            for (int i = 0; i < CharMinCount; i++)
            {
                string MotherDoardArr_Ind = "0";
                string ProcIDArr_Ind = "1";
                string VolSerialArr_Ind = "2";
                string ProgIDArr_Ind = "2";
                string ProgVersionArr_Ind = "2";
                try { MotherDoardArr_Ind = Convert.ToString(MotherDoardArr[i]); } catch (Exception ec) { }
                try { ProcIDArr_Ind = Convert.ToString(ProcIDArr[i]); } catch (Exception ec) { }
                try { VolSerialArr_Ind = Convert.ToString(VolSerialArr[i]); } catch (Exception ec) { }
                try { ProgIDArr_Ind = Convert.ToString(ProgIDArr[i]); } catch (Exception ec) { }
                try { ProgVersionArr_Ind = Convert.ToString(ProgVersionArr[i]); } catch (Exception ec) { }

                sOut += MotherDoardArr_Ind + ProcIDArr_Ind + VolSerialArr_Ind + ProgIDArr_Ind + ProgVersionArr_Ind + MotherBoardID + ProcID + VolSerial + ProgID + ProgVersion;
            }
            return sOut + DelimerChar + MotherBoardID + DelimerChar + ProcID + DelimerChar + VolSerial + ProgVersion + DelimerChar + ProgID + DelimerChar;
        }
        //процедура считывает все статические параметры для формировангия файла запроса
        public void loadStaticParam() 
        {
            GetMotherBoardID();
            GetProcessorID();
            GetVolumeSerial();
            GetProgramName(true);
            GetProgrammVersion();
            GetCADProgramVersion();
            GetComputerName(false);
        }
        //Метод запускаемый при загрузке приложения
        public bool Form_LoadTrue(bool VisMessagForUser)
        {
            //Данные с целевого компьютера
            //string number = GetMotherBoardID() + GetProcessorID() + GetVolumeSerial();
            string number = Char_Mix2(GetMotherBoardID(), GetProcessorID(), GetVolumeSerial(), GetProgramName(true),GetProgrammVersion());
            LicKey_ = number;
            string fPath = Application.StartupPath + "\\keyfile.dat";
            fPath = OptionClass_.KeyFile_Puth;
            //Файл ключа присутствует
            if (File.Exists(fPath))//C:\\Users\\aidarhanov.n.VEZA-SPB\\Desktop\\keyfile.dat
            {
                if (!DecodeKey(number, fPath))
                {
                    //Clipboard.SetText(number);
                    return false;
                }
                else
                {
                    if (VisMessagForUser)
                    {
                        MessageBox.Show("Ключ лицензии не верный!" + "\n" +
                                        "Сформируйте файл-запрос в Менеджере лицензии (Справка - Менеджере лицензии)." + "\n" +
                                        "Отправьте полученный файл разработчику для получения ключа!",
                                        Application.ProductName + ". Менеджер лицензии", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Clipboard.SetText(number);
                    return true ;
                }
            }
            else
            {
                //Файл ключа отсутствует
                if (VisMessagForUser)
                {
                    MessageBox.Show("Ключ лицензии не верный!" + "\n" +
                                    "Сформируйте файл-запрос в Менеджере лицензии (Справка - Менеджере лицензии)." + "\n" +
                                    "Отправьте полученный файл разработчику для получения ключа!",
                                    Application.ProductName + ". Менеджер лицензии", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Clipboard.SetText(number);
                return true;
            } 
            return true;
        }

        //Метод декриптования файла ключа
        //Возвращает true или false в зависимости от того,
        //совпал ли ключ в файле и входная строка
        public bool DecodeKey(string inString, string path)
        {
            try
            {
                string decryptstring = null;
                byte[] key = new byte[0x20];
                for (int i = 0; i <= 0x1f; i++)
                    key[i] = 0x1f;
                Rijndael.Key = key;
                FileStream fs = new FileStream(path, FileMode.Open);
                byte[] IV = new byte[Rijndael.IV.Length];
                fs.Read(IV, 0, IV.Length);
                Rijndael.IV = IV;
                ICryptoTransform transformer = Rijndael.CreateDecryptor();
                CryptoStream cs = new CryptoStream(fs, transformer, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                decryptstring = sr.ReadToEnd();
                fs.Close();
                if (!(decryptstring == inString))
                    return true;
                else
                    return false;
            }
            catch(Exception Ex)
            {
                return true;
            }
        }
        public bool DecodeKey2strings(string inString, string path)
        {
            try
            {
                string decryptstring = null;
                byte[] key = new byte[0x20];
                for (int i = 0; i <= 0x1f; i++)
                    key[i] = 0x1f;
                Rijndael.Key = key;
                return true;
            }
            catch(Exception ex)
            {
                return true;
            }
        }

    }
}
