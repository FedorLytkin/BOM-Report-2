
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VSNRM_Kompas.Options;

namespace VSNRM_Kompas.Controllers
{
    class SetUpdateOptionBD
    {
        Query controller;
        CFG_Class optionClass;
        public const string BDVersion = "18.0.9.2";
        enum BDVersionCompare
        {
            Menshe_CompareEnum= -1,
            Ravno_CompareEnum = 0,
            Bolshe_CompareEnum = 1
        }
        public SetUpdateOptionBD(Query Qcontroller)
        {
            controller = Qcontroller;
            optionClass = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).Main_Options;
            switch (BDVersion)
            {
                case "18.0.9.0":
                    UpdateBD_112_0_9_0();
                    break;
                case "18.0.9.1":
                    UpdateBD_112_0_9_1();
                    break;
                case "18.0.9.2":
                    UpdateBD_112_0_9_2();
                    break;
            }
        }

        private int GetVerdion(string Ver)
        {
            int res = 0;
            string[] arr = Ver.Split('.');
            res = Convert.ToInt32(arr[arr.Length - 1]);

            return res;
        }
        private BDVersionCompare GetVerdionCompare()
        {
            BDVersionCompare res = BDVersionCompare.Menshe_CompareEnum;

            string BD_Version_In_Option = optionClass.OptionInformation.BD_Version;
            string BD_Version_AppRecomend = BDVersion;
            int verInAppRecomend = GetVerdion(BD_Version_AppRecomend);
            int verInOption = GetVerdion(BD_Version_In_Option);

            switch (verInOption.CompareTo((Object)verInAppRecomend))
            {
                case -1:
                    return BDVersionCompare.Menshe_CompareEnum;
                case 0:
                    return BDVersionCompare.Ravno_CompareEnum;
                case 1:
                    return BDVersionCompare.Bolshe_CompareEnum;
            }
            return res;
        }

        private void UpdateBD_112_0_9_0()
        {
            if (!controller.TableExist("tbl_Info"))
            {
                controller.SendQuery("CREATE TABLE tbl_Info (Inf_ID INTEGER, Inf_BD_Version TEXT(50), Inf_Program_Name TEXT(50), Inf_Programm_Version TEXT(50), Inf_Cad_Name TEXT(50), Inf_Email TEXT(50), Inf_Phone TEXT(50))");
                controller.SendQuery("INSERT INTO tbl_Info (Inf_BD_Version, Inf_Program_Name, Inf_Programm_Version, Inf_Cad_Name, Inf_Email, Inf_Phone) " +
                                                $"VALUES ('18.0.9.0', '{Application.ProductName}', '{Application.ProductVersion}', 'Компас 3D', 'DXFAutoHelp@gmail.com', '+7(931)007 93 16')");
            }
        }
        private void UpdateBD_112_0_9_1()
        {
            //предыдущие обновления
            UpdateBD_112_0_9_0();
            //проверить версию БД
            string BD_Version = optionClass.OptionInformation.BD_Version;
            if(GetVerdionCompare() == BDVersionCompare.Menshe_CompareEnum)
            {
                //исправить Площадь на НЕ системное
                controller.SendQuery($"UPDATE tbl_Columns SET Column_System = {false} WHERE Column_Caption = 'Площадь'");
                //добавить поговорку
                controller.SendQuery("INSERT INTO tbl_Sayings (Say_Text) " +
                                                    $"VALUES ('Даже самую простую задачу можно сделать невыполнимой, если провести достаточное количество совещаний')");
                //исправить номер версии БД
                controller.SendQuery("UPDATE tbl_Info SET Inf_BD_Version = '18.0.9.1'");
            }
        }
        private void UpdateBD_112_0_9_2()
        {
            //предыдущие обновления
            UpdateBD_112_0_9_0();
            UpdateBD_112_0_9_1();
            //проверить версию БД
            string BD_Version = optionClass.OptionInformation.BD_Version;
            if (GetVerdionCompare() == BDVersionCompare.Menshe_CompareEnum)
            {
                //добавить столбец Миниатюра
                controller.SendQuery("INSERT INTO tbl_Columns (Column_Caption, Column_Name, Column_FieldName, Column_Visible, Column_System, Column_Index) " +
                                                    $"VALUES ('Миниатюра', 'Миниатюра', 'Миниатюра', {false}, {true}, -1)");
                //исправить номер версии БД
                controller.SendQuery("UPDATE tbl_Info SET Inf_BD_Version = '18.0.9.2'");
            }

        }
    }
}
