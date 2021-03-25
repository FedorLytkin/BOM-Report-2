using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
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
                Col_List.Add(Column.Caption);

            return Col_List;
        }
    }
}
