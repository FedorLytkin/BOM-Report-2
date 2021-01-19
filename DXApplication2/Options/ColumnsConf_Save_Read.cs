using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;

namespace VSNRM_Kompas.Options
{
    class ColumnsConf_Save_Read
    { 
        public List<Column_Class> GetColumnsConfig()
        {
            List<Column_Class> column_s = new List<Column_Class>();

            TreeList treeView = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).treeList1;
            foreach (TreeListColumn Column in treeView.Columns)
            {
                Column_Class column_Class = (Column_Class)Column.Tag;
                if (column_Class != null)
                {
                    column_Class.Visible = Column.Visible;
                    column_Class.Index = Column.VisibleIndex;
                    column_s.Add(column_Class);
                }
            }
            return column_s;
        }
        public static List<string> FindParams()
        {
            List<string> Vis_Columns = new List<string>();

            TreeList treeView = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).treeList1;
            foreach (TreeListColumn Column in treeView.Columns)
            {
                if (Column.Visible)
                    Vis_Columns.Add(Column.Name);
            }
            return Vis_Columns;
        }
    }
}
