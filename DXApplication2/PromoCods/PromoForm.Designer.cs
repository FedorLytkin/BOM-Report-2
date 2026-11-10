
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
            this.lb_Step1 = new DevExpress.XtraEditors.LabelControl();
            this.bt_CreateRequest = new DevExpress.XtraEditors.SimpleButton();
            this.lb_Step2 = new DevExpress.XtraEditors.LabelControl();
            this.bt_ImportPromo = new DevExpress.XtraEditors.SimpleButton();
            this.lb_Active = new DevExpress.XtraEditors.LabelControl();
            this.bt_close = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            //
            // lb_Step1
            //
            this.lb_Step1.Location = new System.Drawing.Point(12, 12);
            this.lb_Step1.Name = "lb_Step1";
            this.lb_Step1.Size = new System.Drawing.Size(277, 13);
            this.lb_Step1.TabIndex = 0;
            this.lb_Step1.Text = "1. Создайте файл-запрос и отправьте его разработчику";
            //
            // bt_CreateRequest
            //
            this.bt_CreateRequest.Location = new System.Drawing.Point(12, 31);
            this.bt_CreateRequest.Name = "bt_CreateRequest";
            this.bt_CreateRequest.Size = new System.Drawing.Size(200, 23);
            this.bt_CreateRequest.TabIndex = 1;
            this.bt_CreateRequest.Text = "Создать файл-запрос...";
            this.bt_CreateRequest.Click += new System.EventHandler(this.bt_CreateRequest_Click);
            //
            // lb_Step2
            //
            this.lb_Step2.Location = new System.Drawing.Point(12, 68);
            this.lb_Step2.Name = "lb_Step2";
            this.lb_Step2.Size = new System.Drawing.Size(244, 13);
            this.lb_Step2.TabIndex = 2;
            this.lb_Step2.Text = "2. Загрузите полученный файл промокода (.dat)";
            //
            // bt_ImportPromo
            //
            this.bt_ImportPromo.Location = new System.Drawing.Point(12, 87);
            this.bt_ImportPromo.Name = "bt_ImportPromo";
            this.bt_ImportPromo.Size = new System.Drawing.Size(200, 23);
            this.bt_ImportPromo.TabIndex = 3;
            this.bt_ImportPromo.Text = "Загрузить промокод...";
            this.bt_ImportPromo.Click += new System.EventHandler(this.bt_ImportPromo_Click);
            //
            // lb_Active
            //
            this.lb_Active.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.lb_Active.Location = new System.Drawing.Point(12, 126);
            this.lb_Active.Name = "lb_Active";
            this.lb_Active.Size = new System.Drawing.Size(441, 13);
            this.lb_Active.TabIndex = 4;
            this.lb_Active.Text = "Промокоды не активированы.";
            //
            // bt_close
            //
            this.bt_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_close.Location = new System.Drawing.Point(378, 158);
            this.bt_close.Name = "bt_close";
            this.bt_close.Size = new System.Drawing.Size(75, 23);
            this.bt_close.TabIndex = 5;
            this.bt_close.Text = "Закрыть";
            this.bt_close.Click += new System.EventHandler(this.bt_close_Click);
            //
            // PromoForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bt_close;
            this.ClientSize = new System.Drawing.Size(465, 193);
            this.Controls.Add(this.bt_close);
            this.Controls.Add(this.lb_Active);
            this.Controls.Add(this.bt_ImportPromo);
            this.Controls.Add(this.lb_Step2);
            this.Controls.Add(this.bt_CreateRequest);
            this.Controls.Add(this.lb_Step1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("PromoForm.IconOptions.SvgImage")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PromoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Промокоды";
            this.Load += new System.EventHandler(this.PromoForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lb_Step1;
        private DevExpress.XtraEditors.SimpleButton bt_CreateRequest;
        private DevExpress.XtraEditors.LabelControl lb_Step2;
        private DevExpress.XtraEditors.SimpleButton bt_ImportPromo;
        private DevExpress.XtraEditors.LabelControl lb_Active;
        private DevExpress.XtraEditors.SimpleButton bt_close;
    }
}
