using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNRM_Kompas.ProjectClone
{
    public class FindAndRepace_Class
    {
        public Find_Method_Enum find_Method_Enum = Find_Method_Enum.Check_Elements;
        public string Replace_Text = null;
        public bool Register_Without = true;
        public bool To4noe = false;
        public TreeList treeList;
        List<TreeListNode> Find_treeListNodes;
        int NextStep;
        public enum Find_Method_Enum
        {
            Check_Elements = 0,
            Not_Check_Elements = 1,
            RepaceText = 2
        } 
        public List<string> GetTreeListColumnsName()
        {
            List<string> Col_List = new List<string>();
            if (treeList == null) return Col_List;

            foreach (TreeListColumn Column in treeList.Columns)
            {
                if(Column.FieldName != "Миниатюра" && Column.FieldName != "Размер" && Column.FieldName != "Тип")
                    Col_List.Add(Column.Caption);
            }

            return Col_List;
        }
        public void FindTextList(string ColumnName, string FindText)
        { 
            Find_treeListNodes = new List<TreeListNode>();
            foreach (TreeListNode node in treeList.GetNodeList())
            {
                if (node.GetValue(ColumnName).ToString().Contains(FindText))
                    Find_treeListNodes.Add(node);
            }
            NextStep = 0;
        }
        public void FindNext(string ColumnName, string FindText, string ReplaceText)
        {
            if (NextStep >= Find_treeListNodes.Count) NextStep = 0;
            for (int ii = NextStep; ii < Find_treeListNodes.Count; ii++)
            {
                TreeListNode node = Find_treeListNodes[ii];
                switch (find_Method_Enum)
                {
                    case Find_Method_Enum.Check_Elements:
                        if (node.GetValue(ColumnName).ToString().Contains(FindText))
                            node.Checked = true;
                        else
                            node.Checked = false;
                        break;
                    case Find_Method_Enum.Not_Check_Elements:
                        if (node.GetValue(ColumnName).ToString().Contains(FindText))
                            node.Checked = false;
                        break;
                    case Find_Method_Enum.RepaceText:
                        string Val = node.GetValue(ColumnName).ToString();
                        if (node.GetValue(ColumnName).ToString().Contains(FindText))
                            node.SetValue("Сохранить в имени", node.GetValue("Сохранить в имени").ToString().Replace(FindText, ReplaceText));
                        break;
                }
                NextStep += 1;
                return;
            }
        }
        public void FindAll(string ColumnName, string FindText, string ReplaceText)
        {
            foreach (TreeListNode node in treeList.GetNodeList())
            {
                switch (find_Method_Enum)
                {
                    case Find_Method_Enum.Check_Elements:
                        if (node.GetValue(ColumnName).ToString().Contains(FindText))
                            node.Checked = true;
                        else
                            node.Checked = false;
                        break;
                    case Find_Method_Enum.Not_Check_Elements:
                        if (node.GetValue(ColumnName).ToString().Contains(FindText))
                            node.Checked = false; 
                        break;
                    case Find_Method_Enum.RepaceText:
                        string Val = node.GetValue(ColumnName).ToString();
                        if (node.GetValue(ColumnName).ToString().Contains(FindText))
                            node.SetValue("Сохранить в имени", node.GetValue("Сохранить в имени").ToString().Replace(FindText, ReplaceText));
                        break;
                }
            }
        }
    }
}
