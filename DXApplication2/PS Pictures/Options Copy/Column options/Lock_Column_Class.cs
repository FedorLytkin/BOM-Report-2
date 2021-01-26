using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VSNRM_Kompas.Options.Column_Options
{
    public class Lock_Column_Class
    {
        TreeList treeView;  
        public List<Column_Class> Lock_Column { get; set; }
        public bool Lock_Column_result { get; set; }
        public Lock_Column_Class()
        {
            Lock_Column_result = false;
            treeView = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).treeList1; 
            Demo_User_Columns();
        } 
        private List<Column_Class> GetDemoUserColumn_ColumnClass()
        {
            ColumnsConf_Save_Read columnsConf_Save_Read = new ColumnsConf_Save_Read();
            
            List<Column_Class> DemoUserCl_List = new List<Column_Class>();
            int Demo_Col_Valid_Qnt = 0;
            foreach (Column_Class column_Class in columnsConf_Save_Read.GetColumnsConfig())
            { 
                if (column_Class != null)
                {
                    if (!column_Class.System && column_Class.Visible)
                    {
                        Demo_Col_Valid_Qnt += 1;
                        if (Demo_Col_Valid_Qnt > 2)
                            DemoUserCl_List.Add(column_Class);
                    }
                }
            }
            return DemoUserCl_List;
        }
        private void Demo_User_Columns()
        {
            List<Column_Class> gridColumns = GetDemoUserColumn_ColumnClass();
            string LockColumnsName = "";
            foreach (Column_Class column in gridColumns)
            {
                LockColumnsName += $", {column.Caption}";
            }
            if (gridColumns.Count > 0)
            {
                MessageBox.Show($"В Demo-режиме программы {Application.ProductName}, сканирование свойств возможно только по 2 пользовательским столбцам!" +
                    $"\nВ пользовательские столбцы: {LockColumnsName.Trim().Trim(',')}, не будут записаны соответствующие значения из моделей!" +
                    $"\nВ полной версии приложения {Application.ProductName}, Вы можете добавлять неограниченное количество пользовательских столбцов!" +
                    $"\nДля использования всех функций программы, обратитесь к разработчику приложения (Справка - Контакты)", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Lock_Column_result = true;
            }
            Lock_Column = gridColumns;
        }
    }
}
