using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNRM_Kompas.Options.Column_options
{
    public class AddNewColumns_Class
    {
        private List<Column_Class> NewSystemColumnsList;
        private List<Column_Class> NewCustomColumnsList;
        public AddNewColumns_Class()
        {
            LoadColumnsList();
        }
        public List<Column_Class> GetUpdateColumnList(List<Column_Class> column_List)
        {
            foreach (Column_Class New_column in NewSystemColumnsList)
            {
                bool ColumnExist = false;
                foreach (Column_Class App_column in column_List)
                {
                    if (New_column.Name == App_column.Name) { ColumnExist = true; break; }
                }
                if (!ColumnExist) column_List.Add(New_column);
            }
            foreach (Column_Class New_column in NewCustomColumnsList)
            {
                bool ColumnExist = false;
                foreach (Column_Class App_column in column_List)
                {
                    if (New_column.Name == App_column.Name) { ColumnExist = true; break; }
                }
                if (!ColumnExist) column_List.Add(New_column);
            }

            return column_List;
        }
        void LoadColumnsList()
        {
            NewSystemColumnsList = new List<Column_Class>();
            NewCustomColumnsList = new List<Column_Class>();
            string[] sys_colNames = new string[] {"Путь файла", "Расположение файла", "Тип объекта"};
            string[] custom_colNames = new string[] {"Раздел спецификации", "Габарит"};


            foreach (string ColName in sys_colNames)
            {
                Column_Class column_ = new Column_Class();
                column_.Index = -1;
                column_.Visible = false;
                column_.System = true;

                column_.Caption = ColName;
                column_.FieldName = ColName;
                column_.Name = ColName;
                NewSystemColumnsList.Add(column_);
            }

            foreach (string ColName in custom_colNames)
            {
                Column_Class column_ = new Column_Class();
                column_.Index = -1;
                column_.Visible = false;
                column_.System = false;

                column_.Caption = ColName;
                column_.FieldName = ColName;
                column_.Name = ColName;
                NewCustomColumnsList.Add(column_);
            }
        }
    }
}
