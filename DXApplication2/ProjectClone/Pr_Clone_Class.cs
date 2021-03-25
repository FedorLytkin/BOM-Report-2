using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VSNRM_Kompas.ProjectClone
{
    public class Pr_Clone_Class
    {
        public bool check_Drw = false;
        public bool check_SP = false;
        public TreeViewEnum treeViewEnum = TreeViewEnum.TreeView;

        public SaveEnum saveEnum = SaveEnum.InFolder;
        public string FolderPath;
        public string ZipFileName;
        
        public string Prefix_Value;
        public string Sufix_Value;
        public bool AddSufix = false;
        public bool AddPrefix = false;

        public bool SaveInOneFolder = false;

        public enum TreeViewEnum
        {
            TreeView = 0,
            GridCiew = 1
        }
        public enum SaveEnum
        {
            InFolder = 0,
            InZipFile = 1
        }

        public string GetFolderName()
        {
            FolderBrowserDialogEx folderdialog = new FolderBrowserDialogEx();
            
            if (folderdialog.ShowDialog() == DialogResult.OK)
                return folderdialog.SelectedPath;
            return null;
        }
        public string GetZipFileName()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Укажите место хранения и имя ZIP-файла";
            saveFileDialog.Filter = "ZIP файлы (*.ZIP)|*.ZIP";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                return saveFileDialog.FileName;
            return null;
        }
    }
}
