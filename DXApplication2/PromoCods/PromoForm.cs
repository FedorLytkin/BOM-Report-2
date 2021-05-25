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

namespace VSNRM_Kompas.PromoCods
{
    public partial class PromoForm : DevExpress.XtraEditors.XtraForm
    {
        public IPromo_Class promo_Class = new IPromo_Class();
        public PromoForm()
        {
            InitializeComponent();
        }

        private void bt_ok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_PromoIn.Text)) return;
            
            if (promo_Class.checkPromo(tb_PromoIn.Text))
                MessageBox.Show($"Промокод {tb_PromoIn.Text.ToUpper()} успешно применен!\nЧтобы изменения вступили в силу, перезапустите приложение {Application.ProductName}", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show($"Промокод {tb_PromoIn.Text.ToUpper()} введен не верно, либо не существует!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}