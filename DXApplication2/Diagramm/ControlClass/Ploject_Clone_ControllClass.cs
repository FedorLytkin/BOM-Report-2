using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using SaveDXF;
using VSNRM_Kompas.API_Toops;
using System.IO;
using DevExpress.XtraTreeList.Nodes.Operations;

namespace VSNRM_Kompas.Diagramm.ControlClass
{
    public class Ploject_Clone_ControllClass
    {
        TreeList treeList;
        Body CADBody;
        List<string> ColList;
        public Ploject_Clone_ControllClass(Body GetCadBody, TreeList GetTreeList)
        {
            CADBody = GetCadBody;
            treeList = GetTreeList;
            AddColumnList();
        }
        private void AddColumnList()
        {
            ColList = new List<string>();
            //ColList.Add("Миниатюра");
            ColList.Add("Имя файла");
            ColList.Add("Расположение");
            ColList.Add("Сохранить в имени");
            ColList.Add("Сохранить в папке");
            ColList.Add("Размер");
            ColList.Add("Тип");
        }
        public TreeList GetTreeViewInPC(TreeList Parent_TreeList)
        {
            TreeList treeList = new TreeList();
            for(int i = 0; i < ColList.Count; i++)
            {
                string colName = ColList[i];
                TreeListColumn Par_column = treeList.Columns.Add(); 
                Par_column.Caption = colName;
                Par_column.FieldName = colName;
                Par_column.Name = colName;
                Par_column.VisibleIndex = i;
                Par_column.Visible = true;
                //treeList.Columns.Add(Par_column);
            }
            //if (Parent_TreeList.Nodes.Count > 0)
            //{
            //    TreeListNode newNode = treeList.Nodes.Add();
            //    AddCellNode(Parent_TreeList.Nodes[0], newNode);
            //    AddNode(Parent_TreeList.Nodes[0], newNode);
            //}
            return Parent_TreeList;
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
                        case "Сохранить в папке":
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
        }
        private void AddNode(TreeListNode Donor_Node, TreeListNode New_Node)
        {
            foreach (TreeListNode This_Node in Donor_Node.Nodes)
            {
                TreeListNode node = New_Node.Nodes.Add();
                AddCellNode(This_Node, node);
                if (This_Node.Nodes.Count > 0) AddNode(This_Node, node);
            }
        }
    }
    public class CopyNodesOperation : TreeListOperation
    {
        TreeList DestTreeList;

        public CopyNodesOperation(TreeList destTreeList)
        {
            if (destTreeList == null)
                throw new ArgumentNullException("destTreeList");
            this.DestTreeList = destTreeList;
        }

        public override void Execute(TreeListNode node)
        {
            object[] values = new object[node.TreeList.Columns.Count];
            for (int i = 0; i < node.TreeList.Columns.Count; i++)
                values[i] = node.GetValue(i);
            if (node.ParentNode == null)
                DestTreeList.AppendNode(values, null);
            else DestTreeList.AppendNode(values, node.ParentNode.Id);
        }
    }
}
