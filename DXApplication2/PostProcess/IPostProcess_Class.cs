using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.ViewInfo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNRM_Kompas.PostProcess
{
    public class IPostProcess_Class
    {
        Option_Class IOption_Class;
        public List<CheckConditionItem> checkConditionItems;
        public IPostProcess_Class(Option_Class Get_Option_Class)
        {
            IOption_Class = Get_Option_Class;
            checkConditionItems = new List<CheckConditionItem>();
            if (IOption_Class.Check_ProfileValue) checkConditionItems.Add(new CheckConditionItem { ColumnName = "Длина профиля", CheckValue = "", CheckСondition = CheckConditionItem.CheckСondition_Unum.equal, SetColor = Color.Red });
        }
        public Color TreeListNode_PostProcess(TreeListNode INode, Color defColor)
        {
            foreach (CheckConditionItem checkConditionItem in checkConditionItems)
            {
                TreeListColumn column = INode.TreeList.Columns[checkConditionItem.ColumnName];
                if (column != null)
                {
                    object Val = INode.GetValue(checkConditionItem.ColumnName);
                    switch (checkConditionItem.CheckСondition)
                    {
                        case CheckConditionItem.CheckСondition_Unum.equal:
                            if (Val == checkConditionItem.CheckValue)
                                return checkConditionItem.SetColor;
                            break;
                    }
                }
            }
            return defColor;
        }
        private void Treelist_PostProcess(TreeList treeList)
        {
            foreach (CheckConditionItem checkConditionItem in checkConditionItems)
            {
                if (treeList.Columns[checkConditionItem.ColumnName] !=null)
                    foreach (TreeListNode node in treeList.GetNodeList())
                    {
                        object Val = node.GetValue(checkConditionItem.ColumnName);
                        switch (checkConditionItem.CheckСondition)
                        {
                            case CheckConditionItem.CheckСondition_Unum.equal:
                                if (Val == checkConditionItem.CheckValue)
                                {
                                    CellInfo cell = treeList.ViewInfo.RowsInfo[node].Cells[treeList.Columns[checkConditionItem.ColumnName].VisibleIndex] as CellInfo;
                                    cell.PaintAppearance.BackColor = checkConditionItem.SetColor;
                                    cell.PaintAppearance.BackColor2 = checkConditionItem.SetColor;
                                    cell.PaintAppearance.BorderColor = checkConditionItem.SetColor;
                                    //treeList.ViewInfo.RowsInfo[node].Cells[treeList.Columns[checkConditionItem.ColumnName].AbsoluteIndex].PaintAppearance.BackColor = checkConditionItem.SetColor;
                                }
                                break;
                            case CheckConditionItem.CheckСondition_Unum.not_equal:
                                if (Val != checkConditionItem.CheckValue)
                                    treeList.ViewInfo.RowsInfo[node].Cells[treeList.Columns[checkConditionItem.ColumnName].AbsoluteIndex].PaintAppearance.BackColor = checkConditionItem.SetColor;
                                break;
                                //case CheckConditionItem.CheckСondition_Unum.less:
                                //    if (Val < checkConditionItem.CheckValue)
                                //        treeList.ViewInfo.RowsInfo[node].Cells[treeList.Columns[checkConditionItem.ColumnName].AbsoluteIndex].PaintAppearance.BackColor = checkConditionItem.SetColor;
                                //    break;
                        }
                    }
            }

            
        }
        public class CheckConditionItem
        {
            public string ColumnName { get; set; }
            public object CheckValue { get; set; }
            public CheckСondition_Unum CheckСondition { get; set; }
            public Color SetColor { get; set; }

            public enum CheckСondition_Unum
            {
                equal = 0,
                more = 1,
                less = 2,
                not_equal = 3
            }
        }
    }
}
