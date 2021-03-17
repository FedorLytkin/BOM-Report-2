
namespace VSNRM_Kompas.Diagramm
{
    partial class AllPartReport_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AllPartReport_Form));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipSeparatorItem toolTipSeparatorItem1 = new DevExpress.Utils.ToolTipSeparatorItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem3 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipSeparatorItem toolTipSeparatorItem2 = new DevExpress.Utils.ToolTipSeparatorItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem4 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem5 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipSeparatorItem toolTipSeparatorItem3 = new DevExpress.Utils.ToolTipSeparatorItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem6 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SuperToolTip superToolTip4 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem7 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem4 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipSeparatorItem toolTipSeparatorItem4 = new DevExpress.Utils.ToolTipSeparatorItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem8 = new DevExpress.Utils.ToolTipTitleItem();
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.bt_ShowColumns = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.All_Level_Check_CH_B = new DevExpress.XtraBars.BarToggleSwitchItem();
            this.bbiSaveAndNew = new DevExpress.XtraBars.BarButtonItem();
            this.Bt_Preview = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.MainGridControl = new DevExpress.XtraGrid.GridControl();
            this.Main_gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Main_gridView)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.ribbon.SearchEditItem,
            this.bt_ShowColumns,
            this.barButtonItem2,
            this.All_Level_Check_CH_B,
            this.bbiSaveAndNew,
            this.Bt_Preview});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 6;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbon.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbon.Size = new System.Drawing.Size(1003, 158);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            // 
            // bt_ShowColumns
            // 
            this.bt_ShowColumns.Caption = "Выбор колонок";
            this.bt_ShowColumns.Id = 1;
            this.bt_ShowColumns.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("bt_ShowColumns.ImageOptions.SvgImage")));
            this.bt_ShowColumns.Name = "bt_ShowColumns";
            this.bt_ShowColumns.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            toolTipTitleItem1.Text = "Выбор колонок";
            toolTipItem1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage")));
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "Показать список доступных для добавления колонок\r\nСписок доступных колонок можно " +
    "изменять с помощью инструментов: Добавить и Удалить столбец";
            toolTipTitleItem2.LeftIndent = 6;
            toolTipTitleItem2.Text = "BOM-Report";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            superToolTip1.Items.Add(toolTipSeparatorItem1);
            superToolTip1.Items.Add(toolTipTitleItem2);
            this.bt_ShowColumns.SuperTip = superToolTip1;
            this.bt_ShowColumns.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bt_ShowColumns_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "barButtonItem2";
            this.barButtonItem2.Id = 2;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // All_Level_Check_CH_B
            // 
            this.All_Level_Check_CH_B.Caption = "Развернуть состав";
            this.All_Level_Check_CH_B.Id = 3;
            this.All_Level_Check_CH_B.Name = "All_Level_Check_CH_B";
            toolTipTitleItem3.Text = "Структура - Все уровни";
            toolTipItem2.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage1")));
            toolTipItem2.LeftIndent = 6;
            toolTipItem2.Text = "При Вкл данной опции, процедура будет обрабатывать все уровни вложенности сканиру" +
    "емой сборки\r\nПри Выкл, процедура пройдет только по \"верхнему\" уровню пложенности" +
    "";
            toolTipTitleItem4.LeftIndent = 6;
            toolTipTitleItem4.Text = "BOM-Report";
            superToolTip2.Items.Add(toolTipTitleItem3);
            superToolTip2.Items.Add(toolTipItem2);
            superToolTip2.Items.Add(toolTipSeparatorItem2);
            superToolTip2.Items.Add(toolTipTitleItem4);
            this.All_Level_Check_CH_B.SuperTip = superToolTip2;
            this.All_Level_Check_CH_B.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.All_Level_Check_CH_B_CheckedChanged);
            // 
            // bbiSaveAndNew
            // 
            this.bbiSaveAndNew.Caption = "Экпорт";
            this.bbiSaveAndNew.Id = 4;
            this.bbiSaveAndNew.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("bbiSaveAndNew.ImageOptions.SvgImage")));
            this.bbiSaveAndNew.Name = "bbiSaveAndNew";
            this.bbiSaveAndNew.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            toolTipTitleItem5.Text = "Экспорт (CTRL + S)";
            toolTipItem3.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage2")));
            toolTipItem3.LeftIndent = 6;
            toolTipItem3.Text = "Сохранить данные из Дерева состава\r\nСписок доступных форматов: CSV, DOCX, PDF, HT" +
    "ML, XML, XLSX, XLS";
            toolTipTitleItem6.LeftIndent = 6;
            toolTipTitleItem6.Text = "BOM-Report";
            superToolTip3.Items.Add(toolTipTitleItem5);
            superToolTip3.Items.Add(toolTipItem3);
            superToolTip3.Items.Add(toolTipSeparatorItem3);
            superToolTip3.Items.Add(toolTipTitleItem6);
            this.bbiSaveAndNew.SuperTip = superToolTip3;
            this.bbiSaveAndNew.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiSaveAndNew_ItemClick);
            // 
            // Bt_Preview
            // 
            this.Bt_Preview.Caption = "Просмотр и Печать";
            this.Bt_Preview.Id = 5;
            this.Bt_Preview.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Bt_Preview.ImageOptions.SvgImage")));
            this.Bt_Preview.Name = "Bt_Preview";
            this.Bt_Preview.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            toolTipTitleItem7.Text = "Предпросмотр и Печать (CTRL + P)";
            toolTipItem4.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage3")));
            toolTipItem4.LeftIndent = 6;
            toolTipItem4.Text = resources.GetString("toolTipItem4.Text");
            toolTipTitleItem8.LeftIndent = 6;
            toolTipTitleItem8.Text = "BOM-Report";
            superToolTip4.Items.Add(toolTipTitleItem7);
            superToolTip4.Items.Add(toolTipItem4);
            superToolTip4.Items.Add(toolTipSeparatorItem4);
            superToolTip4.Items.Add(toolTipTitleItem8);
            this.Bt_Preview.SuperTip = superToolTip4;
            this.Bt_Preview.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Bt_Preview_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2,
            this.ribbonPageGroup3});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Главная";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.bt_ShowColumns);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "c";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.All_Level_Check_CH_B);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Настройки состава";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.bbiSaveAndNew);
            this.ribbonPageGroup3.ItemLinks.Add(this.Bt_Preview);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "Инструмента экспорта";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 464);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(1003, 24);
            // 
            // MainGridControl
            // 
            this.MainGridControl.AllowDrop = true;
            this.MainGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGridControl.Location = new System.Drawing.Point(0, 158);
            this.MainGridControl.MainView = this.Main_gridView;
            this.MainGridControl.Name = "MainGridControl";
            this.MainGridControl.Size = new System.Drawing.Size(1003, 306);
            this.MainGridControl.TabIndex = 6;
            this.MainGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.Main_gridView});
            // 
            // Main_gridView
            // 
            this.Main_gridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.Main_gridView.GridControl = this.MainGridControl;
            this.Main_gridView.Name = "Main_gridView";
            this.Main_gridView.OptionsBehavior.Editable = false;
            this.Main_gridView.OptionsBehavior.ReadOnly = true;
            this.Main_gridView.OptionsSelection.MultiSelect = true;
            this.Main_gridView.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.Main_gridView_CustomRowCellEdit);
            // 
            // AllPartReport_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 488);
            this.Controls.Add(this.MainGridControl);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("AllPartReport_Form.IconOptions.SvgImage")));
            this.Name = "AllPartReport_Form";
            this.Ribbon = this.ribbon;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "Структура объекта";
            this.Load += new System.EventHandler(this.AllPartReport_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Main_gridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarButtonItem bt_ShowColumns;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarToggleSwitchItem All_Level_Check_CH_B;
        private DevExpress.XtraBars.BarButtonItem bbiSaveAndNew;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem Bt_Preview;
        public DevExpress.XtraGrid.GridControl MainGridControl;
        public DevExpress.XtraGrid.Views.Grid.GridView Main_gridView;
    }
}