using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
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

namespace VSNRM_Kompas.Diagramm.ControlClass
{
    class AllPartReport_ControllClass
    {
        TreeList treeView;
        public GridControl MainGridControl;
        public GridView Main_gridView;
        RepositoryItemPictureEdit pictureEdit;
        const string System_ColumnName = "system";
        const string System_Object_Type_ColumnName = "Тип объекта";
        const string System_Count_ColumnName = "Количество";
        const string System_Total_Count_ColumnName = "Количество общ.";
        const string System_Slide_ColumnName = "Миниатюра";

        bool AllNodes = false;

        public AllPartReport_ControllClass(TreeList GetTreeView, GridControl GetGridControl, GridView GetGridView, bool SetAllNodes)
        {
            treeView = GetTreeView;
            MainGridControl = GetGridControl;
            Main_gridView = GetGridView;
            AllNodes = SetAllNodes;

            DataTable dataTable = new DataTable();
            addColumns(dataTable);
            MainGridControl.DataSource = GetComponents(dataTable);
            RepositoryItemImageComboBox rep = Option_Class.GetRepositoryItemImageComboBox(treeView.StateImageList);
            Main_gridView.Columns[System_Object_Type_ColumnName].ColumnEdit = rep;

            pictureEdit = MainGridControl.RepositoryItems.Add("PictureEdit") as RepositoryItemPictureEdit;
            Main_gridView.CustomRowCellEdit += Main_gridView_CustomRowCellEdit;


            SetColumnsVisible();
            Main_gridView.Columns[System_ColumnName].Visible = false;
        }
        public bool AddColumn(TreeListColumn column)
        {
            try
            {
                switch (column.Name)
                {
                    case System_Total_Count_ColumnName:
                        break;
                    default:
                        GridColumn gridColumn = Main_gridView.Columns[column.Name];
                        if (gridColumn == null) gridColumn = Main_gridView.Columns.Add();
                        gridColumn.Visible = column.Visible;
                        gridColumn.FieldName = column.Name;
                        gridColumn.Caption = column.Name;
                        gridColumn.Name = column.Name;
                        gridColumn.VisibleIndex = column.VisibleIndex;
                        return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
        public bool DeleteColumn(TreeListColumn column)
        {
            GridColumn gridColumn = Main_gridView.Columns[column.Name];
            if (gridColumn == null) return true;
            Main_gridView.Columns.Remove(gridColumn);
            return false;
        }
        private void SetColumnsVisible()
        {
            foreach (TreeListColumn column in treeView.Columns)
                AddColumn(column);
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
                    case System_Object_Type_ColumnName:
                        dataTable.Columns.Add(column.Name, typeof(int));
                        break;
                    default:
                        dataTable.Columns.Add(column.Name);
                        break;
                }
            }
            dataTable.Columns.Add(System_ColumnName, typeof(ComponentInfo));
        }
        private DataTable GetComponents(DataTable dataTable)
        {

            List<TreeListNode> nodes = new List<TreeListNode>();
            if (AllNodes)
                nodes = treeView.GetNodeList();
            else
            {
                if (treeView.Nodes.Count > 0)
                {
                    foreach (TreeListNode node in treeView.Nodes[0].Nodes)
                        nodes.Add(node);
                }
                    
            }
                
            foreach (TreeListNode node in nodes)
            {
                DataRow row = null;
                ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                if(!componentInfo.HaveDrw && !componentInfo.HaveSP)
                {
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
                                        case System_Object_Type_ColumnName:
                                            row[Param.Key] = node.ImageIndex;
                                            break;
                                        case System_Total_Count_ColumnName:
                                            break;
                                        default:
                                            row[Param.Key] = Param.Value;
                                            break;
                                    }
                                }
                                row[System_Count_ColumnName] = node[System_Total_Count_ColumnName];
                                //row[System_Object_Type_ColumnName] = node.ImageIndex;
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
            }
            return dataTable;
        }
        public void RowsClear()
        {
            for (int i = 0; i < Main_gridView.RowCount;)
                Main_gridView.DeleteRow(i);
        }
        private void Main_gridView_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.Column.FieldName == System_Slide_ColumnName)
            {
                e.RepositoryItem = pictureEdit;
            }
        }
        public void ExportToCSV()
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
        public void LevelChange(bool SetAllNodes)
        {
            AllNodes = SetAllNodes;
            RowsClear();
            GetComponents((DataTable)MainGridControl.DataSource);
        }
    }
}
