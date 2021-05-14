
namespace VSNRM_Kompas.ProjectClone
{
    partial class FindAndRepace_Form
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.cb_To4noe = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.tb_ReplceText = new DevExpress.XtraEditors.TextEdit();
            this.rb_ReplaceText = new System.Windows.Forms.RadioButton();
            this.rb_NotCheckElements = new System.Windows.Forms.RadioButton();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cb_NotCheckRegister = new DevExpress.XtraEditors.CheckEdit();
            this.rb_CheckElement = new System.Windows.Forms.RadioButton();
            this.tb_FindText = new DevExpress.XtraEditors.TextEdit();
            this.cb_FindParams = new DevExpress.XtraEditors.ComboBoxEdit();
            this.bt_all = new DevExpress.XtraEditors.SimpleButton();
            this.bt_Next = new DevExpress.XtraEditors.SimpleButton();
            this.bt_close = new DevExpress.XtraEditors.SimpleButton();
            this.cb_EditName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cb_To4noe.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_ReplceText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_NotCheckRegister.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_FindText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_FindParams.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_EditName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.cb_EditName);
            this.groupControl1.Controls.Add(this.cb_To4noe);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.tb_ReplceText);
            this.groupControl1.Controls.Add(this.rb_ReplaceText);
            this.groupControl1.Controls.Add(this.rb_NotCheckElements);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.cb_NotCheckRegister);
            this.groupControl1.Controls.Add(this.rb_CheckElement);
            this.groupControl1.Controls.Add(this.tb_FindText);
            this.groupControl1.Controls.Add(this.cb_FindParams);
            this.groupControl1.Location = new System.Drawing.Point(12, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(344, 187);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Критерий поиска";
            // 
            // cb_To4noe
            // 
            this.cb_To4noe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_To4noe.Location = new System.Drawing.Point(204, 160);
            this.cb_To4noe.Name = "cb_To4noe";
            this.cb_To4noe.Properties.Caption = "Точное соответствие";
            this.cb_To4noe.Size = new System.Drawing.Size(133, 20);
            this.cb_To4noe.TabIndex = 11;
            this.cb_To4noe.CheckedChanged += new System.EventHandler(this.cb_To4noe_CheckedChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelControl1.Location = new System.Drawing.Point(5, 163);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(61, 13);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Параметры:";
            // 
            // tb_ReplceText
            // 
            this.tb_ReplceText.Location = new System.Drawing.Point(158, 109);
            this.tb_ReplceText.Name = "tb_ReplceText";
            this.tb_ReplceText.Size = new System.Drawing.Size(179, 20);
            this.tb_ReplceText.TabIndex = 9;
            // 
            // rb_ReplaceText
            // 
            this.rb_ReplaceText.AutoSize = true;
            this.rb_ReplaceText.Location = new System.Drawing.Point(22, 110);
            this.rb_ReplaceText.Name = "rb_ReplaceText";
            this.rb_ReplaceText.Size = new System.Drawing.Size(130, 17);
            this.rb_ReplaceText.TabIndex = 8;
            this.rb_ReplaceText.TabStop = true;
            this.rb_ReplaceText.Text = "Заменить текст чем:";
            this.rb_ReplaceText.UseVisualStyleBackColor = true;
            this.rb_ReplaceText.CheckedChanged += new System.EventHandler(this.rb_ReplaceText_CheckedChanged);
            // 
            // rb_NotCheckElements
            // 
            this.rb_NotCheckElements.AutoSize = true;
            this.rb_NotCheckElements.Location = new System.Drawing.Point(22, 87);
            this.rb_NotCheckElements.Name = "rb_NotCheckElements";
            this.rb_NotCheckElements.Size = new System.Drawing.Size(149, 17);
            this.rb_NotCheckElements.TabIndex = 7;
            this.rb_NotCheckElements.TabStop = true;
            this.rb_NotCheckElements.Text = "Не отмечать элемент(ы)";
            this.rb_NotCheckElements.UseVisualStyleBackColor = true;
            this.rb_NotCheckElements.CheckedChanged += new System.EventHandler(this.rb_NotCheckElements_CheckedChanged);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(5, 66);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(11, 13);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "И:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(174, 29);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(10, 13);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "в:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(5, 29);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(34, 13);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "Поиск:";
            // 
            // cb_NotCheckRegister
            // 
            this.cb_NotCheckRegister.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_NotCheckRegister.Location = new System.Drawing.Point(72, 160);
            this.cb_NotCheckRegister.Name = "cb_NotCheckRegister";
            this.cb_NotCheckRegister.Properties.Caption = "Без учета регистра";
            this.cb_NotCheckRegister.Size = new System.Drawing.Size(126, 20);
            this.cb_NotCheckRegister.TabIndex = 3;
            this.cb_NotCheckRegister.CheckedChanged += new System.EventHandler(this.cb_NotCheckRegister_CheckedChanged);
            // 
            // rb_CheckElement
            // 
            this.rb_CheckElement.AutoSize = true;
            this.rb_CheckElement.Location = new System.Drawing.Point(22, 64);
            this.rb_CheckElement.Name = "rb_CheckElement";
            this.rb_CheckElement.Size = new System.Drawing.Size(135, 17);
            this.rb_CheckElement.TabIndex = 2;
            this.rb_CheckElement.TabStop = true;
            this.rb_CheckElement.Text = "Отметить элемент(ы)";
            this.rb_CheckElement.UseVisualStyleBackColor = true;
            this.rb_CheckElement.CheckedChanged += new System.EventHandler(this.rb_CheckElement_CheckedChanged);
            // 
            // tb_FindText
            // 
            this.tb_FindText.Location = new System.Drawing.Point(190, 26);
            this.tb_FindText.Name = "tb_FindText";
            this.tb_FindText.Size = new System.Drawing.Size(123, 20);
            this.tb_FindText.TabIndex = 1;
            this.tb_FindText.EditValueChanged += new System.EventHandler(this.tb_FindText_EditValueChanged);
            // 
            // cb_FindParams
            // 
            this.cb_FindParams.Location = new System.Drawing.Point(45, 26);
            this.cb_FindParams.Name = "cb_FindParams";
            this.cb_FindParams.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_FindParams.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cb_FindParams.Size = new System.Drawing.Size(123, 20);
            this.cb_FindParams.TabIndex = 0;
            // 
            // bt_all
            // 
            this.bt_all.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_all.Location = new System.Drawing.Point(93, 205);
            this.bt_all.Name = "bt_all";
            this.bt_all.Size = new System.Drawing.Size(88, 23);
            this.bt_all.TabIndex = 2;
            this.bt_all.Text = "Проверить все";
            this.bt_all.Click += new System.EventHandler(this.bt_all_Click);
            // 
            // bt_Next
            // 
            this.bt_Next.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_Next.Location = new System.Drawing.Point(12, 205);
            this.bt_Next.Name = "bt_Next";
            this.bt_Next.Size = new System.Drawing.Size(75, 23);
            this.bt_Next.TabIndex = 3;
            this.bt_Next.Text = "Проверить";
            this.bt_Next.Click += new System.EventHandler(this.bt_Next_Click);
            // 
            // bt_close
            // 
            this.bt_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_close.Location = new System.Drawing.Point(281, 205);
            this.bt_close.Name = "bt_close";
            this.bt_close.Size = new System.Drawing.Size(75, 23);
            this.bt_close.TabIndex = 4;
            this.bt_close.Text = "Закрыть";
            this.bt_close.Click += new System.EventHandler(this.bt_close_Click);
            // 
            // cb_EditName
            // 
            this.cb_EditName.Location = new System.Drawing.Point(158, 135);
            this.cb_EditName.Name = "cb_EditName";
            this.cb_EditName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_EditName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cb_EditName.Size = new System.Drawing.Size(179, 20);
            this.cb_EditName.TabIndex = 12;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(64, 138);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(88, 13);
            this.labelControl5.TabIndex = 13;
            this.labelControl5.Text = "Заменять в поле:";
            // 
            // FindAndRepace_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bt_close;
            this.ClientSize = new System.Drawing.Size(368, 240);
            this.Controls.Add(this.bt_close);
            this.Controls.Add(this.bt_Next);
            this.Controls.Add(this.bt_all);
            this.Controls.Add(this.groupControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximumSize = new System.Drawing.Size(370, 272);
            this.MinimumSize = new System.Drawing.Size(370, 272);
            this.Name = "FindAndRepace_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Найти / Заменить";
            this.Load += new System.EventHandler(this.FindAndRepace_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cb_To4noe.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_ReplceText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_NotCheckRegister.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_FindText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_FindParams.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_EditName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckEdit cb_NotCheckRegister;
        private System.Windows.Forms.RadioButton rb_CheckElement;
        private DevExpress.XtraEditors.TextEdit tb_FindText;
        private DevExpress.XtraEditors.ComboBoxEdit cb_FindParams;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit cb_To4noe;
        private DevExpress.XtraEditors.TextEdit tb_ReplceText;
        private System.Windows.Forms.RadioButton rb_ReplaceText;
        private System.Windows.Forms.RadioButton rb_NotCheckElements;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SimpleButton bt_all;
        private DevExpress.XtraEditors.SimpleButton bt_Next;
        private DevExpress.XtraEditors.SimpleButton bt_close;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.ComboBoxEdit cb_EditName;
    }
}