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
            }
            This_treeList.StateImageList = Donor_treeList.StateImageList;
            if (treeViewEnum == TreeViewEnum.TreeView)
            {
                foreach (TreeListNode Donor_Node in Donor_treeList.Nodes)
                {
                    TreeListNode New_Node = This_treeList.Nodes.Add();
                    AddCellNode(Donor_Node, New_Node);
                    AddNodeDrw((ComponentInfo)Donor_Node.Tag, Donor_Node, New_Node);
                    AddNode(Donor_Node, New_Node);
                }
            }
            else
            {
                foreach (TreeListNode Donor_Node in Donor_treeList.GetNodeList())
                {
                    TreeListNode New_Node = This_treeList.Nodes.Add();
                    AddCellNode(Donor_Node, New_Node);
                    AddNodeDrw((ComponentInfo)Donor_Node.Tag, Donor_Node, New_Node);
                } 
            }
            This_treeList.ExpandAll();
            CalcComponentCout(This_treeList);
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
                            ParamVal = f.Name;
                            break;
                        case "Расположение":
                            ParamVal = f.DirectoryName;
                            break;
                        case "Размер":
                            ParamVal = f.Length;
                            break;
                        case "Тип":
                            ParamVal = f.GetType();
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
                            ParamVal = f.Name;
                            break;
                        case "Расположение":
                            ParamVal = f.DirectoryName;
                            break;
                        case "Размер":
                            ParamVal = f.Length;
                            break;
                        case "Тип":
                            ParamVal = f.GetType();
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
