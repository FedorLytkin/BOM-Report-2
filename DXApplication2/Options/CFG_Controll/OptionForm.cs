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
        public Option_Class IOption_Class { get; set; }
        public OptionForm()
        {
            InitializeComponent();
        }
        private void OptionForm_Load(object sender, EventArgs e)
        {
            AddControls();

        }
        void AddControls()
        {
            cb_Length.Text = IOption_Class.Length_MU_Name;
            cb_Mass.Text = IOption_Class.Mass_MU_Name;
            cb_Volume.Text = IOption_Class.Volume_MU_Name;
            cb_Area.Text = IOption_Class.Area_MU_Name;
            cb_Count.Text = IOption_Class.Count_MU_Name;
        }

        private void Cancel_Bt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OK_Bt_Click(object sender, EventArgs e)
        {
            IOption_Class.Set_MU_Mass(cb_Mass.Text);
            IOption_Class.Set_MU_Area(cb_Area.Text);
            IOption_Class.Set_MU_Length(cb_Length.Text);
            IOption_Class.Set_MU_Volume(cb_Volume.Text);
            IOption_Class.Set_MU_Count(cb_Count.Text);

            XMLContreller.XMLCLass xMLCLass = new XMLContreller.XMLCLass();
            xMLCLass.IOptions.SaveOptions(IOption_Class, false);
            this.Close();
        }
    }
}