using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace VSNRM_Kompas.PromoCods
{
    public partial class PromoForm : DevExpress.XtraEditors.XtraForm
    {
        public IPromo_Class promo_Class = new IPromo_Class();
        bool promoApplied;

        public PromoForm()
        {
            InitializeComponent();
        }

        private void PromoForm_Load(object sender, EventArgs e)
        {
            lb_Active.Text = promo_Class.ActiveFeaturesText();
        }

        private void bt_CreateRequest_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.FileName = promo_Class.Client.RequestFileName();
                dialog.Filter = "Файл-запрос (*.datq)|*.datq";
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    promo_Class.CreateRequestFile(dialog.FileName);
                    MessageBox.Show("Файл-запрос создан. Отправьте его разработчику.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось создать файл-запрос:\n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bt_ImportPromo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Файл промокода (*.dat)|*.dat";
                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                if (promo_Class.ImportPromoFile(dialog.FileName, out string message))
                {
                    promoApplied = true;
                    lb_Active.Text = promo_Class.ActiveFeaturesText();
                    MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show(message ?? "Файл промокода не подходит.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bt_close_Click(object sender, EventArgs e)
        {
            //OK - чтобы главное окно обновило панели по новым промокодам
            this.DialogResult = promoApplied ? DialogResult.OK : DialogResult.Cancel;
            this.Close();
        }
    }
}
