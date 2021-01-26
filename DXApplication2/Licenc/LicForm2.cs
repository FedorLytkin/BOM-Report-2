using System;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Management;
using System.IO;
using VSNRM_Kompas.Options;

namespace SaveDXF
{
    public partial class LicForm2 : Form
    {
        RijndaelManaged Rijndael;
        CryptoClass CryptoClass_;
        CFG_Class OptionClass_ = new CFG_Class();
        CompressedFile compressedFile;

        public static string PatternID = "100";
        public static string ThisPCLic;
        public LicForm2()
        {
            InitializeComponent();
            Rijndael = new RijndaelManaged();
            this.Load += new EventHandler(LicForm2_Load);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        { 
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = $"Queryfile_({CryptoClass.ComputerName_}_{CryptoClass.ProgramID_}_{Application.ProductName}_v{Application.ProductVersion})";
            dialog.Filter = "datq files(*.datq)|*.datq";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                GenerateQueryFile(dialog.FileName);
                MessageBox.Show("Файл запрос создан.", Application.ProductName);
            }
        }
        public void GenerateQueryFile(string path)
        { 
            using (StreamWriter sw = new StreamWriter(File.Open(path, FileMode.OpenOrCreate), System.Text.Encoding.GetEncoding(1251)))
            {
                sw.WriteLine(PatternID);
                sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                sw.WriteLine(CryptoClass.CADProgramVersion_);
                sw.WriteLine(CryptoClass.ComputerName_);
                sw.WriteLine(CryptoClass.ProgramVersion_);
                sw.WriteLine(CryptoClass.ProgramID_);
                sw.WriteLine(CryptoClass.ProcessorID_);
                sw.WriteLine(CryptoClass.Volumeserialnumber_);
                sw.WriteLine(CryptoClass.MotherBoardID_);
            }
            if (CryptoClass_ == null) CryptoClass_ = new CryptoClass();
            if (compressedFile == null) compressedFile = new CompressedFile();
            compressedFile.AddCompressedFile(path, true);
        }
        private void LicForm2_Load(object sender, EventArgs e)
        {
            CryptoClass_ = new CryptoClass();
            compressedFile = new CompressedFile();

            CryptoClass_.loadStaticParam();
            ThisPCLic = CryptoClass_.Char_Mix2(CryptoClass.MotherBoardID_, CryptoClass.ProcessorID_, CryptoClass.Volumeserialnumber_, CryptoClass.ProgramID_, CryptoClass.ProgramVersion_);
        }

        private void buttonImortFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ThisPCLic)) return;
            openFileDialog1.FileName = $"keyfile_({CryptoClass.ComputerName_}_{CryptoClass.ProgramID_}_{Application.ProductName}_v{Application.ProductVersion})";
            openFileDialog1.Filter = "dat files(*.dat)|*.dat";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (File.Exists(OptionClass_.KeyFile_Puth))
                        File.Delete(OptionClass_.KeyFile_Puth);
                    File.Copy(openFileDialog1.FileName, OptionClass_.KeyFile_Puth, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при распозновании ключа!", $"{Application.ProductName}. Менеджер лицензии", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    add_LogFile($@"{Path.GetDirectoryName(OptionClass_.KeyFile_Puth)}\err.txt", ex.Message);
                }
                this.Close();
            }
        }
        public void GetKeyFile(string inString, string path)
        {
            byte[] key = new byte[0x20];
            for (int i = 0; i <= 0x1f; i++)
                key[i] = 0x1f;
            Rijndael.Key = key;
            ICryptoTransform transformer = Rijndael.CreateEncryptor();
            FileStream fs = new FileStream(path, FileMode.Create);
            fs.Write(Rijndael.IV, 0, Rijndael.IV.Length);
            CryptoStream cs = new CryptoStream(fs, transformer, CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cs);
            sw.Write(inString);
            sw.Flush();
            cs.FlushFinalBlock();
            sw.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.evernote.com/l/AjJSBG66FZBA7KMBazxxsVvqEjyUddPCVfw/");
        }
        private void add_LogFile(string FileName, string Message)
        {
            StreamWriter sw = new StreamWriter(FileName, false, System.Text.Encoding.Default);
            sw.Write(Message);
            sw.Flush();
            sw.Close();
        }
    }
}
