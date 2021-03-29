
namespace VSNRM_Kompas.ProjectClone
{
    partial class Proj_Clone
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Proj_Clone));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.rb_GridView = new System.Windows.Forms.RadioButton();
            this.rb_TreeList = new System.Windows.Forms.RadioButton();
            this.cb_check_SP = new DevExpress.XtraEditors.CheckEdit();
            this.cb_check_Drw = new DevExpress.XtraEditors.CheckEdit();
            this.rb_SaveInZIPFile = new System.Windows.Forms.RadioButton();
            this.rb_SaveInFolder = new System.Windows.Forms.RadioButton();
            this.tb_FolderPath = new DevExpress.XtraEditors.TextEdit();
            this.lb_Sborka = new DevExpress.XtraEditors.LabelControl();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            this.pictureEdit3 = new DevExpress.XtraEditors.PictureEdit();
            this.pictureEdit4 = new DevExpress.XtraEditors.PictureEdit();
            this.lb_Part = new DevExpress.XtraEditors.LabelControl();
            this.lb_Drws = new DevExpress.XtraEditors.LabelControl();
            this.lb_SP = new DevExpress.XtraEditors.LabelControl();
            this.bt_FinndAndReplase = new DevExpress.XtraEditors.SimpleButton();
            this.bt_FolderObzor = new DevExpress.XtraEditors.SimpleButton();
            this.bt_ZipFileObzor = new DevExpress.XtraEditors.SimpleButton();
            this.tb_ZipFileName = new DevExpress.XtraEditors.TextEdit();
            this.cb_AddPrefix = new DevExpress.XtraEditors.CheckEdit();
            this.cb_AddSufix = new DevExpress.XtraEditors.CheckEdit();
            this.cb_SaveInOneFolder = new DevExpress.XtraEditors.CheckEdit();
            this.tb_Prefix = new DevExpress.XtraEditors.TextEdit();
            this.tb_Sufix = new DevExpress.XtraEditors.TextEdit();
            this.bt_Save = new DevExpress.XtraEditors.SimpleButton();
            this.bt_Cancel = new DevExpress.XtraEditors.SimpleButton();
            this.bt_Help = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_check_SP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_check_Drw.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_FolderPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_ZipFileName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_AddPrefix.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_AddSufix.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_SaveInOneFolder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_Prefix.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_Sufix.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl1.Controls.Add(this.treeList1);
            this.groupControl1.Controls.Add(this.rb_GridView);
            this.groupControl1.Controls.Add(this.rb_TreeList);
            this.groupControl1.Controls.Add(this.cb_check_SP);
            this.groupControl1.Controls.Add(this.cb_check_Drw);
            this.groupControl1.Location = new System.Drawing.Point(12, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(653, 305);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Выберите файлы для сохранения в указанной папке Копирования проекта";
            // 
            // treeList1
            // 
            this.treeList1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeList1.Location = new System.Drawing.Point(5, 78);
            this.treeList1.Name = "treeList1";
            this.treeList1.OptionsBehavior.Editable = false;
            this.treeList1.OptionsBehavior.ReadOnly = true;
            this.treeList1.OptionsCustomization.AllowBandMoving = false;
            this.treeList1.OptionsCustomization.AllowBandResizing = false;
            this.treeList1.OptionsCustomization.AllowColumnMoving = false;
            this.treeList1.OptionsCustomization.AllowQuickHideColumns = false;
            this.treeList1.Size = new System.Drawing.Size(643, 222);
            this.treeList1.TabIndex = 4;
            this.treeList1.NodeCellStyle += new DevExpress.XtraTreeList.GetCustomNodeCellStyleEventHandler(this.treeList1_NodeCellStyle);
            this.treeList1.CustomDrawColumnHeader += new DevExpress.XtraTreeList.CustomDrawColumnHeaderEventHandler(this.treeList1_CustomDrawColumnHeader);
            this.treeList1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeList1_MouseUp);
            // 
            // rb_GridView
            // 
            this.rb_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rb_GridView.AutoSize = true;
            this.rb_GridView.Location = new System.Drawing.Point(543, 53);
            this.rb_GridView.Name = "rb_GridView";
            this.rb_GridView.Size = new System.Drawing.Size(89, 17);
            this.rb_GridView.TabIndex = 3;
            this.rb_GridView.TabStop = true;
            this.rb_GridView.Text = "Плоский вид";
            this.rb_GridView.UseVisualStyleBackColor = true;
            // 
            // rb_TreeList
            // 
            this.rb_TreeList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rb_TreeList.AutoSize = true;
            this.rb_TreeList.Location = new System.Drawing.Point(543, 27);
            this.rb_TreeList.Name = "rb_TreeList";
            this.rb_TreeList.Size = new System.Drawing.Size(105, 17);
            this.rb_TreeList.TabIndex = 2;
            this.rb_TreeList.TabStop = true;
            this.rb_TreeList.Text = "Вложенный вид";
            this.rb_TreeList.UseVisualStyleBackColor = true;
            this.rb_TreeList.CheckedChanged += new System.EventHandler(this.rb_TreeList_CheckedChanged);
            // 
            // cb_check_SP
            // 
            this.cb_check_SP.Location = new System.Drawing.Point(5, 52);
            this.cb_check_SP.Name = "cb_check_SP";
            this.cb_check_SP.Properties.Caption = "Включить Спецификации";
            this.cb_check_SP.Size = new System.Drawing.Size(152, 20);
            this.cb_check_SP.TabIndex = 1;
            this.cb_check_SP.CheckedChanged += new System.EventHandler(this.cb_check_SP_CheckedChanged);
            // 
            // cb_check_Drw
            // 
            this.cb_check_Drw.Location = new System.Drawing.Point(5, 26);
            this.cb_check_Drw.Name = "cb_check_Drw";
            this.cb_check_Drw.Properties.Caption = "Включить чертежи";
            this.cb_check_Drw.Size = new System.Drawing.Size(152, 20);
            this.cb_check_Drw.TabIndex = 0;
            this.cb_check_Drw.CheckedChanged += new System.EventHandler(this.cb_check_Drw_CheckedChanged);
            // 
            // rb_SaveInZIPFile
            // 
            this.rb_SaveInZIPFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rb_SaveInZIPFile.AutoSize = true;
            this.rb_SaveInZIPFile.Location = new System.Drawing.Point(17, 422);
            this.rb_SaveInZIPFile.Name = "rb_SaveInZIPFile";
            this.rb_SaveInZIPFile.Size = new System.Drawing.Size(145, 17);
            this.rb_SaveInZIPFile.TabIndex = 5;
            this.rb_SaveInZIPFile.TabStop = true;
            this.rb_SaveInZIPFile.Text = "Сохранить в файле Zip:";
            this.rb_SaveInZIPFile.UseVisualStyleBackColor = true;
            this.rb_SaveInZIPFile.CheckedChanged += new System.EventHandler(this.rb_SaveInZIPFile_CheckedChanged);
            // 
            // rb_SaveInFolder
            // 
            this.rb_SaveInFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rb_SaveInFolder.AutoSize = true;
            this.rb_SaveInFolder.Location = new System.Drawing.Point(17, 392);
            this.rb_SaveInFolder.Name = "rb_SaveInFolder";
            this.rb_SaveInFolder.Size = new System.Drawing.Size(126, 17);
            this.rb_SaveInFolder.TabIndex = 4;
            this.rb_SaveInFolder.TabStop = true;
            this.rb_SaveInFolder.Text = "Сохранить в папке:";
            this.rb_SaveInFolder.UseVisualStyleBackColor = true;
            this.rb_SaveInFolder.CheckedChanged += new System.EventHandler(this.rb_SaveInFolder_CheckedChanged);
            // 
            // tb_FolderPath
            // 
            this.tb_FolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tb_FolderPath.Location = new System.Drawing.Point(168, 392);
            this.tb_FolderPath.Name = "tb_FolderPath";
            this.tb_FolderPath.Size = new System.Drawing.Size(215, 20);
            this.tb_FolderPath.TabIndex = 6;
            // 
            // lb_Sborka
            // 
            this.lb_Sborka.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lb_Sborka.Location = new System.Drawing.Point(43, 325);
            this.lb_Sborka.Name = "lb_Sborka";
            this.lb_Sborka.Size = new System.Drawing.Size(50, 13);
            this.lb_Sborka.TabIndex = 7;
            this.lb_Sborka.Text = "Сборки: 0";
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureEdit1.EditValue = ((object)(resources.GetObject("pictureEdit1.EditValue")));
            this.pictureEdit1.Location = new System.Drawing.Point(17, 318);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Size = new System.Drawing.Size(20, 20);
            this.pictureEdit1.TabIndex = 9;
            // 
            // pictureEdit2
            // 
            this.pictureEdit2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureEdit2.EditValue = ((object)(resources.GetObject("pictureEdit2.EditValue")));
            this.pictureEdit2.Location = new System.Drawing.Point(128, 318);
            this.pictureEdit2.Name = "pictureEdit2";
            this.pictureEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit2.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit2.Size = new System.Drawing.Size(20, 20);
            this.pictureEdit2.TabIndex = 10;
            // 
            // pictureEdit3
            // 
            this.pictureEdit3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureEdit3.EditValue = ((object)(resources.GetObject("pictureEdit3.EditValue")));
            this.pictureEdit3.Location = new System.Drawing.Point(238, 318);
            this.pictureEdit3.Name = "pictureEdit3";
            this.pictureEdit3.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit3.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit3.Size = new System.Drawing.Size(20, 20);
            this.pictureEdit3.TabIndex = 11;
            // 
            // pictureEdit4
            // 
            this.pictureEdit4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureEdit4.EditValue = ((object)(resources.GetObject("pictureEdit4.EditValue")));
            this.pictureEdit4.Location = new System.Drawing.Point(349, 318);
            this.pictureEdit4.Name = "pictureEdit4";
            this.pictureEdit4.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit4.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit4.Size = new System.Drawing.Size(20, 20);
            this.pictureEdit4.TabIndex = 12;
            // 
            // lb_Part
            // 
            this.lb_Part.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lb_Part.Location = new System.Drawing.Point(154, 325);
            this.lb_Part.Name = "lb_Part";
            this.lb_Part.Size = new System.Drawing.Size(51, 13);
            this.lb_Part.TabIndex = 13;
            this.lb_Part.Text = "Детали: 0";
            // 
            // lb_Drws
            // 
            this.lb_Drws.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lb_Drws.Location = new System.Drawing.Point(264, 325);
            this.lb_Drws.Name = "lb_Drws";
            this.lb_Drws.Size = new System.Drawing.Size(58, 13);
            this.lb_Drws.TabIndex = 14;
            this.lb_Drws.Text = "Чертежи: 0";
            // 
            // lb_SP
            // 
            this.lb_SP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lb_SP.Location = new System.Drawing.Point(375, 325);
            this.lb_SP.Name = "lb_SP";
            this.lb_SP.Size = new System.Drawing.Size(88, 13);
            this.lb_SP.TabIndex = 15;
            this.lb_SP.Text = "Спецификации: 0";
            // 
            // bt_FinndAndReplase
            // 
            this.bt_FinndAndReplase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_FinndAndReplase.Location = new System.Drawing.Point(550, 320);
            this.bt_FinndAndReplase.Name = "bt_FinndAndReplase";
            this.bt_FinndAndReplase.Size = new System.Drawing.Size(110, 23);
            this.bt_FinndAndReplase.TabIndex = 16;
            this.bt_FinndAndReplase.Text = "Найти / Заменить";
            this.bt_FinndAndReplase.Click += new System.EventHandler(this.bt_FinndAndReplase_Click);
            // 
            // bt_FolderObzor
            // 
            this.bt_FolderObzor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_FolderObzor.Location = new System.Drawing.Point(389, 390);
            this.bt_FolderObzor.Name = "bt_FolderObzor";
            this.bt_FolderObzor.Size = new System.Drawing.Size(48, 23);
            this.bt_FolderObzor.TabIndex = 17;
            this.bt_FolderObzor.Text = "Обзор";
            this.bt_FolderObzor.Click += new System.EventHandler(this.bt_FolderObzor_Click);
            // 
            // bt_ZipFileObzor
            // 
            this.bt_ZipFileObzor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_ZipFileObzor.Location = new System.Drawing.Point(389, 419);
            this.bt_ZipFileObzor.Name = "bt_ZipFileObzor";
            this.bt_ZipFileObzor.Size = new System.Drawing.Size(48, 23);
            this.bt_ZipFileObzor.TabIndex = 19;
            this.bt_ZipFileObzor.Text = "Обзор";
            this.bt_ZipFileObzor.Click += new System.EventHandler(this.bt_ZipFileObzor_Click);
            // 
            // tb_ZipFileName
            // 
            this.tb_ZipFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tb_ZipFileName.Location = new System.Drawing.Point(168, 421);
            this.tb_ZipFileName.Name = "tb_ZipFileName";
            this.tb_ZipFileName.Size = new System.Drawing.Size(215, 20);
            this.tb_ZipFileName.TabIndex = 18;
            // 
            // cb_AddPrefix
            // 
            this.cb_AddPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_AddPrefix.Location = new System.Drawing.Point(43, 461);
            this.cb_AddPrefix.Name = "cb_AddPrefix";
            this.cb_AddPrefix.Properties.Caption = "Добавить префикс:";
            this.cb_AddPrefix.Size = new System.Drawing.Size(126, 20);
            this.cb_AddPrefix.TabIndex = 20;
            this.cb_AddPrefix.CheckedChanged += new System.EventHandler(this.cb_AddPrefix_CheckedChanged);
            // 
            // cb_AddSufix
            // 
            this.cb_AddSufix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_AddSufix.Location = new System.Drawing.Point(349, 461);
            this.cb_AddSufix.Name = "cb_AddSufix";
            this.cb_AddSufix.Properties.Caption = "Добавить суффикс:";
            this.cb_AddSufix.Size = new System.Drawing.Size(126, 20);
            this.cb_AddSufix.TabIndex = 21;
            this.cb_AddSufix.CheckedChanged += new System.EventHandler(this.cb_AddSufix_CheckedChanged);
            // 
            // cb_SaveInOneFolder
            // 
            this.cb_SaveInOneFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_SaveInOneFolder.Location = new System.Drawing.Point(43, 487);
            this.cb_SaveInOneFolder.Name = "cb_SaveInOneFolder";
            this.cb_SaveInOneFolder.Properties.Caption = "Уложить в одну папку";
            this.cb_SaveInOneFolder.Size = new System.Drawing.Size(140, 20);
            this.cb_SaveInOneFolder.TabIndex = 22;
            this.cb_SaveInOneFolder.CheckedChanged += new System.EventHandler(this.cb_SaveInOneFolder_CheckedChanged);
            // 
            // tb_Prefix
            // 
            this.tb_Prefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tb_Prefix.Location = new System.Drawing.Point(175, 461);
            this.tb_Prefix.Name = "tb_Prefix";
            this.tb_Prefix.Properties.EditValueChanged += new System.EventHandler(this.tb_Prefix_Properties_EditValueChanged);
            this.tb_Prefix.Size = new System.Drawing.Size(137, 20);
            this.tb_Prefix.TabIndex = 23;
            // 
            // tb_Sufix
            // 
            this.tb_Sufix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tb_Sufix.Location = new System.Drawing.Point(481, 461);
            this.tb_Sufix.Name = "tb_Sufix";
            this.tb_Sufix.Size = new System.Drawing.Size(137, 20);
            this.tb_Sufix.TabIndex = 24;
            this.tb_Sufix.EditValueChanged += new System.EventHandler(this.tb_Sufix_EditValueChanged);
            // 
            // bt_Save
            // 
            this.bt_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Save.Location = new System.Drawing.Point(438, 523);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(70, 23);
            this.bt_Save.TabIndex = 25;
            this.bt_Save.Text = "Сохранить";
            // 
            // bt_Cancel
            // 
            this.bt_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Cancel.Location = new System.Drawing.Point(514, 523);
            this.bt_Cancel.Name = "bt_Cancel";
            this.bt_Cancel.Size = new System.Drawing.Size(70, 23);
            this.bt_Cancel.TabIndex = 26;
            this.bt_Cancel.Text = "Отмена";
            this.bt_Cancel.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // bt_Help
            // 
            this.bt_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Help.Location = new System.Drawing.Point(590, 523);
            this.bt_Help.Name = "bt_Help";
            this.bt_Help.Size = new System.Drawing.Size(70, 23);
            this.bt_Help.TabIndex = 27;
            this.bt_Help.Text = "Справка";
            // 
            // Proj_Clone
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 558);
            this.Controls.Add(this.bt_Help);
            this.Controls.Add(this.bt_Cancel);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.tb_Sufix);
            this.Controls.Add(this.tb_Prefix);
            this.Controls.Add(this.cb_SaveInOneFolder);
            this.Controls.Add(this.cb_AddSufix);
            this.Controls.Add(this.cb_AddPrefix);
            this.Controls.Add(this.bt_ZipFileObzor);
            this.Controls.Add(this.tb_ZipFileName);
            this.Controls.Add(this.bt_FolderObzor);
            this.Controls.Add(this.bt_FinndAndReplase);
            this.Controls.Add(this.lb_SP);
            this.Controls.Add(this.lb_Drws);
            this.Controls.Add(this.lb_Part);
            this.Controls.Add(this.pictureEdit4);
            this.Controls.Add(this.pictureEdit3);
            this.Controls.Add(this.pictureEdit2);
            this.Controls.Add(this.pictureEdit1);
            this.Controls.Add(this.lb_Sborka);
            this.Controls.Add(this.tb_FolderPath);
            this.Controls.Add(this.rb_SaveInZIPFile);
            this.Controls.Add(this.rb_SaveInFolder);
            this.Controls.Add(this.groupControl1);
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Proj_Clone.IconOptions.SvgImage")));
            this.MinimumSize = new System.Drawing.Size(640, 590);
            this.Name = "Proj_Clone";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Копировать проект";
            this.Load += new System.EventHandler(this.Proj_Clone_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_check_SP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_check_Drw.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_FolderPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_ZipFileName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_AddPrefix.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_AddSufix.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_SaveInOneFolder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_Prefix.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_Sufix.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private System.Windows.Forms.RadioButton rb_GridView;
        private System.Windows.Forms.RadioButton rb_TreeList;
        private DevExpress.XtraEditors.CheckEdit cb_check_SP;
        private DevExpress.XtraEditors.CheckEdit cb_check_Drw;
        private System.Windows.Forms.RadioButton rb_SaveInZIPFile;
        private System.Windows.Forms.RadioButton rb_SaveInFolder;
        private DevExpress.XtraEditors.TextEdit tb_FolderPath;
        private DevExpress.XtraEditors.LabelControl lb_Sborka;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit2;
        private DevExpress.XtraEditors.PictureEdit pictureEdit3;
        private DevExpress.XtraEditors.PictureEdit pictureEdit4;
        private DevExpress.XtraEditors.LabelControl lb_Part;
        private DevExpress.XtraEditors.LabelControl lb_Drws;
        private DevExpress.XtraEditors.LabelControl lb_SP;
        private DevExpress.XtraEditors.SimpleButton bt_FinndAndReplase;
        private DevExpress.XtraEditors.SimpleButton bt_FolderObzor;
        private DevExpress.XtraEditors.SimpleButton bt_ZipFileObzor;
        private DevExpress.XtraEditors.TextEdit tb_ZipFileName;
        private DevExpress.XtraEditors.CheckEdit cb_AddPrefix;
        private DevExpress.XtraEditors.CheckEdit cb_AddSufix;
        private DevExpress.XtraEditors.CheckEdit cb_SaveInOneFolder;
        private DevExpress.XtraEditors.TextEdit tb_Prefix;
        private DevExpress.XtraEditors.TextEdit tb_Sufix;
        private DevExpress.XtraEditors.SimpleButton bt_Save;
        private DevExpress.XtraEditors.SimpleButton bt_Cancel;
        private DevExpress.XtraEditors.SimpleButton bt_Help;
    }
}