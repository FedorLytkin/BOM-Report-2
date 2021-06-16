using SaveDXF;
using SaveDXF.OptionsFold;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VSNRM_Kompas.Options
{
    class CopyINIFile
    {
        public CopyINIFile()
        {
            Copy_INI_File();
        }

        private void Copy_INI_File()
        {
            string now_directory = Application.StartupPath + @"\CFG";
            string Main_Directory = $@"C:\Users\Public\Documents\NSoft\{Application.ProductName}\CFG";

            if (Directory.Exists(Main_Directory) != true)
            {
                System.Diagnostics.Process.Start("https://dxfautohelp.wixsite.com/dxfauto/%D0%BA%D0%BE%D0%BD%D1%82%D0%B0%D0%BA%D1%82%D1%8B");
                Directory.CreateDirectory(Main_Directory);
            }
            DirectoryInfo dir = new DirectoryInfo(now_directory);
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath_Main = Path.Combine(Main_Directory, file.Name);
                if (File.Exists(temppath_Main) != true)
                    file.CopyTo(temppath_Main, false);
            }
        }
        public void Copy_INI_FileInOtherFolder(string user_Directory)
        {
            if (string.IsNullOrWhiteSpace(user_Directory)) return;
            string now_directory = Application.StartupPath + @"\CFG";
            if (Directory.Exists(user_Directory) != true)
                Directory.CreateDirectory(user_Directory);
            DirectoryInfo dir = new DirectoryInfo(now_directory);
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath_User = Path.Combine(user_Directory, file.Name);
                if (File.Exists(temppath_User) != true)
                    file.CopyTo(temppath_User, false);
            }
        }
    }
}
