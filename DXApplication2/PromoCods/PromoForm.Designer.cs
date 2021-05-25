
namespace VSNRM_Kompas.PromoCods
{
    partial class PromoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PromoForm));
            this.tb_PromoIn = new DevExpress.XtraEditors.TextEdit();
            this.bt_ok = new DevExpress.XtraEditors.SimpleButton();
            this.bt_close = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.tb_PromoIn.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tb_PromoIn
            // 
            this.tb_PromoIn.Location = new System.Drawing.Point(12, 31);
            this.tb_PromoIn.Name = "tb_PromoIn";
            this.tb_PromoIn.Size = new System.Drawing.Size(224, 20);
            this.tb_PromoIn.TabIndex = 0;
            // 
            // bt_ok
            // 
            this.bt_ok.Location = new System.Drawing.Point(12, 85);
            this.bt_ok.Name = "bt_ok";
            this.bt_ok.Size = new System.Drawing.Size(115, 23);
            this.bt_ok.TabIndex = 1;
            this.bt_ok.Text = "ОК";
            this.bt_ok.Click += new System.EventHandler(this.bt_ok_Click);
            // 
            // bt_close
            // 
            this.bt_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_close.Location = new System.Drawing.Point(161, 85);
            this.bt_close.Name = "bt_close";
            this.bt_close.Size = new System.Drawing.Size(75, 23);
            this.bt_close.TabIndex = 2;
            this.bt_close.Text = "Отмена";
            this.bt_close.Click += new System.EventHandler(this.bt_close_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(95, 13);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "Введите промокод";
            // 
            // PromoForm
            // 
            this.AcceptButton = this.bt_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bt_close;
            this.ClientSize = new System.Drawing.Size(248, 120);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.bt_close);
            this.Controls.Add(this.bt_ok);
            this.Controls.Add(this.tb_PromoIn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("PromoForm.IconOptions.SvgImage")));
            this.Name = "PromoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Промо";
            ((System.ComponentModel.ISupportInitialize)(this.tb_PromoIn.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit tb_PromoIn;
        private DevExpress.XtraEditors.SimpleButton bt_ok;
        private DevExpress.XtraEditors.SimpleButton bt_close;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}