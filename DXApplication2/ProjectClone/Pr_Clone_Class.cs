using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VSNRM_Kompas.API_Toops;

namespace VSNRM_Kompas.ProjectClone
{
    public class Pr_Clone_Class
    {
        public TreeList Donor_treeList;
        public TreeList This_treeList;
        public FindAndRepace_Class FindAndRepace;
        public bool check_Drw = false;
        public bool check_SP = false;
        public TreeViewEnum treeViewEnum = TreeViewEnum.TreeView;

        public SaveEnum saveEnum = SaveEnum.InFolder;
        public string FolderPath;
        public string SourseFolderPath;
        public string ZipFileName;
        
        public string Prefix_Value;
        public string Sufix_Value;
        public bool AddSufix = false;
        public bool AddPrefix = false;

        public bool SaveInOneFolder = true;
        List<string> ColList;
        public LabelControl LB_Sborka;
        public LabelControl LB_Part;
        public LabelControl LB_Drw;
        public LabelControl LB_SP;

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

        public Pr_Clone_Class()
        {
            AddColumnList();
        }
        public void Bild_Tree()
        {
            if (This_treeList == null) return;
            This_treeList.Nodes.Clear();
            This_treeList.Columns.Clear();
            //This_treeList.StateImageList = null;
            for (int i = 0; i < ColList.Count; i++)
            {
                string colName = ColList[i];
                TreeListColumn Par_column = This_treeList.Columns.Add();
                Par_column.Caption = colName;
                Par_column.FieldName = colName;
                Par_column.Name = colName;
                Par_column.VisibleIndex = i;
                Par_column.Visible = true;
                Par_column.OptionsColumn.ReadOnly = true;
                Par_column.OptionsColumn.AllowEdit = false;
            }
            This_treeList.StateImageList = Donor_treeList.StateImageList;
            if (treeViewEnum == TreeViewEnum.TreeView)
            {
                foreach (TreeListNode Donor_Node in Donor_treeList.Nodes)
                {
                    SourseFolderPath = Path.GetDirectoryName(((ComponentInfo)Donor_Node.Tag).FFN);
                    TreeListNode New_Node = This_treeList.Nodes.Add();
                    AddCellNode(Donor_Node, New_Node);
                    AddNodeDrw((ComponentInfo)Donor_Node.Tag, Donor_Node, New_Node);
                    AddNode(Donor_Node, New_Node);
                }
            }
            else
            {
                List <TreeListNode> AllNodes = Donor_treeList.GetNodeList();
                if (AllNodes.Count > 0) SourseFolderPath = Path.GetDirectoryName(((ComponentInfo)AllNodes[0].Tag).FFN);
                foreach (TreeListNode Donor_Node in AllNodes)
                {
                    TreeListNode New_Node = This_treeList.Nodes.Add();
                    AddCellNode(Donor_Node, New_Node);
                    AddNodeDrw((ComponentInfo)Donor_Node.Tag, Donor_Node, New_Node);
                } 
            }
            This_treeList.ExpandAll();
            This_treeList.CheckAll();
            CalcComponentCout(This_treeList);
            This_treeList.Columns["Сохранить в имени"].OptionsColumn.ReadOnly = false;
            This_treeList.Columns["Сохранить в имени"].OptionsColumn.AllowEdit = true;
        }
        public void SetOutFolderPathInComponents()
        {
            if (This_treeList == null) return;
            foreach (TreeListNode node in This_treeList.GetNodeList())
            {
                if(SaveInOneFolder)
                    node.SetValue("Сохранить в папке", FolderPath);
                else
                    node.SetValue("Сохранить в папке", getResultFolderPath(FolderPath, SourseFolderPath, ((ComponentInfo)node.Tag).FFN));
            }
        }
        public void CalcComponentCout(TreeList treeList)
        {
            if (treeList == null) return;
            int Assembly_Count = 0;
            int Part_Count = 0;
            int Drw_Count = 0;
            int SP_Count = 0;
            foreach (TreeListNode node in treeList.GetNodeList())
            {
                ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                switch (Path.GetExtension(componentInfo.FFN).ToUpper())
                {
                    case ".A3D":
                        Assembly_Count+=1;
                        break;
                    case ".M3D":
                        Part_Count += 1;
                        break;
                    case ".CDW":
                        Drw_Count += 1;
                        break;
                    case ".SPW":
                        SP_Count += 1;
                        break;
                }
            }
            LB_Sborka.Text = $"Сборки: {Assembly_Count}";
            LB_Part.Text = $"Детали: {Part_Count}";
            LB_Drw.Text = $"Чертежи: {Drw_Count}";
            LB_SP.Text = $"Спецификации: {SP_Count}";
        }
        private void AddNodeDrw(ComponentInfo Donor_componentInfo, TreeListNode Donor_Node, TreeListNode Article_With_Drw_Node)
        {

            foreach (ComponentInfo.Drw_Info_Class Drw in Donor_componentInfo.drw_List)
            {
                switch (Path.GetExtension(Drw.FFN).ToUpper())
                {
                    case ".SPW":
                        if (check_SP)
                        {
                            TreeListNode Drwnode = Article_With_Drw_Node.Nodes.Add();
                            AddCellNode(Donor_Node, Drwnode, Drw);
                        }
                        break;
                    case ".CDW":
                        if (check_Drw)
                        {
                            TreeListNode Drwnode = Article_With_Drw_Node.Nodes.Add();
                            AddCellNode(Donor_Node, Drwnode, Drw);
                        }
                        break;
                }
            }
        }
        private void AddNode(TreeListNode Donor_Node, TreeListNode New_Node)
        {
            foreach (TreeListNode This_Node in Donor_Node.Nodes)
            {
                ComponentInfo componentInfo = (ComponentInfo)This_Node.Tag;
                if(!componentInfo.HaveDrw && !componentInfo.HaveSP)
                {
                    TreeListNode node = New_Node.Nodes.Add();
                    AddCellNode(This_Node, node);
                    AddNodeDrw(componentInfo, This_Node, node);
                    if (This_Node.Nodes.Count > 0) AddNode(This_Node, node);
                }
            }
        }
        private void AddCellNode(TreeListNode Donor_Node, TreeListNode New_Node, ComponentInfo.Drw_Info_Class Drw_Info)
        {
            FileInfo f = new FileInfo(Drw_Info.FFN);
            foreach (string ParamName in ColList)
            {
                object ParamVal = Donor_Node.GetValue(ParamName);
                if (ParamVal == null)
                {
                    switch (ParamName)
                    {
                        case "Сохранить в имени":
                            ParamVal = $@"{Prefix_Value}{Path.GetFileNameWithoutExtension(f.Name)}{Sufix_Value}";
                            break;
                        case "Расположение":
                            ParamVal = f.DirectoryName;
                            break;
                        case "Размер":
                            ParamVal = $"{f.Length/1024} КБ";
                            break;
                        case "Тип":
                            ParamVal = f.Extension;
                            break;
                        case "Сохранить в папке":
                            if(SaveInOneFolder)
                                ParamVal = FolderPath;
                            else
                                ParamVal = getResultFolderPath(FolderPath, SourseFolderPath, Drw_Info.FFN);
                            break;
                    }
                }
                New_Node.SetValue(ParamName, ParamVal);
            }
            New_Node.SetValue("Миниатюра", Drw_Info.Slide);
            switch (Path.GetExtension(Drw_Info.FFN).ToUpper())
            {
                case ".CDW":
                    New_Node.ImageIndex = 10;
                    New_Node.StateImageIndex = 10;
                    New_Node.SelectImageIndex = 10;
                    break;
                case ".SPW":
                    New_Node.ImageIndex = 11;
                    New_Node.StateImageIndex = 11;
                    New_Node.SelectImageIndex = 11;
                    break;
            }
            ComponentInfo componentInfo = (ComponentInfo)Donor_Node.Tag;
            ComponentInfo Drw_componentInfo = (ComponentInfo)componentInfo.Clone();
            Drw_componentInfo.FFN = Drw_Info.FFN;
            Drw_componentInfo.Slide = Drw_Info.Slide;
            Drw_componentInfo.Key = Drw_Info.FFN + "|" + Drw_Info.Oboz;
            Drw_componentInfo.drw_Info  = Drw_Info;

            New_Node.Tag = Drw_componentInfo;
        }
        private void AddCellNode(TreeListNode Donor_Node, TreeListNode New_Node)
        {
            ComponentInfo componentInfo = (ComponentInfo)Donor_Node.Tag;
            FileInfo f = new FileInfo(componentInfo.FFN);
            foreach (string ParamName in ColList)
            {
                object ParamVal = Donor_Node.GetValue(ParamName);
                if (ParamVal == null)
                {
                    switch (ParamName)
                    {
                        case "Сохранить в имени":
                            ParamVal = $@"{Prefix_Value}{Path.GetFileNameWithoutExtension(f.Name)}{Sufix_Value}";
                            break;
                        case "Расположение":
                            ParamVal = f.DirectoryName;
                            break;
                        case "Размер":
                            ParamVal = $"{f.Length / 1024} КБ";
                            break;
                        case "Тип":
                            ParamVal = f.Extension;
                            break;
                        case "Сохранить в папке":
                            ParamVal = FolderPath;
                            break;
                    }
                }
                New_Node.SetValue(ParamName, ParamVal);
            }
            New_Node.SetValue("Миниатюра", componentInfo.Slide);

            New_Node.ImageIndex = Donor_Node.ImageIndex;
            New_Node.StateImageIndex = Donor_Node.StateImageIndex;
            New_Node.SelectImageIndex = Donor_Node.SelectImageIndex;
            New_Node.Tag = Donor_Node.Tag;
        }
        private void AddColumnList()
        {
            ColList = new List<string>();
            ColList.Add("Миниатюра");
            ColList.Add("Имя файла");
            ColList.Add("Расположение");
            ColList.Add("Сохранить в имени");
            ColList.Add("Сохранить в папке");
            ColList.Add("Размер");
            ColList.Add("Тип");
        }
        public string GetFolderName()
        {
            XtraFolderBrowserDialog folderdialog = new XtraFolderBrowserDialog();
            folderdialog.DialogStyle = DevExpress.Utils.CommonDialogs.FolderBrowserDialogStyle.Wide;
            if (folderdialog.ShowDialog() == DialogResult.OK)
            {
                FolderPath = folderdialog.SelectedPath;
                SetOutFolderPathInComponents();
                return folderdialog.SelectedPath;
            }
            return null;
        }
        public string GetZipFileName()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Укажите имя ZIP-файла и место его хранения";
            saveFileDialog.Filter = "ZIP файлы (*.ZIP)|*.ZIP";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ZipFileName = saveFileDialog.FileName;
                return saveFileDialog.FileName;
            }
            return null;
        }
        public void SetPrefixAndSufix()
        {
            foreach(TreeListNode node in This_treeList.GetNodeList())
            {
                ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                if (!string.IsNullOrEmpty(node.GetValue("Сохранить в имени").ToString()))
                    node.SetValue("Сохранить в имени", $@"{Prefix_Value}{Path.GetFileNameWithoutExtension(componentInfo.FFN)}{Sufix_Value}");
                //if (string.IsNullOrEmpty(node.GetValue("Сохранить в имени").ToString()) || string.IsNullOrEmpty(Prefix_Value) || string.IsNullOrEmpty(Sufix_Value))
                //    node.SetValue("Сохранить в имени", $@"{Prefix_Value}{Path.GetFileNameWithoutExtension(componentInfo.FFN)}{Sufix_Value}");
                //else
                //    node.SetValue("Сохранить в имени", $@"{Prefix_Value}{node.GetValue("Сохранить в имени").ToString()}{Sufix_Value}");
            }
        }
        private string getResultFolderPath(string OutFolderPath, string SoursFolderPath, string PartFileName)
        {
            string ResultFolderPath = null;
            foreach (string FolderName in Path.GetDirectoryName(PartFileName).Split('\\'))
            {
                ResultFolderPath += $@"{FolderName}\";
                if (ResultFolderPath.Trim('\\') == SoursFolderPath)
                    return Path.GetDirectoryName(PartFileName).Replace(SoursFolderPath, OutFolderPath);
            }
            return OutFolderPath;
        }
        public string getFreeFileName(string FileName)
        {
            while (File.Exists(FileName))
                FileName = $@"{Path.GetDirectoryName(FileName)}\{Path.GetFileNameWithoutExtension(FileName)}_1{Path.GetExtension(FileName)}";
            return FileName;
        }
        public string getFileNameWithFindOptions(string FullFileName)
        {
            //if (FindAndRepace.To4noe)
            //    if(FullFileName == FindAndRepace.fin)

            if (SaveInOneFolder)
                return FolderPath + $@"\{Prefix_Value}{Path.GetFileNameWithoutExtension(FullFileName)}{Sufix_Value}{Path.GetExtension(FullFileName)}";
            else
                return getResultFolderPath(FolderPath, SourseFolderPath, FullFileName) + $@"\{Prefix_Value}{Path.GetFileNameWithoutExtension(FullFileName)}{Sufix_Value}{Path.GetExtension(FullFileName)}";
            return FullFileName;
        }
        public string getSourceFileNameByExpFN(string ExportFileName)
        {
            foreach (TreeListNode Node in This_treeList.GetNodeList())
            {
                if(Node.GetValue("Имя файла").ToString() == Path.GetFileNameWithoutExtension(ExportFileName)) 
                    return ((ComponentInfo)Node.Tag).FFN; 
            }
            return null;
        }
    }
}
