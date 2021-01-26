using SaveDXF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VSNRM_Kompas.XMLContreller;

namespace VSNRM_Kompas.Options
{
    public class CFG_Class
    { 
        public enum SlideSizeEnum
        {
            //настройки по просьбе предприятий
            MiniSlide = 0,
            largeSlide = 1
        }
        public string KeyFile_Puth = "";
        XMLCLass controller;
        public GetOptionInformation OptionInformation;
        public Str_Variable_Class Skin_Name { get; set; } = new Str_Variable_Class("Skin_Name", "Basic");
        public Bool_Variable_Class Add_InVisiblePart { get; set; } = new Bool_Variable_Class("Add_InVisiblePart", true);
        public Bool_Variable_Class SlideSize_Large { get; set; } = new Bool_Variable_Class("SlideSize_Large", true); public void SaveOption()
        {
            List<Obj_Variable_Class> variable_s = new List<Obj_Variable_Class>();
            variable_s.Add(new Obj_Variable_Class(Skin_Name.Name, Skin_Name.Value));
            variable_s.Add(new Obj_Variable_Class(Add_InVisiblePart.Name, Add_InVisiblePart.Value));
            variable_s.Add(new Obj_Variable_Class(SlideSize_Large.Name, SlideSize_Large.Value));

            controller.IOptions.Save(variable_s);
        }

        public CFG_Class()
        {
            KeyFile_Puth = $@"C:\Users\Public\Documents\NSoft\{Application.ProductName}\CFG\keyfile.dat";
            controller = new XMLCLass();
            OptionInformation = controller.ISystemInformation.GetInfo();
            OptionInformation.GetData();
             
            List<Obj_Variable_Class> variable_s = controller.IOptions.GetOptions();
            foreach (Obj_Variable_Class variable in variable_s)
            {
                switch (variable.Name)
                {
                    case "Skin_Name":
                        Skin_Name.Value = (string)variable.Value;
                        break;
                    case "SlideSize_Large":
                        SlideSize_Large.Value = (bool)variable.Value;
                        break;
                }
            }
        }

    }
    public class OptionPath
    {
        public static string Columns_FileXML = $@"C:\Users\Public\Documents\NSoft\{Application.ProductName}\CFG\Columns.XML";
        public static string Options_FileXML = $@"C:\Users\Public\Documents\NSoft\{Application.ProductName}\CFG\Options.XML";
        public static string SystemInformation_ForUser_FileXML = $@"C:\Users\Public\Documents\NSoft\{Application.ProductName}\CFG\SystemInformation.XML";
        public static string SystemInformation_FileXML = $@"{Application.StartupPath}\CFG\SystemInformation.XML";

        public static string Sayings_FileINI = $@"C:\Users\Public\Documents\NSoft\{Application.ProductName}\CFG\Sayings.INI";
    }
    public class GetOptionInformation
    {
        public const string Inf_BD_Version = "Inf_BD_Version";
        public const string Inf_Cad_Name = "Inf_Cad_Name";
        public const string Inf_Email = "Inf_Email";
        public const string Inf_Phone = "Inf_Phone";
        public string BD_Version { get; set; }
        public string Phone { get; set; }
        public string Cad_Name { get; set; }
        public string Cad_Version_Recomend { get; set; }
        public string Cad_VersionInPC { get; set; }
        public string Email { get; set; }
        public string Bild_Create_Date { get; set; }
        public GetOptionInformation()
        {
            Bild_Create_Date = System.IO.File.GetCreationTime(System.Windows.Forms.Application.ExecutablePath).ToString();
            Phone = "+7(931)007 93 16";
            BD_Version = "112.0.9.3";
            Cad_Name = "Компас 3D";
            Email = "DXFAutoHelp@gmail.com";

            Cad_Version_Recomend = Body.KompasVersionFlag;
            Cad_VersionInPC = Body.KompasVersion;
        }
        public void GetData()
        {
            Cad_Version_Recomend = Body.KompasVersionFlag;
            Cad_VersionInPC = Body.KompasVersion;
            Bild_Create_Date = System.IO.File.GetCreationTime(System.Windows.Forms.Application.ExecutablePath).ToString();
        }
    }
}
