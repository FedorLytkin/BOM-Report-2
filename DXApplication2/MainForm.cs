using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using System; 
using VSNRM_Kompas.Options;
using VSNRM_Kompas.API_Toops;
using VSNRM_Kompas.CSVReportExport;
using SaveDXF;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraReports.UI;
using DevExpress.Utils.Menu;
using System.Threading;
using VSNRM_Kompas.Controllers;
using System.Data;
using About_Control;
using VSNRM_Kompas.Options.Column_Options;
using System.Drawing;
using DevExpress.XtraBars;
using System.Xml.Serialization;
using DiagramDataControllerBehavior.Data;
using VSNRM_Kompas.Diagramm.ControlClass;
using VSNRM_Kompas.Diagramm;
using VSNRM_Kompas.Options.CFG_Controll;
using VSNRM_Kompas.XMLContreller;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraVerticalGrid.Rows;
using DevExpress.XtraEditors.Controls;
using System.Drawing.Drawing2D;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid.Columns;

namespace VSNRM_Kompas
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public static bool thisDemo = true;
        public static bool thisVirtualPC = true; 
        public static string ProgramID = "20";
        public Body body = new Body();
        XMLContreller.XMLCLass controller;
        public CFG_Class Main_Options;
        public Option_Class option_Class;
        RepositoryItemPictureEdit pictureEdit;
        DiagrammForm_ControllClass diagrammForm;
        AllPartReport_ControllClass allPartReport;
        public MainForm()
        {
            CopyINIFile copyINIFile = new CopyINIFile();
            controller = new XMLContreller.XMLCLass();
            InitializeComponent();
            Body.Init();
            AddColumns(false);
            treeList1.NodeCellStyle += new GetCustomNodeCellStyleEventHandler(treeList1_NodeCellStyle);
            pictureEdit = treeList1.RepositoryItems.Add("PictureEdit") as RepositoryItemPictureEdit;
        }
        private void AddOptionControls()
        {
            Main_Options = new CFG_Class();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(Main_Options.Skin_Name.Value);
            option_Class = controller.IOptions.GetOptions(false);
            UpdateData();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            AddOptionControls();
            //treeList1.Load += TreeList1_Load;
             
            //for (int i = 0; i < 5; i++)
            //    Thread.Sleep(100); 
            DemoVers_StartWindows();
            All_Level_Check_CH_B.Checked = !(thisDemo);
            barCheckItem1.Checked = All_Level_Check_CH_B.Checked;
            bt_SplitButton.SuperTip = bt_LinkVis.SuperTip;
            Bt_NaimSpletter.Checked = option_Class.Split_Naim;
            Add_Drw_In_Tree_CH_B.Checked = option_Class.Add_Drw;
            All_Level_Check_CH_B_InAllReport.Checked = option_Class.All_Level_In_AllReport;
            bt_Dublicate.Down = option_Class.Dublicate_In_Visual; 
            Bt_Qnt_On_Line.Down = option_Class.Qnt_On_Line_In_Visual;

            rb_UserRibbon.Visible = option_Class.IVC.SpecialPan;
            bt_CutLenth.Visibility = option_Class.IVC.CutLength ? BarItemVisibility.Always : BarItemVisibility.Never;
            
            Bt_Cut_Length_ON.Down = option_Class.ICShMProperty.CutLengt_Calc;
            Bt_Bend_Count_ON.Down = option_Class.ICShMProperty.BendCount_Calc;
            Bt_Holl_Count_ON.Down = option_Class.ICShMProperty.HoleCount_Calc;
            ts_EditOn.Checked = option_Class.IST.EditOn;
            Bt_Cut_Length_ON.Visibility = option_Class.IVC.CutLength ? BarItemVisibility.Always : BarItemVisibility.Never;
            Bt_Bend_Count_ON.Visibility = option_Class.IVC.CutLength ? BarItemVisibility.Always : BarItemVisibility.Never;
            Bt_Holl_Count_ON.Visibility = option_Class.IVC.CutLength ? BarItemVisibility.Always : BarItemVisibility.Never;
            ts_EditOn.Visibility = option_Class.IVC.EditOn ? BarItemVisibility.Always : BarItemVisibility.Never;

            Bt_chek_ProfileValue.Visibility = option_Class.IVC.Check_ProfileValue ? BarItemVisibility.Always : BarItemVisibility.Never;
            Bt_chek_ProfileValue.Down = option_Class.Check_ProfileValue;

            bt_Prop_trans.Visibility = option_Class.IVC.PropertyTranslation ? BarItemVisibility.Always : BarItemVisibility.Never;
            ribbonPageGroup_SpecialPan.Visible = option_Class.IVC.SpecialPan;
            Bt_Copy.Visibility = option_Class.IVC.ProjectClone ? BarItemVisibility.Always : BarItemVisibility.Never;

            Body.AppVersNOTValidStrongMessage();
            OptionsBehaviorChangche();
            //mainRibbonControl.PageCategories["Дерево"].Visible = true;
            mainRibbonControl.PageCategories["Обозреватель"].Visible = false;
            mainRibbonControl.PageCategories["Визуализатор"].Visible = false;
            TableHightSizeByColumn(Main_gridView.Columns["Миниатюра"]);
        }

        private void TableHightSizeByColumn(GridColumn cln)
        {
            if (cln == null) return;
            if (cln.Caption != "Миниатюра") return;

            if (cln.Visible) Main_gridView.RowHeight = option_Class.ModelSlideSize + 2;
            else Main_gridView.RowHeight = default;

            TreeListColumn treeColunm = treeList1.Columns[cln.Caption];
            if (treeColunm == null) return;
            if (treeColunm.Caption != "Миниатюра") return;
            if (treeColunm.Visible) treeList1.RowHeight = option_Class.ModelSlideSize + 2;
            else treeList1.RowHeight = default;
        }
        private void OptionsBehaviorChangche()
        {
            treeList1.OptionsBehavior.ReadOnly = !option_Class.IST.EditOn;
            treeList1.OptionsBehavior.Editable = option_Class.IST.EditOn;
            Main_gridView.OptionsBehavior.ReadOnly = !option_Class.IST.EditOn;
            Main_gridView.OptionsBehavior.Editable = option_Class.IST.EditOn; 
            if (option_Class.IST.EditOn)
            {
                MessageBox.Show("Включен режим редактирования!\nБудьте внимательны при работе с Деревом состава и Обозревателем", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void AddColumns(bool askCFGFileName)
        {
            List<Column_Class> column_List = controller.IColumns.GetColumns(askCFGFileName);
            if (column_List == null) return;
            Options.Column_options.AddNewColumns_Class ColumnsUpdate = new Options.Column_options.AddNewColumns_Class();
            ColumnsUpdate.GetUpdateColumnList(column_List);

            column_List.Sort(delegate (Column_Class column_1, Column_Class column_2) { return column_1.Index.CompareTo(column_2.Index); });
            treeList1.Nodes.Clear();
            treeList1.Columns.Clear();
            foreach (Column_Class column in column_List)
            {
                TreeListColumn listColumn = new TreeListColumn();

                AddOneColumns(listColumn, column);
                treeList1.Columns.Add(listColumn);
                if (!column.System)
                    (Col_List_CB.Edit as RepositoryItemComboBox).Items.Add(column.Name);
            }
            if (treeList1.Columns["Раздел спецификации"] != null) 
                treeList1.Columns["Раздел спецификации"].SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
            if (treeList1.Columns["Позиция"] != null)
                treeList1.Columns["Позиция"].SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;

            if (treeList1.Columns["Тип объекта"] != null)
            {
                RepositoryItemImageComboBox rep = Option_Class.GetRepositoryItemImageComboBox(treeList1.StateImageList);
                treeList1.Columns["Тип объекта"].ColumnEdit = rep;
            }
        }

        public void AddItem_In_Combobox()
        {
            (Col_List_CB.Edit as RepositoryItemComboBox).Items.Clear();
            foreach (TreeListColumn column in treeList1.Columns)
            {
                Column_Class column_Class = (Column_Class)column.Tag;
                if (!column_Class.System)
                    (Col_List_CB.Edit as RepositoryItemComboBox).Items.Add(column.Name);
            }
        }
        private void TreeList1_Load(object sender, EventArgs e)
        {
            TreeList treeList = sender as TreeList;
            treeList.BeginInvoke(new Action(treeList.ShowFindPanel));
        }

        private void DemoVers_StartWindows()
        {
            CryptoClass CryptoClass_ = new CryptoClass();
            thisDemo = CryptoClass_.Form_LoadTrue(true);
            if (thisDemo)
            {
                this.Text = $"{Application.ProductName} v{Application.ProductVersion} {Body.KompasVersionFlag} !!!This DemoVersion!!!";
                Licence_Manager_ribbonPageGroup1.Visible = true;
                GoToWebSite();
            }
            else
            {
                this.Text = $"{Application.ProductName} v{Application.ProductVersion} {Body.KompasVersionFlag}";
                Licence_Manager_ribbonPageGroup1.Visible = false;
            }
            thisVirtualPC = DetectVirtualMachine_Class.DetectVirtualMachine();
            if (thisVirtualPC)
            {
                bbiFindBOM_AcriveDoc.Enabled = false;
                bbiFindBOM.Enabled = false;
                Update_Tree.Enabled = false;
                MessageBox.Show($"ВНИМАНИЕ!\nПрограмма установлена на виртуальную машину!\nДля предотвращения ошибок в работе программы, некоторый функционал будет отключен.\nДля восстановления всего функционала программы, установите {Application.ProductName} на физическую рабочую машину!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                bbiFindBOM_AcriveDoc.Enabled = true;
                bbiFindBOM.Enabled = true;
                Update_Tree.Enabled = true;
            }
        }
        private void FindBOM()
        {
            Stopwatch st = StartTimer();
            body.All_Level_Search = All_Level_Check_CH_B.Checked;
            splashScreenManager2.ShowWaitForm();
            splashScreenManager2.SetWaitFormCaption("Сканирование состава");
            body.OpenDocumentParam_API7();
            UpdateData();
            PostProcessData();
            splashScreenManager2.CloseWaitForm();
            StopTimer(st);
        }
        private void bbiFindBOM_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FindBOM();
        }

        private void bbiSaveAndClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteTreeListNodes();
        }
        private void DeleteTreeListNodes()
        {
            treeList1.Nodes.Clear();
            UpdateData();
        }
        private void DeleteAllSourse()
        {
            UpdateData();
        }
        private void bbiSaveAndNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExportToCSV();
        }
        private void ExportXML(string ExportFileName)
        {
            TreeListColumn treeListColumn_Image = new TreeListColumn();
            treeListColumn_Image = treeList1.Columns["Миниатюра"];

            treeList1.Columns.Remove(treeList1.Columns["Миниатюра"]);
            treeList1.ExportToXml(ExportFileName);
            treeList1.Columns.Add(treeListColumn_Image);
            //foreach (TreeListNode node in treeList1.GetNodeList())
            //{
            //    ComponentInfo componentInfo = (ComponentInfo)node.Tag;
            //    node.SetValue("Миниатюра", componentInfo.Slide);
            //}
        }
        private void ExportToCSV()
        {
            if (treeList1.Nodes.Count == 0) return;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            ComponentInfo componentInfo = (ComponentInfo)treeList1.Nodes[0].Tag;
            if (componentInfo != null) { 
                saveFileDialog.FileName = Path.GetFileNameWithoutExtension(componentInfo.FFN);
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(componentInfo.FFN);
            }
            string ExportFileName = "";
            saveFileDialog.Filter = "CSV files (*.CSV)|*.CSV|DOCX files (*.DOCX)|*.DOCX|HTML files (*.HTML)|*.HTML|PDF files (*.PDF)|*.PDF|TXT files (*.TXT)|*.TXT|XLS files (*.XLS)|*.XLS|XLSX files (*.XLSX)|*.XLSX|XML files (*.XML)|*.XML|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportFileName = saveFileDialog.FileName;
                string FileFormat = Path.GetExtension(ExportFileName).ToUpper();
                switch (FileFormat)
                {
                    case ".TXT":
                        treeList1.ExportToText(ExportFileName);
                        break;
                    case ".HTML":
                        treeList1.ExportToHtml(ExportFileName);
                        break;
                    case ".XML":
                        VSNRM_Kompas.Export.XMLexport.XMLExport_Class xMLExport_ = new Export.XMLexport.XMLExport_Class();
                        xMLExport_.exportToXml2(treeList1, ExportFileName);
                        //ExportXML(ExportFileName);
                        //treeList1.ExportToXml(ExportFileName);
                        break;
                    case ".PDF":
                        treeList1.ExportToPdf(ExportFileName);
                        break;
                    case ".DOCX":
                        treeList1.ExportToDocx(ExportFileName);
                        break;
                    case ".XLS":
                        ExportExl(ExportFileName);
                        return;
                        treeList1.ExportToXls(ExportFileName);
                        break; 
                    case ".XLSX":
                        ExportExl(ExportFileName);
                        return;
                        treeList1.ExportToXlsx(ExportFileName);
                        break;
                    case ".CSV":
                        treeList1.ExportToCsv(ExportFileName);
                        break;
                }
                if (MessageBox.Show("Файл успешно создан! \nОткрыть?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Process.Start(ExportFileName);
                }
            } 
        }
        private void ExportExl(string ExportFileName)
        {
            PrintableComponentLink link = new PrintableComponentLink(new PrintingSystem());
            link.Component = treeList1;
            XlsExportOptions xlsExportOptions = new XlsExportOptions();
            xlsExportOptions.RasterizeImages = true;
            xlsExportOptions.TextExportMode = TextExportMode.Value;
            link.ExportToXls(ExportFileName, xlsExportOptions);
            System.Diagnostics.Process.Start(ExportFileName);
        }
        private void AddNewColName_But_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string ColName = (string)Col_List_CB.EditValue;
            if (string.IsNullOrWhiteSpace(ColName)) return;
            TreeListColumn listColumn = treeList1.Columns[ColName];
            if (listColumn == null)
            {
                listColumn = treeList1.Columns.Add();

                Column_Class column = new Column_Class();
                column.Caption = ColName;
                column.Name = ColName;
                column.FieldName = ColName;
                column.Visible = true;
                column.System = false;
                column.Index = treeList1.Columns.Count;

                AddOneColumns(listColumn, column);
                AddItem_In_Combobox();
                allPartReport.AddColumn(listColumn);
                if (MainForm.thisDemo)
                {
                    Lock_Column_Class lock_Column_Class = new Lock_Column_Class();
                }
            }
            else
            {
                if (listColumn.Visible == false)
                {
                    if (MessageBox.Show($"Столбец {ColName} уже есть в Списке столбцов. \nСделать его видимым?", $"{Application.ProductName}. Добавить столбец", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        listColumn.Visible = true;
                        listColumn.VisibleIndex = treeList1.Columns.Count;
                    }
                }
            }
        }

        private void AddOneColumns(TreeListColumn listColumn, Column_Class column_Config)
        {
            listColumn.Caption = column_Config.Caption;
            listColumn.Name = column_Config.Name;
            listColumn.FieldName = column_Config.FieldName;
            listColumn.Visible = column_Config.Visible;
            listColumn.Tag = column_Config;
            listColumn.VisibleIndex = column_Config.Index; 
            if (!column_Config.System)
                listColumn.AppearanceHeader.BackColor = Color.Gainsboro;

        }

        private void Columns_Custom_Bt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            treeList1.OptionsCustomization.ShowBandsInCustomizationForm = true;

        }

        private void Website_bt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GoToWebSite();
        }
        private void GoToWebSite()
        {
            Process.Start("https://dxfautohelp.wixsite.com/dxfauto/%D0%BA%D0%BE%D0%BD%D1%82%D0%B0%D0%BA%D1%82%D1%8B");
        }
        private void email_bt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process.Start("mailto:DXFAutoHelpDesk@gmail.com");
        }

        private void Livence_Manager_Bt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        { 
            LicForm2 licForm2 = new LicForm2();
            licForm2.ShowDialog();
        }

        private void SaveColumnsSort_Bt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            splashScreenManager2.ShowWaitForm();
            splashScreenManager2.SetWaitFormCaption("Сохранение настроек");
            splashScreenManager2.SetWaitFormDescription("Настройка столбцов");
            ColumnsConf_Save_Read columnsConf_Save_Read = new ColumnsConf_Save_Read();

            //Main_Options.SaveColumnCFG(columnsConf_Save_Read.GetColumnsConfig());

            XMLContreller.XMLCLass xMLCLass = new XMLContreller.XMLCLass();
            xMLCLass.IColumns.SaveColums(columnsConf_Save_Read.GetColumnsConfig(), false); ;

            splashScreenManager2.CloseWaitForm();
        }

        //public void SaveColumnCFG(List<Column_Class> Columns_Option_)
        //{
        //    controller.Delete_Data(Query.Select_Option.DeleteColumns);
        //    foreach (Column_Class column_ in Columns_Option_)
        //    {
        //        controller.Set_Column_Option(column_);
        //    }
        //}
        private bool All_Level_Check_CheckedChanged(bool ResChecked)
        {
            if (thisDemo)
            {
                MessageBox.Show("Вы используете DEMO-версию продукта " + Application.ProductName + Environment.NewLine +
                                "В DEMO-версии не доступна опция Структура - Все уровни" + Environment.NewLine +
                                "Для использования всех функций программы, обратитесь к разработчику приложения (Справка - Контакты - Email)", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                body.All_Level_Search = false;
                return false;
            }
            body.All_Level_Search = ResChecked;
            return ResChecked;
        }

        private void barToggleSwitch_All_Level_Check_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            All_Level_Check_CH_B.Checked = All_Level_Check_CheckedChanged(All_Level_Check_CH_B.Checked);
            barCheckItem1.Checked = All_Level_Check_CH_B.Checked;
        }

        private void treeList1_DragDrop(object sender, DragEventArgs e)
        {
            if (!Body.AppVersNOTValidStrongMessage()) return;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] s = (string[])e.Data.GetData(DataFormats.FileDrop); 

                foreach (string fileName in s)
                {
                    if (System.IO.Path.GetExtension(fileName) == ".a3d")
                    {
                        splashScreenManager2.ShowWaitForm();
                        splashScreenManager2.SetWaitFormCaption("Сканирование состава");
                        //открыть документ
                        body.All_Level_Search = All_Level_Check_CH_B.Checked;
                        body.OpenThisDocument(fileName);
                        splashScreenManager2.CloseWaitForm();
                    }
                }
            }
        }

        private void treeList1_DragEnter(object sender, DragEventArgs e)
        { 
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void YouTobe_Help_Bt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GoYouTube();
        }

        private void UpdateBOM()
        {
            Stopwatch st = StartTimer();
            splashScreenManager2.ShowWaitForm();
            splashScreenManager2.SetWaitFormCaption("Сканирование состава");
            body.UpDateTreeList();
            UpdateData();
            PostProcessData();
            splashScreenManager2.CloseWaitForm();
            StopTimer(st);
        }
        private void Update_Tree_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UpdateBOM();
        }

        private void DeleteColumn_Bt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string ColName = (string)Col_List_CB.EditValue;
            if (string.IsNullOrWhiteSpace(ColName)) return;
            TreeListColumn listColumn = treeList1.Columns[ColName];
            if (listColumn != null)
            {
                Column_Class column_Class = (Column_Class)listColumn.Tag;
                if (!column_Class.System)
                {
                    treeList1.Columns.Remove(listColumn);
                    allPartReport.DeleteColumn(listColumn);
                }
            }
            Col_List_CB.EditValue = "";
            AddItem_In_Combobox();
        }
        private void BOM_ActiveDocum()
        {
            Stopwatch st = StartTimer();

            splashScreenManager2.ShowWaitForm();
            splashScreenManager2.SetWaitFormCaption("Сканирование состава");
            body.OpenThisDocument();

            UpdateData();
            PostProcessData();
            splashScreenManager2.CloseWaitForm();
            StopTimer(st);
        }
        private Stopwatch StartTimer()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            return stopwatch;
        }
        private void StopTimer(Stopwatch stopwatch)
        {
            if (stopwatch == null)
            {
                Tb_Timer.Caption = "";
                return;
            }

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Tb_Timer.Caption = elapsedTime;
        }
        private void bbiFindBOM_AcriveDoc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BOM_ActiveDocum();
        }

        private void Bt_Preview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        { 
            treeList1.ShowRibbonPrintPreview();
        }

        private void bt_Check_Update_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process.Start("https://cloud.mail.ru/public/5hAo/2TA1Q4oH7");
        }

        private void bt_DXFAuto_View_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process.Start("https://www.youtube.com/watch?v=Tsh8Rwx3pGk&list=PLfM9ugxzmk4Hl_OgwEGJ1sZC0jv9isOL9");
        }

        private void bt_ServiseDXFAuto_View_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process.Start("https://youtu.be/xTek__i15cs");
        }

        private void bt_ShowColumns_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            treeList1.ColumnsCustomization();
        }

        private void Bt_NaimSpletter_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            option_Class.Split_Naim = Bt_NaimSpletter.Checked;
            SaveOptions();
        }

        private void Add_Drw_In_Tree_CH_B_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            option_Class.Add_Drw = Add_Drw_In_Tree_CH_B.Checked;
            SaveOptions();
        }
        private void SaveOptions()
        {
            XMLContreller.XMLCLass xMLCLass = new XMLContreller.XMLCLass();
            xMLCLass.IOptions.SaveOptions(option_Class, false);
        }
        private void treeList1_PopupMenuShowing(object sender, DevExpress.XtraTreeList.PopupMenuShowingEventArgs e)
        {
            TreeList tL = sender as TreeList;
            if (tL.Nodes.Count == 0) return;
            TreeListHitInfo hitInfo = tL.CalcHitInfo(e.Point);
            DXMenuItem menuItem = new DXMenuItem("Открыть", this.OpenComponent);
            menuItem.Tag = hitInfo.Column;
            e.Menu.Items.Add(menuItem);
        }
        private void OpenComponent(object sender, EventArgs e)
        {
            TreeListColumn clickedColumn = (sender as DXMenuItem).Tag as TreeListColumn;
            TreeList tl = clickedColumn.TreeList;
            TreeListNode node = tl.FocusedNode;
            if (node == null) return;

            ComponentInfo componentInfo = (ComponentInfo)node.Tag;
            string FFN = componentInfo.FFN;
            body.OpenDocument(componentInfo.FFN);
            //foreach (TreeListColumn column in tl.Columns)
            //    column.SummaryFooter = SummaryItemType.None;
        }

        DXMenuCheckItem CreateMenuItemOpenComponent(GridView view, int rowHandle)
        {
            DXMenuCheckItem checkItem = new DXMenuCheckItem("Открыть",
              view.OptionsView.AllowCellMerge, null, new EventHandler(OnCellMergingClick));
            checkItem.Tag = new RowInfo(view, rowHandle);
            //checkItem.ImageOptions.Image = imageCollection1.Images[0];
            return checkItem;
        }
        void OnCellMergingClick(object sender, EventArgs e)
        {
            var gv = MainGridControl.MainView as GridView;
            var index = gv.FocusedRowHandle;
            if (index < 0) return;

            DataRow row = Main_gridView.GetDataRow(index);
            ComponentInfo componentInfo = (ComponentInfo)row["System"];
            if (componentInfo == null) return;

            string FFN = componentInfo.FFN;
            body.OpenDocument(componentInfo.FFN);
        }
        
        class RowInfo
        {
            public RowInfo(GridView view, int rowHandle)
            {
                this.RowHandle = rowHandle;
                this.View = view;
            }
            public GridView View;
            public int RowHandle;
        }

        private void Main_gridView_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.MenuType == DevExpress.XtraGrid.Views.Grid.GridMenuType.Row)
            {
                int rowHandle = e.HitInfo.RowHandle;
                DXMenuItem item = CreateMenuItemOpenComponent(view, rowHandle);
                item.BeginGroup = true;
                e.Menu.Items.Add(item);
            }
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main_Options.Skin_Name.Value = DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName;
            Main_Options.SaveOption();
        }

        private void bt_AboutBox_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            splashScreenManager2.ShowWaitForm();
            splashScreenManager2.SetWaitFormCaption("Создание Dump");
            Dump.Dump dump = new Dump.Dump();
            splashScreenManager2.CloseWaitForm();
        }

        private void bt_Demo_Dump_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process.Start("https://youtu.be/KNEi7dwS4mU");
        }

        private void treeList1_CustomNodeCellEdit(object sender, GetCustomNodeCellEditEventArgs e)
        {
            if (e.Column.FieldName == "Миниатюра")
            {
                e.RepositoryItem = pictureEdit;
            }
        }

        private void Bt_Telegram_Canal_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            goTelegram();
        }
        private void goTelegram()
        {
            Process.Start("https://t.me/BOMReport");
        }
        private void Bt_Telegram_Chat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process.Start("https://t.me/BOMReportChat");
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
        }
        int SelectButton = 1;
        private void Bt_AllParts_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bt_SplitButton.Caption = Bt_AllParts.Caption;
            bt_SplitButton.ImageOptions.SvgImage = Bt_AllParts.ImageOptions.SvgImage;
            bt_SplitButton.SuperTip = Bt_AllParts.SuperTip;
            SelectButton = 0;

            Diagramm.AllPartReport_Form allPartReport_ = new Diagramm.AllPartReport_Form();
            allPartReport_.ShowDialog();
        }

        private void bt_LinkVis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bt_SplitButton.Caption = bt_LinkVis.Caption;
            bt_SplitButton.ImageOptions.SvgImage = bt_LinkVis.ImageOptions.SvgImage;
            bt_SplitButton.SuperTip = bt_LinkVis.SuperTip;
            SelectButton = 1;

            DiagramDataControllerBehavior.DiagrammForm2 form1 = new DiagramDataControllerBehavior.DiagrammForm2();
            form1.ShowDialog();

            //Diagramm.DiagrammForm diagrammForm = new Diagramm.DiagrammForm();
            //diagrammForm.ShowDialog();
        }

        private void bt_SplitButton_ItemClick(object sender, ItemClickEventArgs e)
        { 
            PopupMenu menu = (e.Item as BarButtonItem).DropDownControl as PopupMenu;
            menu.ItemLinks[SelectButton].Item.PerformClick();
        }

        private void navigationPane1_StateChanging(object sender, DevExpress.XtraBars.Navigation.StateChangingEventArgs e)
        {
            e.Cancel = true;
        }

        private void navigationPane1_SelectedPageChanged(object sender, DevExpress.XtraBars.Navigation.SelectedPageChangedEventArgs e)
        {
            if (e.Page == null) return;
            switch (e.Page.Caption)
            {
                case "Дерево состава":
                    //mainRibbonControl.PageCategories["Дерево"].Visible = true;
                    mainRibbonControl.PageCategories["Обозреватель"].Visible = false;
                    mainRibbonControl.PageCategories["Визуализатор"].Visible = false;
                    break;
                case "Обозреватель объектов":
                    //mainRibbonControl.PageCategories["Дерево"].Visible = false;
                    mainRibbonControl.PageCategories["Обозреватель"].Visible = true;
                    mainRibbonControl.PageCategories["Визуализатор"].Visible = false;
                    mainRibbonControl.SelectPage(mainRibbonControl.PageCategories["Обозреватель"].Pages[0]);
                    break;
                case "Визуализатор связей":
                    //mainRibbonControl.PageCategories["Дерево"].Visible = false;
                    mainRibbonControl.PageCategories["Обозреватель"].Visible = false;
                    mainRibbonControl.PageCategories["Визуализатор"].Visible = true;
                    mainRibbonControl.SelectPage(mainRibbonControl.PageCategories["Визуализатор"].Pages[0]);
                    break;
            } 
        }
        private void PostProcessData()
        {
            //VSNRM_Kompas.PostProcess.IPostProcess_Class ipostpocess = new PostProcess.IPostProcess_Class(option_Class);
            //ipostpocess.Treelist_PostProcess(treeList1);
        }
        private void UpdateData()
        {
            allPartReport = new AllPartReport_ControllClass(treeList1, MainGridControl, Main_gridView, option_Class.All_Level_In_AllReport);
            diagrammForm = new DiagrammForm_ControllClass(treeList1, diagramDataBindingController1, option_Class.Dublicate_In_Visual, option_Class.Qnt_On_Line_In_Visual);
        }

        #region "Визуализатор связей"

        private void Bt_SaveAs_ItemClick(object sender, ItemClickEventArgs e)
        {
            var viewmodel = (ViewModel)diagramDataBindingController1.DataSource;

            var office = new Office();
            office.Elements = viewmodel.Elements;
            office.Connections = viewmodel.Connections;

            using (FileStream fileStream = new FileStream("classes.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Office));
                serializer.Serialize(fileStream, office);
            }
        }

        private void bt_Print_ItemClick(object sender, ItemClickEventArgs e)
        {
            diagramControl1.Print();
        }

        private void bt_Export_ItemClick(object sender, ItemClickEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "BMP files (*.BMP)|*.BMP|GIF files (*.GIF)|*.GIF|PNG files (*.PNG)|*.PNG|JPEG files (*.JPEG)|*.JPEG";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                diagramControl1.ExportDiagram(saveFileDialog.FileName);

                if (MessageBox.Show("Файл успешно создан! \nОткрыть?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Process.Start(saveFileDialog.FileName);
                }
            }
        }
        private void Dublicate_And_Qnt_On_Line()
        {
            option_Class.Qnt_On_Line_In_Visual = Bt_Qnt_On_Line.Down;
            option_Class.Dublicate_In_Visual = bt_Dublicate.Down;
            option_Class.ICShMProperty.CutLengt_Calc = Bt_Cut_Length_ON.Down;
            option_Class.ICShMProperty.BendCount_Calc = Bt_Bend_Count_ON.Down;
            option_Class.ICShMProperty.HoleCount_Calc = Bt_Holl_Count_ON.Down;
            option_Class.Check_ProfileValue = Bt_chek_ProfileValue.Down;
            SaveOptions();
        }
        private void bt_Dublicate_ItemClick(object sender, ItemClickEventArgs e)
        {
            Dublicate_And_Qnt_On_Line();
            diagrammForm.classStructureGenerator.Create_Qnt_On_Line = option_Class.Qnt_On_Line_In_Visual;
            diagrammForm.AddElements(option_Class.Dublicate_In_Visual);
        }

        private void Bt_Qnt_On_Line_ItemClick(object sender, ItemClickEventArgs e)
        {
            Dublicate_And_Qnt_On_Line();
            diagrammForm.classStructureGenerator.Create_Qnt_On_Line = option_Class.Qnt_On_Line_In_Visual;
            diagrammForm.AddConnectionList(option_Class.Dublicate_In_Visual);
        }

        private void Bt_VideoAboutLink_ItemClick(object sender, ItemClickEventArgs e)
        {
            Process.Start("https://youtu.be/TA43iv-ZIXs");
        }
        private void bt_Update_InTree_ItemClick(object sender, ItemClickEventArgs e)
        {
            UpdateData();
        }
        #endregion
        
        #region "Обозреватель(Структура) объекта"

        private void bt_Update_ItemClick(object sender, ItemClickEventArgs e)
        {
            UpdateData();
        }

        private void bt_ShowColumns_InAllReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            Main_gridView.ColumnsCustomization();
        }

        private void Bt_Preview_InAllReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainGridControl.ShowRibbonPrintPreview();
        }

        private void bt_SaveAndNew_InAllReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            allPartReport.ExportToCSV();
        }

        private void All_Level_Check_CH_B_InAllReport_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            option_Class.All_Level_In_AllReport = All_Level_Check_CH_B_InAllReport.Checked;
            allPartReport.LevelChange(option_Class.All_Level_In_AllReport);
            SaveOptions();
        }
        #endregion

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            //ComponentInfo componentInfo = (ComponentInfo)treeList1.Nodes[0].Tag;
            //body.GetInvisibleDocument();
            body.SetAttachedDoc(@"C:\Users\admin_veza\Desktop\Новая папка (3)\сборка\ПВ.000.000.165.000 _ Сборка Поддон.a3d");
            //body.getSP();
            //body.SetPropertyIPart7(componentInfo.FFN, "Обозначение", "2222");
            //body.SetPropertyIPart7(componentInfo.FFN, "Наименование", "1111");
        }

        private void Bt_ProjClone_ItemClick(object sender, ItemClickEventArgs e)
        {
            Project_Clone project_Clone = new Project_Clone(body, treeList1);
            project_Clone.ShowDialog();
        }

        private void Bt_Copy_ItemClick(object sender, ItemClickEventArgs e)
        {
            ProjectClone.Proj_Clone proj_Clone = new ProjectClone.Proj_Clone(treeList1);
            proj_Clone.body = body;
            proj_Clone.ShowDialog();
        }

        private void treeList1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
                radialMenu1.ShowPopup(MousePosition);
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            barCheckItem1.Checked = All_Level_Check_CheckedChanged(barCheckItem1.Checked);
            All_Level_Check_CH_B.Checked = barCheckItem1.Checked;
        }

        private void barCheckItem3_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            Bt_NaimSpletter.Checked = barCheckItem3.Checked;
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            Add_Drw_In_Tree_CH_B.Checked = barCheckItem2.Checked;
        }

        private void BOM_ActiveDoc_ItemClick(object sender, ItemClickEventArgs e)
        {
            BOM_ActiveDocum();
        }

        private void BOM_OpenDoc_ItemClick(object sender, ItemClickEventArgs e)
        {
            FindBOM();
        }

        private void BOM_Update_ItemClick(object sender, ItemClickEventArgs e)
        {
            UpdateBOM();
        }

        private void TreeListPanel_ItemClick(object sender, ItemClickEventArgs e)
        {
            navigationPane1.SelectedPageIndex = 0;
        }

        private void DeleteTreeList_ItemClick(object sender, ItemClickEventArgs e)
        {
            DeleteTreeListNodes();
        }

        private void ShowColumns_ItemClick(object sender, ItemClickEventArgs e)
        {
            treeList1.ColumnsCustomization();
        }

        private void OpenDocument_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void Export_ItemClick(object sender, ItemClickEventArgs e)
        {
            ExportToCSV();
        }

        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {
            treeList1.ShowRibbonPrintPreview();
        }

        private void GridViiewPanel_ItemClick(object sender, ItemClickEventArgs e)
        {
            navigationPane1.SelectedPageIndex = 1;
        }

        private void DiagramPanel_ItemClick(object sender, ItemClickEventArgs e)
        {
            navigationPane1.SelectedPageIndex = 2;
        }

        private void bt_ExportColOpt_ItemClick(object sender, ItemClickEventArgs e)
        {
            splashScreenManager2.ShowWaitForm();
            splashScreenManager2.SetWaitFormCaption("Сохранение настроек");
            splashScreenManager2.SetWaitFormDescription("Настройка столбцов");
            ColumnsConf_Save_Read columnsConf_Save_Read = new ColumnsConf_Save_Read();

            //Main_Options.SaveColumnCFG(columnsConf_Save_Read.GetColumnsConfig());

            XMLContreller.XMLCLass xMLCLass = new XMLContreller.XMLCLass();
            xMLCLass.IColumns.SaveColums(columnsConf_Save_Read.GetColumnsConfig(), true); ;

            splashScreenManager2.CloseWaitForm();
        }

        private void Bt_ImportColOpt_ItemClick(object sender, ItemClickEventArgs e)
        {
            AddColumns(true);
        }
        private int GetPosition(object NodeVal)
        {
            int poz1 = -1;
            if (option_Class.Positio_On_Value)
            {
                if (Convert.ToString(NodeVal).IndexOf(Convert.ToChar(option_Class.Positio_Split_Value)) >= 0)
                {
                    string[] arr = (Convert.ToString(NodeVal)).Split(Convert.ToChar(option_Class.Positio_Split_Value));
                    poz1 = string.IsNullOrEmpty(arr[arr.Length - 1]) ? 9999 : Convert.ToInt32(arr[arr.Length - 1]);
                }
                else
                    poz1 = string.IsNullOrEmpty(Convert.ToString(NodeVal)) ? 9999 : Convert.ToInt32(NodeVal);
            }
            else
            {
                poz1 = string.IsNullOrEmpty(Convert.ToString(NodeVal)) ? 9999 : Convert.ToInt32(NodeVal);
            }
            return poz1;
        }
        private void treeList1_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        { 
            if (e.Node1.HasChildren && !e.Node2.HasChildren)
                e.Result = e.SortOrder == SortOrder.Ascending ? -1 : 1;
            if (!e.Node1.HasChildren && e.Node2.HasChildren)
                e.Result = e.SortOrder == SortOrder.Ascending ? 1 : -1; 

            if(e.Column.Caption == "Позиция")
            {
                int poz1, poz2;
                poz1 = GetPosition(e.NodeValue1);
                poz2 = GetPosition(e.NodeValue2);
                if(e.SortOrder == SortOrder.Ascending)
                {
                    if (poz1 < poz2)
                        e.Result = e.SortOrder == SortOrder.Ascending ? -1 : 1;
                    else
                        e.Result = e.SortOrder == SortOrder.Ascending ? 1 : -1;
                }
                if(e.SortOrder == SortOrder.Descending)
                {
                    if (poz1 < poz2)
                        e.Result = e.SortOrder == SortOrder.Ascending ? -1 : 1;
                    else
                        e.Result = e.SortOrder == SortOrder.Descending ? 1 : -1;
                }
            }
        }

        private void bt_CFG_ItemClick(object sender, ItemClickEventArgs e)
        {
            OptionForm optionForm = new OptionForm();
            optionForm.IOption_Class = option_Class;
            if (optionForm.ShowDialog() == DialogResult.OK)
            {
                Options.TreeListResize.iTreeListResizeClass.ReSize(Main_gridView, Main_Options, treeList1, option_Class.ModelSlideSize);
                TableHightSizeByColumn(Main_gridView.Columns["Миниатюра"]);
            }
        }

        private void bt_Prop_trans_ItemClick(object sender, ItemClickEventArgs e)
        {
            splashScreenManager2.ShowWaitForm();
            splashScreenManager2.SetWaitFormCaption("Трансляция в узлы");
            body.TransProp_St1_V2();
            splashScreenManager2.CloseWaitForm();
            MessageBox.Show("Процесс передачи данных завершен!", "Трансляция параметров в узлы", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bt_CutLenth_ItemClick(object sender, ItemClickEventArgs e)
        {
            splashScreenManager2.ShowWaitForm();
            splashScreenManager2.SetWaitFormCaption("Подсчет лист. тела");
            body.getSheeteMetalBends();
            splashScreenManager2.CloseWaitForm(); 
        }

        private void bt_Promo_ItemClick(object sender, ItemClickEventArgs e)
        {
            PromoCods.PromoForm promoForm = new PromoCods.PromoForm();
            promoForm.promo_Class.IOption_Class = option_Class;
            promoForm.ShowDialog();
        }

        private void Bt_Cut_Length_ON_ItemClick(object sender, ItemClickEventArgs e)
        {
            Dublicate_And_Qnt_On_Line();
        }

        private void Bt_Bend_Count_ON_ItemClick(object sender, ItemClickEventArgs e)
        {
            Dublicate_And_Qnt_On_Line();
        }

        private void Bt_Holl_Count_ON_ItemClick(object sender, ItemClickEventArgs e)
        {
            Dublicate_And_Qnt_On_Line();
        }

        private void Bt_chek_ProfileValue_ItemClick(object sender, ItemClickEventArgs e)
        {
            Dublicate_And_Qnt_On_Line();
        }
        void treeList1_NodeCellStyle(object sender, DevExpress.XtraTreeList.GetCustomNodeCellStyleEventArgs e)
        {
            //ComponentInfo componentInfo = (ComponentInfo)e.Node.Tag;
            //if (componentInfo == null) return;
            //if (componentInfo.SheeMetall && !componentInfo.HaveUnfold)
            //{
            //    e.Appearance.BackColor = Color.Pink;
            //    //e.Appearance.Options.UseFont = true;
            //    //e.Appearance.Options.UseBackColor = true; 
            //}
            //if (string.IsNullOrEmpty(Convert.ToString(e.Node.GetValue("Длина профиля"))) && option_Class.Check_ProfileValue)
            //    e.Appearance.BackColor = Color.Pink;
        }
        private void ts_EditOn_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            option_Class.IST.EditOn = ts_EditOn.Checked;
            OptionsBehaviorChangche();
            SaveOptions();
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            body.SetLinkComponovGeometry(@"D:\Desktop\test1\параметризация Компас\PW.ыавыавыа.000.183.001 - Стенка НЭО.m3d");
        }

        private void Bt_2Telegram_Canal_ItemClick(object sender, ItemClickEventArgs e)
        {
            goTelegram();
        }

        private void Bt_Website_ItemClick(object sender, ItemClickEventArgs e)
        {
            GoToWebSite();
        }
        private void GoYouTube()
        {
            Process.Start("https://www.youtube.com/watch?v=417GSua2TSk&list=PLfM9ugxzmk4Ftvo9qg-Ftb5ZWumcFhA9H");
        }

        private void bt_GoYouTube_ItemClick(object sender, ItemClickEventArgs e)
        {
            GoYouTube();
        }

        private void Main_gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            if (Main_Options == null) return;
            GridColumn cln = sender as DevExpress.XtraGrid.Columns.GridColumn;
            if (cln != null)
                TableHightSizeByColumn(cln);
        }

        private void Main_gridView_DragObjectDrop(object sender, DevExpress.XtraGrid.Views.Base.DragObjectDropEventArgs e)
        {
            GridColumn cln = e.DragObject as DevExpress.XtraGrid.Columns.GridColumn;
            if (cln != null)
                TableHightSizeByColumn(cln);
        }
    }
}
