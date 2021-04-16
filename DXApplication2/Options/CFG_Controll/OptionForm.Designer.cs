
namespace VSNRM_Kompas.Options.CFG_Controll
{
    partial class OptionForm
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
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionForm));
            DevExpress.Utils.ToolTipSeparatorItem toolTipSeparatorItem1 = new DevExpress.Utils.ToolTipSeparatorItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem3 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipSeparatorItem toolTipSeparatorItem2 = new DevExpress.Utils.ToolTipSeparatorItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem4 = new DevExpress.Utils.ToolTipTitleItem();
            this.Cancel_Bt = new DevExpress.XtraEditors.SimpleButton();
            this.OK_Bt = new DevExpress.XtraEditors.SimpleButton();
            this.TP_MU = new DevExpress.XtraTab.XtraTabPage();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.cb_Count = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.cb_Area = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.cb_Volume = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.cb_Mass = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.cb_Length = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.TP_MU.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Count.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Area.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Volume.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Mass.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Length.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Cancel_Bt
            // 
            this.Cancel_Bt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_Bt.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Bt.Location = new System.Drawing.Point(427, 288);
            this.Cancel_Bt.Name = "Cancel_Bt";
            this.Cancel_Bt.Size = new System.Drawing.Size(75, 23);
            toolTipTitleItem1.Text = "Отмена";
            toolTipItem1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage")));
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "Отменить и выйти";
            toolTipTitleItem2.LeftIndent = 6;
            toolTipTitleItem2.Text = "DXF-Auto. Настройки";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            superToolTip1.Items.Add(toolTipSeparatorItem1);
            superToolTip1.Items.Add(toolTipTitleItem2);
            this.Cancel_Bt.SuperTip = superToolTip1;
            this.Cancel_Bt.TabIndex = 4;
            this.Cancel_Bt.Text = "Отмена";
            this.Cancel_Bt.Click += new System.EventHandler(this.Cancel_Bt_Click);
            // 
            // OK_Bt
            // 
            this.OK_Bt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK_Bt.Location = new System.Drawing.Point(319, 288);
            this.OK_Bt.Name = "OK_Bt";
            this.OK_Bt.Size = new System.Drawing.Size(75, 23);
            toolTipTitleItem3.Text = "Сохранить";
            toolTipItem2.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage1")));
            toolTipItem2.LeftIndent = 6;
            toolTipItem2.Text = "Сохранить текущую конфигурацию настроек";
            toolTipTitleItem4.LeftIndent = 6;
            toolTipTitleItem4.Text = "DXF-Auto. Настройки";
            superToolTip2.Items.Add(toolTipTitleItem3);
            superToolTip2.Items.Add(toolTipItem2);
            superToolTip2.Items.Add(toolTipSeparatorItem2);
            superToolTip2.Items.Add(toolTipTitleItem4);
            this.OK_Bt.SuperTip = superToolTip2;
            this.OK_Bt.TabIndex = 3;
            this.OK_Bt.Text = "Сохранить";
            this.OK_Bt.Click += new System.EventHandler(this.OK_Bt_Click);
            // 
            // TP_MU
            // 
            this.TP_MU.Controls.Add(this.groupControl1);
            this.TP_MU.Name = "TP_MU";
            this.TP_MU.Size = new System.Drawing.Size(488, 245);
            this.TP_MU.Text = "Единицы измерения";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.cb_Count);
            this.groupControl1.Controls.Add(this.labelControl6);
            this.groupControl1.Controls.Add(this.cb_Area);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.cb_Volume);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.cb_Mass);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl7);
            this.groupControl1.Controls.Add(this.cb_Length);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(488, 245);
            this.groupControl1.TabIndex = 18;
            this.groupControl1.Text = "ЕИ в МЦХ";
            // 
            // cb_Count
            // 
            this.cb_Count.Location = new System.Drawing.Point(164, 149);
            this.cb_Count.Name = "cb_Count";
            this.cb_Count.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_Count.Properties.Items.AddRange(new object[] {
            "Штуки"});
            this.cb_Count.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cb_Count.Size = new System.Drawing.Size(116, 20);
            this.cb_Count.TabIndex = 25;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(101, 152);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(34, 13);
            this.labelControl6.TabIndex = 24;
            this.labelControl6.Text = "Штуки";
            // 
            // cb_Area
            // 
            this.cb_Area.Location = new System.Drawing.Point(164, 97);
            this.cb_Area.Name = "cb_Area";
            this.cb_Area.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_Area.Properties.Items.AddRange(new object[] {
            "Сантиметры кв.",
            "Миллиметры кв.",
            "Дециметры кв.",
            "Метры кв.",
            "Настройки документа"});
            this.cb_Area.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cb_Area.Size = new System.Drawing.Size(116, 20);
            this.cb_Area.TabIndex = 23;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(88, 100);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(47, 13);
            this.labelControl5.TabIndex = 22;
            this.labelControl5.Text = "Площадь";
            // 
            // cb_Volume
            // 
            this.cb_Volume.Location = new System.Drawing.Point(164, 123);
            this.cb_Volume.Name = "cb_Volume";
            this.cb_Volume.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_Volume.Properties.Items.AddRange(new object[] {
            "Сантиметры куб.",
            "Миллиметры куб.",
            "Дециметры куб.",
            "Метры куб.",
            "Настройки документа"});
            this.cb_Volume.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cb_Volume.Size = new System.Drawing.Size(116, 20);
            this.cb_Volume.TabIndex = 21;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(102, 126);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(33, 13);
            this.labelControl4.TabIndex = 20;
            this.labelControl4.Text = "Объем";
            // 
            // cb_Mass
            // 
            this.cb_Mass.Location = new System.Drawing.Point(164, 45);
            this.cb_Mass.Name = "cb_Mass";
            this.cb_Mass.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_Mass.Properties.Items.AddRange(new object[] {
            "Граммы",
            "Килограммы",
            "Настройки документа"});
            this.cb_Mass.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cb_Mass.Size = new System.Drawing.Size(116, 20);
            this.cb_Mass.TabIndex = 19;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(105, 48);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(30, 13);
            this.labelControl3.TabIndex = 18;
            this.labelControl3.Text = "Масса";
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Location = new System.Drawing.Point(5, 26);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(130, 13);
            this.labelControl7.TabIndex = 15;
            this.labelControl7.Text = "Наименование единиц";
            // 
            // cb_Length
            // 
            this.cb_Length.Location = new System.Drawing.Point(164, 71);
            this.cb_Length.Name = "cb_Length";
            this.cb_Length.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_Length.Properties.Items.AddRange(new object[] {
            "Сантиметры",
            "Миллиметры",
            "Дециметры",
            "Метры",
            "Настройки документа"});
            this.cb_Length.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cb_Length.Size = new System.Drawing.Size(116, 20);
            this.cb_Length.TabIndex = 17;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(103, 74);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(32, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Длина";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(164, 26);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(116, 13);
            this.labelControl2.TabIndex = 16;
            this.labelControl2.Text = "Единицы измерения";
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xtraTabControl1.Location = new System.Drawing.Point(12, 12);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.TP_MU;
            this.xtraTabControl1.Size = new System.Drawing.Size(490, 270);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.TP_MU});
            // 
            // OptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel_Bt;
            this.ClientSize = new System.Drawing.Size(514, 323);
            this.Controls.Add(this.Cancel_Bt);
            this.Controls.Add(this.OK_Bt);
            this.Controls.Add(this.xtraTabControl1);
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("OptionForm.IconOptions.SvgImage")));
            this.MinimumSize = new System.Drawing.Size(516, 355);
            this.Name = "OptionForm";
            this.Text = "Конфигурации";
            this.Load += new System.EventHandler(this.OptionForm_Load);
            this.TP_MU.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Count.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Area.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Volume.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Mass.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Length.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton Cancel_Bt;
        private DevExpress.XtraEditors.SimpleButton OK_Bt;
        private DevExpress.XtraTab.XtraTabPage TP_MU;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cb_Count;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.ComboBoxEdit cb_Area;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.ComboBoxEdit cb_Volume;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit cb_Mass;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.ComboBoxEdit cb_Length;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
    }
}