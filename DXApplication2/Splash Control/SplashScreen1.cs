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
        XMLContreller.XMLCLass controller;
        public SplashScreen1()
        {
            InitializeComponent();
            controller = new XMLContreller.XMLCLass();
            this.memoEdit_Pogovorki.Text = GetRandomPogovorka();
            this.labelCopyright.Text = $"{Application.ProductName} v{Application.ProductVersion}\nCopyright © 2020-" + DateTime.Now.Year.ToString();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(controller.IOptions.GetFieldValue("Skin_Name"));
        }
        private string GetRandomPogovorka()
        {
            string Pogovorka = "";
            List<string> table = controller.IList.GetList(OptionPath.Sayings_FileINI);
            int row_ind = 0;
            Random rnd = new Random();
            row_ind = rnd.Next(table.Count);
            Pogovorka = table[row_ind].ToString();
            return Pogovorka;
        }
        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

    }
}