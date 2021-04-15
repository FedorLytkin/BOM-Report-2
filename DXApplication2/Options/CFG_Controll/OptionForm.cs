using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VSNRM_Kompas.Options.CFG_Controll
{
    public partial class OptionForm : DevExpress.XtraEditors.XtraForm
    {
        public Option_Class IOption_Class;
        public OptionForm()
        {
            InitializeComponent();
            AddControls();
        }
        private void OptionForm_Load(object sender, EventArgs e)
        {

        }
        void AddControls()
        {
            cb_Length.Text = IOption_Class.Length_MU_Name;
            cb_Mass.Text = IOption_Class.Mass_MU_Name;
            cb_Volume.Text = IOption_Class.Volume_MU_Name;
            cb_Area.Text = IOption_Class.Area_MU_Name;
        }

        private void Cancel_Bt_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}