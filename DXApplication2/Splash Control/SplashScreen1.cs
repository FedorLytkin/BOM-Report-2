//using BDConnetction.Controllers;
using DevExpress.XtraSplashScreen;
//using DXFAuto2.BDConnetction.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VSNRM_Kompas.Controllers;
using VSNRM_Kompas.Options;

namespace VSNRM_Kompas
{
    public partial class SplashScreen1 : SplashScreen
    {
        Query controller;
        public SplashScreen1()
        {
            InitializeComponent();
            controller = new Query(ConnectionString.ConnStr);
            this.lb_Pogovorki.Text = GetRandomPogovorka();
            this.labelCopyright.Text = $"{Application.ProductName} v{Application.ProductVersion}\nCopyright © 2020-" + DateTime.Now.Year.ToString();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(controller.Get_Option_FieldValue(OptionClass.OptionsListEnum.Skin_Name));
        }
        private string GetRandomPogovorka()
        {
            string Pogovorka = "";
            DataTable table = controller.Get_Data(Query.Select_Option.Pogovorki);
            int row_ind = 0;
            Random rnd = new Random();
            row_ind = rnd.Next(table.Rows.Count);
            Pogovorka = table.Rows[row_ind][1].ToString();
            return Pogovorka;
        }
        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum SplashScreenCommand
        {
        }
    }
}