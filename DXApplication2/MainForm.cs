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

namespace VSNRM_Kompas
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public static bool thisDemo = true;
        public static string ProgramID = "20";
        public Body body = new Body();
        Query controller;
        public OptionClass Main_Options;
        public MainForm()
        {
            CopyINIFile copyINIFile = new CopyINIFile();
            controller = new Query(ConnectionString.ConnStr);
            InitializeComponent();
            Body.Init();
            AddColumns();
        }
        private void AddOptionControls()
        {
            Main_Options = new OptionClass();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(Main_Options.Skin_Name);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            AddOptionControls();
            //treeList1.Load += TreeList1_Load;


            for (int i = 0; i < 5; i++)
                Thread.Sleep(100);

            DemoVers_StartWindows();
            All_Level_Check_CH_B.Checked = !(thisDemo);
            Bt_NaimSpletter.Checked = true;
            Body.AppVersNOTValidStrongMessage();
        }
        private void AddColumns()
        {
            List<Column_Class> column_List = new List<Column_Class>();
            DataTable Col_dataTable = controller.Get_Data(Query.Select_Option.ColumsList);
            foreach (DataRow oRow in Col_dataTable.Rows)
            {
                Column_Class column = new Column_Class();
                column.Caption = oRow["Column_Caption"].ToString();
                column.Name = oRow["Column_Name"].ToString();
                column.FieldName = oRow["Column_FieldName"].ToString();
                column.Visible = Convert.ToBoolean(oRow["Column_Visible"].ToString());
                column.System = Convert.ToBoolean(oRow["Column_System"].ToString());
                column.Index = Convert.ToInt32(oRow["Column_Index"]);
                //column.TreeList = Convert.ToBoolean(oRow["Column_TreeList"].ToString());
                //column.Index_TreeList = Convert.ToInt32(oRow["Column_Index_TreeList"]);
                //column.Visible_TreeList = Convert.ToBoolean(oRow["Column_Visible_TreeList"]);
                column_List.Add(column);
            }
            column_List.Sort(delegate (Column_Class column_1, Column_Class column_2) { return column_1.Index.CompareTo(column_2.Index); });

            treeList1.Nodes.Clear();
            foreach (Column_Class column in column_List)
            {
                TreeListColumn listColumn = treeList1.Columns.Add();
                AddOneColumns(listColumn, column);
                if (!column.System)
                    (Col_List_CB.Edit as RepositoryItemComboBox).Items.Add(column.Name);
            }

        }
        //public void AddColumns2()
        //{
        //    List<Column_Class> ColumnsConfig =  OptionClass.GetColumnsConfig();
        //    ColumnsConfig.Sort(delegate (Column_Class column_1, Column_Class column_2){return column_1.Index.CompareTo(column_2.Index);});
        //    foreach (Column_Class column in ColumnsConfig)
        //    {
        //        TreeListColumn listColumn = treeList1.Columns.Add();
        //        AddOneColumns(listColumn, column);
        //        if (!column.System) 
        //            (Col_List_CB.Edit as RepositoryItemComboBox).Items.Add(column.Name); 
        //    }
        //}

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
            }
            else
            {
                this.Text = $"{Application.ProductName} v{Application.ProductVersion} {Body.KompasVersionFlag}";
                Licence_Manager_ribbonPageGroup1.Visible = false;
            }
        }
        private void bbiFindBOM_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        { 
            body.All_Level_Search = All_Level_Check_CH_B.Checked;
            splashScreenManager2.ShowWaitForm();
            splashScreenManager2.SetWaitFormCaption("Сканирование состава");
            body.OpenDocumentParam_API7();
            splashScreenManager2.CloseWaitForm();
        }

        private void bbiSaveAndClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteTreeListNodes();
        }
        private void DeleteTreeListNodes()
        {
            treeList1.Nodes.Clear();
        }

        private void bbiSaveAndNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExportToCSV();
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
                        treeList1.ExportToXml(ExportFileName);
                        break;
                    case ".PDF":
                        treeList1.ExportToPdf(ExportFileName);
                        break;
                    case ".DOCX":
                        treeList1.ExportToDocx(ExportFileName);
                        break;
                    case ".XLS":
                        treeList1.ExportToXls(ExportFileName);
                        break;
                    case ".XLSX":
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

        }

        private void Columns_Custom_Bt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            treeList1.OptionsCustomization.ShowBandsInCustomizationForm = true;

        }

        private void Website_bt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
            Main_Options.SaveColumnCFG(columnsConf_Save_Read.GetColumnsConfig());
            splashScreenManager2.CloseWaitForm();
        }

        public void SaveColumnCFG(List<Column_Class> Columns_Option_)
        {
            controller.Delete_Data(Query.Select_Option.DeleteColumns);
            foreach (Column_Class column_ in Columns_Option_)
            {
                controller.Set_Column_Option(column_);
            }
        }
        private void barToggleSwitch_All_Level_Check_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (thisDemo )
            {
                MessageBox.Show("Вы используете DEMO-версию продукта " + Application.ProductName + Environment.NewLine +
                                "В DEMO-версии не доступна опция Структура - Все уровни" + Environment.NewLine +
                                "Для использования всех функций программы, обратитесь к разработчику приложения (Справка - Контакты - Email)", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                All_Level_Check_CH_B.Checked = false;
                return;
            }
            body.All_Level_Search = All_Level_Check_CH_B.Checked;
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
            Process.Start("https://www.youtube.com/watch?v=zlS-UA3r4TU");
        }


        private void Update_Tree_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            splashScreenManager2.ShowWaitForm();
            splashScreenManager2.SetWaitFormCaption("Сканирование состава");
            body.UpDateTreeList();
            splashScreenManager2.CloseWaitForm();
        }

        private void DeleteColumn_Bt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string ColName = (string)Col_List_CB.EditValue;
            if (string.IsNullOrWhiteSpace(ColName)) return;
            TreeListColumn listColumn = treeList1.Columns[ColName];
            if (listColumn != null)
            {
                Column_Class column_Class = (Column_Class)listColumn.Tag;
                if(!column_Class.System)
                    treeList1.Columns.Remove(listColumn);
            }
            Col_List_CB.EditValue = "";
            AddItem_In_Combobox();
        }

        private void bbiFindBOM_AcriveDoc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            splashScreenManager2.ShowWaitForm();
            splashScreenManager2.SetWaitFormCaption("Сканирование состава");
            body.OpenThisDocument();
            splashScreenManager2.CloseWaitForm();
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
            Process.Start("https://www.youtube.com/watch?v=ZWaNM7nfIoU");
        }

        private void bt_ServiseDXFAuto_View_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process.Start("https://www.youtube.com/watch?v=7r1xRlieKsE");
        }

        private void bt_ShowColumns_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            treeList1.ColumnsCustomization();
        }

        private void Bt_NaimSpletter_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            body.Split_Naim = Bt_NaimSpletter.Checked;
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

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        { 
            controller.Set_Option_FieldValue("Skin_Name", DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName);
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
    }
}
