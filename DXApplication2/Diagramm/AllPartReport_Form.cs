using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VSNRM_Kompas.API_Toops;

namespace VSNRM_Kompas.Diagramm
{
    public partial class AllPartReport_Form : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        TreeList treeView;
        RepositoryItemPictureEdit pictureEdit;
        const string System_ColumnName = "system";
        const string System_Count_ColumnName = "Количество";
        const string System_Total_Count_ColumnName = "Количество общ.";
        const string System_Slide_ColumnName = "Миниатюра";

        bool AllNodes = false;
        public AllPartReport_Form()
        {
            InitializeComponent();
            All_Level_Check_CH_B.Checked = AllNodes;
            treeView = ((MainForm)Application.OpenForms["MainForm"]).treeList1;

            DataTable dataTable = new DataTable();
            addColumns(dataTable);
            MainGridControl.DataSource = GetComponents(dataTable);


            pictureEdit = MainGridControl.RepositoryItems.Add("PictureEdit") as RepositoryItemPictureEdit;
            Main_gridView.CustomRowCellEdit += Main_gridView_CustomRowCellEdit;


            SetColumnsVisible();
            Main_gridView.Columns[System_ColumnName].Visible = false; 
        } 
        private void AllPartReport_Form_Load(object sender, EventArgs e)
        {

        }
        private void Bt_Preview_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainGridControl.ShowRibbonPrintPreview();
        }

        private void bbiSaveAndNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            ExportToCSV();
        }
        private void ExportToCSV()
        {
            if (Main_gridView.RowCount == 0) return;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            string ExportFileName = "";
            saveFileDialog.Filter = "CSV files (*.CSV)|*.CSV|DOCX files (*.DOCX)|*.DOCX|HTML files (*.HTML)|*.HTML|PDF files (*.PDF)|*.PDF|TXT files (*.TXT)|*.TXT|XLS files (*.XLS)|*.XLS|XLSX files (*.XLSX)|*.XLSX|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportFileName = saveFileDialog.FileName;
                string FileFormat = Path.GetExtension(ExportFileName).ToUpper();
                switch (FileFormat)
                {
                    case ".TXT":
                        MainGridControl.ExportToText(ExportFileName);
                        break;
                    case ".HTML":
                        MainGridControl.ExportToHtml(ExportFileName);
                        break;
                    case ".PDF":
                        MainGridControl.ExportToPdf(ExportFileName);
                        break;
                    case ".DOCX":
                        MainGridControl.ExportToDocx(ExportFileName);
                        break;
                    case ".XLS":
                        MainGridControl.ExportToXls(ExportFileName);
                        break;
                    case ".XLSX":
                        MainGridControl.ExportToXlsx(ExportFileName);
                        break;
                    case ".CSV":
                        MainGridControl.ExportToCsv(ExportFileName);
                        break;
                }
                if (MessageBox.Show("Файл успешно создан! \nОткрыть?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Process.Start(ExportFileName);
                }
            }

        }
         
        private void bt_ShowColumns_ItemClick(object sender, ItemClickEventArgs e)
        {
            Main_gridView.ColumnsCustomization();
        }
        private DataTable GetComponents(DataTable dataTable)
        {

            List<TreeListNode> nodes = new List<TreeListNode>();
            if (AllNodes)
                nodes = treeView.GetNodeList();
            else
                if(treeView.Nodes.Count > 0)
                    foreach (TreeListNode node in treeView.Nodes[0].Nodes)
                        nodes.Add(node);
            foreach (TreeListNode node in nodes)
            {
                DataRow row = null;
                ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                foreach (TreeListNode MainNode in treeView.Nodes)
                {
                    if (node != MainNode)
                    {
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            ComponentInfo temp_componentInfo = (ComponentInfo)dataRow[System_ColumnName];
                            if (temp_componentInfo != null)
                            {
                                if (temp_componentInfo.Key == componentInfo.Key)
                                {
                                    row = dataRow;
                                    break;
                                }
                            }
                        }
                        if (row == null)
                        {
                            row = dataTable.Rows.Add();
                            Dictionary<string, string> Comp_params;
                            if (componentInfo.isBody) Comp_params = componentInfo.Body.ParamValueList;
                            else Comp_params = componentInfo.ParamValueList;
                            foreach (KeyValuePair<string, string> Param in Comp_params)
                            {
                                switch (Param.Key)
                                {
                                    case System_Slide_ColumnName:
                                        row[Param.Key] = componentInfo.Slide;
                                        break;
                                    case System_Count_ColumnName:
                                        row[Param.Key] = componentInfo.QNT;
                                        break;
                                    case System_Total_Count_ColumnName:
                                        break;
                                    default:
                                        row[Param.Key] = Param.Value;
                                        break;
                                } 
                            }
                            row[System_Count_ColumnName] = node[System_Total_Count_ColumnName];
                        }
                        else
                        {
                            row[System_Count_ColumnName] = Convert.ToDouble(row[System_Count_ColumnName]) + Convert.ToDouble(node[System_Total_Count_ColumnName]);
                            //row[System_Total_Count_ColumnName] = Convert.ToInt32(row[System_Total_Count_ColumnName]) + componentInfo.Total_QNT;
                        }
                        row[System_ColumnName] = componentInfo;
                    }
                }
            }
            return dataTable;
        }
        private void SetColumnsVisible()
        {
            foreach (TreeListColumn column in treeView.Columns)
            {
                switch (column.Name)
                {
                    case System_Total_Count_ColumnName:
                        break;
                    default:
                        Main_gridView.Columns[column.Name].Visible = column.Visible;
                        Main_gridView.Columns[column.Name].FieldName = column.Name;
                        Main_gridView.Columns[column.Name].Caption = column.Name;
                        Main_gridView.Columns[column.Name].Name = column.Name;
                        Main_gridView.Columns[column.Name].VisibleIndex = column.VisibleIndex;
                        break;
                }
            }
        } 
        private void addColumns(DataTable dataTable)
        {
            foreach (TreeListColumn column in treeView.Columns)
            {
                switch (column.Name)
                {
                    case System_Slide_ColumnName:
                        dataTable.Columns.Add(column.Name, typeof(object));
                        break;
                    case System_Total_Count_ColumnName:
                        break;
                    case System_Count_ColumnName:
                        dataTable.Columns.Add(column.Name, typeof(double));
                        break;
                    default:
                        dataTable.Columns.Add(column.Name);
                        break;
                }
            }
            dataTable.Columns.Add(System_ColumnName, typeof(ComponentInfo));
        } 
        private void RowsClear()
        {
            for (int i = 0; i < Main_gridView.RowCount;)
                Main_gridView.DeleteRow(i);
        }
        private void All_Level_Check_CH_B_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            AllNodes = All_Level_Check_CH_B.Checked;
            RowsClear();
            GetComponents((DataTable)MainGridControl.DataSource);
        }

        private void Main_gridView_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.Column.FieldName == System_Slide_ColumnName)
            {
                e.RepositoryItem = pictureEdit;
            }
        }

    }
}