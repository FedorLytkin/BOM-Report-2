using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VSNRM_Kompas.Options;
using SaveDXF;
using System.IO.Compression;
using System.Diagnostics;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using VSNRM_Kompas.API_Toops;
using DevExpress.XtraSplashScreen;

namespace VSNRM_Kompas.Dump
{
    class Dump
    {
        OptionClass Option;
        string NowDumpFolder;
        public static string ModelFolderInDump;
        string resultZipFile;
        SplashScreenManager waitMng;
        //создать папку DUMP *
        //скопировать файл exe *
        //скопировать файл настроек *
        //скопировать файлa запросы к БД *
        //скопировть файл-запрос *
        //скопировть файл-модели
        //архивировать данные созданной папки *
        //удалить созданную папку DUMP *
        //открыть каталог в котором расположен архив *
        public Dump()
        {
            waitMng = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).splashScreenManager2;
            Option = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).Main_Options;
            CreateDumpFolder();
            CopyEXEFile();
            CopyINIFiles();
            CopyBDQueryFiles();
            Create_QueryFile();
            CopyModelFiles();
            Create_ZipFile();
            Delete_DumpFolder();
            Open_DumpFolder();
        }
        private void AddWaitStatus(string Text)
        {
            waitMng.SetWaitFormDescription(Text);
        }
        void CreateDumpFolder()
        {
            string DumpsFld = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\DUMPS";
            if (!Directory.Exists(DumpsFld))
                Directory.CreateDirectory(DumpsFld);
            NowDumpFolder = $@"{DumpsFld}\Dump_{Application.ProductName}_{DateTime.Now.ToString("dd.MM.yyyy HH.mm")}";

            if (!Directory.Exists(NowDumpFolder))
                Directory.CreateDirectory(NowDumpFolder);
            AddWaitStatus("Создана директория для Dump");
        }
        void CopyEXEFile()
        {
            string EXE_File = Application.ExecutablePath;
            File.Copy(EXE_File, $@"{NowDumpFolder}\{Path.GetFileName(EXE_File)}", true);
            AddWaitStatus("Скопирован исполняемый файл");
        }
        void CopyINIFiles()
        {
            string INIFolderPath = $@"C:\Users\Public\Documents\NSoft\{Application.ProductName}\CFG";
            string INIFolderInDump = $@"{NowDumpFolder}\CFG";
            if (!Directory.Exists(INIFolderInDump))
                Directory.CreateDirectory(INIFolderInDump);

            DirectoryInfo dir = new DirectoryInfo(INIFolderPath);
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath_Main = Path.Combine(INIFolderInDump, file.Name);
                if (File.Exists(temppath_Main) != true)
                    file.CopyTo(temppath_Main, false);
            }
            AddWaitStatus("Скопированы настройки");
        }
        void CopyBDQueryFiles()
        {
            string QueryFolderPath = $@"{Application.StartupPath}\Queries";
            string QueryFolderInDump = $@"{NowDumpFolder}\Queries";
            if (!Directory.Exists(QueryFolderInDump))
                Directory.CreateDirectory(QueryFolderInDump);

            DirectoryInfo dir = new DirectoryInfo(QueryFolderPath);
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath_Main = Path.Combine(QueryFolderInDump, file.Name);
                if (File.Exists(temppath_Main) != true)
                    file.CopyTo(temppath_Main, false);
            }
            AddWaitStatus("Скопированы запросы");
        }
        void CopyModelFiles()
        {
            ModelFolderInDump = $@"{NowDumpFolder}\Models";
            Body body = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).body;
            TreeList treeView = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).treeList1;
            foreach (TreeListNode node in treeView.Nodes)
            {
                ComponentInfo component = (ComponentInfo)node.Tag; 
                body.SetSourseChancge(component.FFN, $@"{ModelFolderInDump}\{Directory.GetParent(component.FFN).Name}\{Path.GetFileName(component.FFN)}");
            }
            AddWaitStatus("Скопированы модели");
        }
        void Create_QueryFile()
        {
            string QueryFile = $@"{NowDumpFolder}\Query.datq";
            LicForm2 licForm2 = new LicForm2();
            licForm2.GenerateQueryFile(QueryFile);
            AddWaitStatus("Подготовка системной информации");
        }

        void Create_ZipFile()
        {
            resultZipFile = $"{NowDumpFolder}.ZIP";
            ZipFile.CreateFromDirectory(NowDumpFolder, resultZipFile);
            AddWaitStatus("Создание архива");
        }
        void Delete_DumpFolder()
        {
            try
            {
                if (Directory.Exists(NowDumpFolder))
                    Directory.Delete(NowDumpFolder, true);
            }
            catch { }
            AddWaitStatus("Удаление буфера");
        }
        void Open_DumpFolder()
        {
            if (Directory.Exists(Path.GetDirectoryName(NowDumpFolder)))
                Process.Start(Path.GetDirectoryName(NowDumpFolder));
        }
    }
}
