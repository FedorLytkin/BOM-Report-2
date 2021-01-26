
using SaveDXF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSNRM_Kompas.Controllers;

namespace VSNRM_Kompas.Options
{
    public class OptionClass
    {
        public Query controller;

        public GetOptionInformation OptionInformation;

        public string KeyFile_Puth = "";
         
        public string Skin_Name { get; set; }

        public enum OptionsListEnum
        {
            Skin_Name = 10
        } 
        public enum Select_Folder_Enum
        {
            SelectFolder = 0,
            AssemblyDirectory = 1,
            PartDirectory = 2,
            AddNewFolder = 3
        }
        public OptionClass()
        {
            KeyFile_Puth =  $@"C:\Users\Public\Documents\NSoft\{System.Windows.Forms.Application.ProductName}\CFG\keyfile.dat";
            controller = new Query(ConnectionString.ConnStr);
            DataTable dataTable = controller.Get_Data(Query.Select_Option.Options);
            foreach(DataRow row in dataTable.Rows)
            {
                switch (row["Opt_Name"])
                { 
                    case "Skin_Name":
                        Skin_Name = row["Opt_Val"].ToString();
                        break;
                }
            }
            OptionInformation = new GetOptionInformation(controller);
        }
        public void UpdateBD()
        { 
            SetUpdateOptionBD updateOptionBD = new SetUpdateOptionBD(controller);
            OptionInformation = new GetOptionInformation(controller);
        }
        public void SaveColumnCFG(List<Column_Class> Columns_Option_)
        {
            controller.Delete_Data(Query.Select_Option.DeleteColumns);
            foreach (Column_Class column_ in Columns_Option_)
            {
                controller.Set_Column_Option(column_);
            }
        }
        public void SaveParameters()
        {
            controller.Set_Option_FieldValue("Skin_Name", this.Skin_Name);
        }
        public static String Get_Opt_Name_By_OptionsList(OptionsListEnum OptionsEnum)
        {
            string Opt_Name = "";
            switch (OptionsEnum)
            { 
                case OptionsListEnum.Skin_Name:
                    Opt_Name = "Skin_Name";
                    break;
            }
            return Opt_Name;
        }
        public class GetOptionInformation
        {
            public const string Inf_BD_Version = "Inf_BD_Version";
            public const  string Inf_Cad_Name = "Inf_Cad_Name";
            public const string Inf_Email = "Inf_Email";
            public const string Inf_Phone = "Inf_Phone";
            public string BD_Version { get; }
            public string Phone { get; }
            public string Cad_Name { get;}
            public string Cad_Version_Recomend { get; }
            public string Cad_VersionInPC { get; }
            public string Email { get; }
            public string Bild_Create_Date { get;}
            public GetOptionInformation(Query controller)
            {
                Bild_Create_Date = System.IO.File.GetCreationTime(System.Windows.Forms.Application.ExecutablePath).ToString();
                BD_Version = controller.Get_SystemInformation_FieldValue(Inf_BD_Version);
                Phone = controller.Get_SystemInformation_FieldValue(Inf_Phone);
                Cad_Name = controller.Get_SystemInformation_FieldValue(Inf_Cad_Name);
                Email = controller.Get_SystemInformation_FieldValue(Inf_Email);
                Cad_Version_Recomend = Body.KompasVersionFlag;
                Cad_VersionInPC = Body.KompasVersion;
            }

        }
    }
}
