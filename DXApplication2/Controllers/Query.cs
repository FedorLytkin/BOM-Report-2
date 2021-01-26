using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using VSNRM_Kompas.Options;

namespace VSNRM_Kompas.Controllers
{
    public class Query
    {
        OleDbConnection     connection;
        OleDbCommand        command;
        OleDbDataAdapter    DataAdapter; 
        DataTable           bufferTable; 
        OleDbConnection     Column_List; 
        public Query(string Conn)
        {
            connection = new OleDbConnection(Conn);
            Column_List = new OleDbConnection(Conn);
            bufferTable = new DataTable(); 
        }
        public enum Select_Option
        {
            ColumsList = 0, 
            Options = 1,
            DeleteColumns = 2,
            Pogovorki = 3,
            SystemInformation = 4
        }
        private string AddQueryFile(Select_Option selectGr)
        {
            string Result = null;
            try
            {
                string AppPath = Application.StartupPath;
                string FileQuery_Path = null;
                switch (selectGr)
                {
                    case Select_Option.ColumsList:
                        FileQuery_Path = AppPath + @"\Queries\Columns.qrs";
                        break;
                    case Select_Option.Options:
                        FileQuery_Path = AppPath + @"\Queries\Options.qrs";
                        break;
                    case Select_Option.DeleteColumns:
                        FileQuery_Path = AppPath + @"\Queries\DeleteColumns.qrs";
                        break; 
                    case Select_Option.Pogovorki:
                        FileQuery_Path = AppPath + @"\Queries\Sayings.qrs";
                        break;
                    case Select_Option.SystemInformation:
                        FileQuery_Path = AppPath + @"\Queries\SystemInformation.qrs";
                        break;
                }
                using (System.IO.StreamReader r = new System.IO.StreamReader(FileQuery_Path, System.Text.Encoding.GetEncoding(1251)))
                {
                    string line = r.ReadToEnd();
                    Result = line;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при чтении настроек" + Environment.NewLine + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return Result;
        }
        public DataTable Get_Data(Select_Option select_Option)
        {
            DataTable bufferTable = new DataTable();

            connection.Open();
            string commandQuery = AddQueryFile(select_Option);
            DataAdapter = new OleDbDataAdapter(commandQuery, connection);

            bufferTable.Clear();
            DataAdapter.Fill(bufferTable);
            connection.Close();  
            return bufferTable;
        }
//        public string Get_Option_FieldValue(OptionClass.OptionsListEnum OptionsList)
//        {
//            string OptVal = "";
//            string CommantSQL = $@"SELECT tbl_Options.Opt_Name, tbl_Options.Opt_Val
//FROM tbl_Options
//WHERE tbl_Options.Opt_Name = '{OptionClass.Get_Opt_Name_By_OptionsList(OptionsList)}'";
//            connection.Open();
//            command = new OleDbCommand(CommantSQL, connection);

//            command.ExecuteNonQuery();
//            using (OleDbDataReader reader = command.ExecuteReader())
//            {
//                reader.Read();
//                OptVal = reader["Opt_Val"].ToString();
//            }
//            connection.Close();
//            return OptVal;
//        }
        public string Get_SystemInformation_FieldValue(string FieldName)
        {
            string OptVal = "";
            string CommantSQL = $@"SELECT tbl_Info.{FieldName} FROM tbl_Info";
            connection.Open();
            command = new OleDbCommand(CommantSQL, connection);

            command.ExecuteNonQuery();
            using (OleDbDataReader reader = command.ExecuteReader())
            {
                reader.Read();
                OptVal = reader[FieldName].ToString();
            }
            connection.Close();
            return OptVal;
        } 
        public int Set_Option_FieldValue(string ParamName, string ParamValue)
        {
            int Result = -1;
            connection.Open();
            command = new OleDbCommand($"UPDATE tbl_Options SET tbl_Options.Opt_Val= '{ParamValue}' " +
                                        $"WHERE tbl_Options.Opt_Name = '{ParamName}'", connection);
            try
            {
                command.ExecuteNonQuery();
                Result = 1;
            }
            catch (Exception ex)
            {
                try
                {
                    command = new OleDbCommand($"UPDATE tbl_Options SET tbl_Options.Opt_Val= '{ParamValue}' " +
                                        $"WHERE tbl_Options.Opt_Name = {ParamName}", connection);
                    command.ExecuteNonQuery();
                    Result = 1;
                }
                catch (Exception exc) { }
            }
            connection.Close();
            return Result;
        }
        
        public int Set_Column_Option(Column_Class column_)
        {
            int Result = -1;
            connection.Open();
            string Caption, Name, FieldName;
            bool Vis, Sys, Col_Tree;
            int Col_Ind;
            Caption = column_.Caption;
            Name = column_.Name;
            FieldName = column_.FieldName;
            Vis = column_.Visible;
            Sys = column_.System;
            Col_Ind = column_.Index; 

            command = new OleDbCommand($"INSERT INTO tbl_Columns (Column_Caption, Column_Name, Column_FieldName, Column_Visible, Column_System, Column_Index)" +
                                                        $" VALUES ('{Caption}', '{Name}', '{FieldName}', {Vis}, {Sys}, {Col_Ind});", connection);
            try
            {
                command.ExecuteNonQuery();
                Result = 1;
            }
            catch(Exception exc1)
            {
                try
                {
                    command = new OleDbCommand($"INSERT INTO tbl_Columns (tbl_Columns.Column_Caption, tbl_Columns.Column_Name, tbl_Columns.Column_FieldName, tbl_Columns.Column_Visible, tbl_Columns.Column_System, tbl_Columns.Column_Index)" +
                                                $" VALUES ({Caption}, {Name}, {FieldName}, {Vis}, {Sys}, {Col_Ind})", connection);
                    command.ExecuteNonQuery();
                    Result = 1;
                }
                catch (Exception exc) { }
            }
            connection.Close();
            return Result;
        }
        public int SendQuery(string SQL_Query)
        {
            int Result = -1;
            connection.Open();
            command = new OleDbCommand(SQL_Query, connection);
            try
            {
                command.ExecuteNonQuery();
                Result = 1;
            }
            catch (Exception ex)
            {
                Result = -1;
            }
            connection.Close();
            return Result;
        }
        public int Delete_Data(Select_Option select_Option)
        {
            int Result = -1;
            connection.Open();
            string commandQuery = AddQueryFile(select_Option); 

            command = new OleDbCommand(commandQuery, connection); 
            command.ExecuteNonQuery();
            Result = 1; 
            connection.Close();
            return Result;
        }
        public bool TableExist(string tableName)
        {
            bool exists = false;
            connection.Open();
            try
            {
                DataTable dbTbl = connection.GetSchema("Tables", new string[] { null, null, tableName, null });
                if (dbTbl.Rows.Count != 0)
                    exists = true;
            }
            catch { }
            connection.Close();
            return exists;
        }
    }
}
