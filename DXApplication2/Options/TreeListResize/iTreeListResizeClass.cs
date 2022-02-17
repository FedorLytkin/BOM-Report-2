using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using VSNRM_Kompas.API_Toops;

namespace VSNRM_Kompas.Options.TreeListResize
{
    public class iTreeListResizeClass
    {
        GridView GridView;
        CFG_Class IOptions;
        public iTreeListResizeClass(GridView GetGridView, CFG_Class GetIOptions)
        {
            GridView = GetGridView;
            IOptions = GetIOptions;
        }
        public static void ReSize(GridView GetGridView, CFG_Class GetIOptions, TreeList treeList, int Size)
        {
            for (int i = 0; i < GetGridView.RowCount; i++)
            {
                DataRowView row = (DataRowView)GetGridView.GetRow(i);
                ComponentInfo componentInfo = (ComponentInfo)row["System"];
                row["Миниатюра"] = BitmapClass.resizeImage(componentInfo.LargeSlide, Size);
            }
            foreach(TreeListNode node in treeList.GetNodeList())
            {
                ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                if(node.StateImageIndex ==  (int)Option_Class.Obj_Type_Enum.Drawing || node.StateImageIndex == (int)Option_Class.Obj_Type_Enum.Specification)
                    node["Миниатюра"] = BitmapClass.resizeImage(componentInfo.drw_Info.LargeSlide, Size);
                else
                    node["Миниатюра"] = BitmapClass.resizeImage(componentInfo.LargeSlide, Size);
            }
        }

    }
}
