using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraTreeList.ViewInfo;

namespace VSNRM_Kompas.ProjectClone
{
    public partial class Proj_Clone : DevExpress.XtraEditors.XtraForm
    {
        RepositoryItemPictureEdit pictureEdit;
        Pr_Clone_Class Pr_Clone;
        RepositoryItemCheckEdit checkEdit;
        Dictionary<TreeListColumn, bool> checkedColumns = new Dictionary<TreeListColumn, bool>();
        public Proj_Clone(TreeList Donor_treeList)
        {
            InitializeComponent();

            treeList1.OptionsView.ShowCheckBoxes = true;
            pictureEdit = treeList1.RepositoryItems.Add("PictureEdit") as RepositoryItemPictureEdit;
            this.treeList1.CustomNodeCellEdit += new DevExpress.XtraTreeList.GetCustomNodeCellEditEventHandler(this.treeList1_CustomNodeCellEdit);
            Pr_Clone = new Pr_Clone_Class();
            AddOptionInControls();

            Pr_Clone.Donor_treeList = Donor_treeList;
            Pr_Clone.This_treeList = treeList1;
            
            Pr_Clone.LB_Sborka = lb_Sborka;
            Pr_Clone.LB_Part = lb_Part;
            Pr_Clone.LB_Drw = lb_Drws;
            Pr_Clone.LB_SP = lb_SP;
            Pr_Clone.Bild_Tree();
            checkEdit = (RepositoryItemCheckEdit)treeList1.RepositoryItems.Add("CheckEdit");
        }
        private void Proj_Clone_Load(object sender, EventArgs e)
        {

        }
        private void AddOptionInControls()
        {
            cb_check_Drw.Checked = Pr_Clone.check_Drw;
            cb_check_SP.Checked = Pr_Clone.check_SP;

            if(Pr_Clone.treeViewEnum == Pr_Clone_Class.TreeViewEnum.TreeView)
            {
                rb_TreeList.Checked = true;
                rb_GridView.Checked = false;
            }
            else
            {
                rb_TreeList.Checked = false;
                rb_GridView.Checked = true;
            }

            if(Pr_Clone.saveEnum == Pr_Clone_Class.SaveEnum.InFolder)
            {
                rb_SaveInFolder.Checked = true;
                rb_SaveInZIPFile.Checked = false;

                tb_FolderPath.Enabled = true;
                bt_FolderObzor.Enabled = true;
                tb_ZipFileName.Enabled = false;
                bt_ZipFileObzor.Enabled = false;

            }
            else
            {
                rb_SaveInFolder.Checked = false;
                rb_SaveInZIPFile.Checked = true;

                tb_FolderPath.Enabled = false;
                bt_FolderObzor.Enabled = false;
                tb_ZipFileName.Enabled = true;
                bt_ZipFileObzor.Enabled = true;
            }

            cb_AddPrefix.Checked = Pr_Clone.AddPrefix;
            if (Pr_Clone.AddPrefix) 
                tb_Prefix.Enabled = true;
            else
                tb_Prefix.Enabled = false;

            cb_AddSufix.Checked = Pr_Clone.AddSufix;
            if (Pr_Clone.AddSufix)
                tb_Sufix.Enabled = true;
            else
                tb_Sufix.Enabled = false;

            cb_SaveInOneFolder.Checked = Pr_Clone.SaveInOneFolder;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bt_FinndAndReplase_Click(object sender, EventArgs e)
        {
            FindAndRepace_Form findAndRepace_Form = new FindAndRepace_Form();
            findAndRepace_Form.treeList = treeList1;
            findAndRepace_Form.ShowDialog();
        }

        private void bt_FolderObzor_Click(object sender, EventArgs e)
        {
            string folderPuth = Pr_Clone.GetFolderName();
            if (!string.IsNullOrEmpty(folderPuth))
                tb_FolderPath.Text = folderPuth;
        }

        private void bt_ZipFileObzor_Click(object sender, EventArgs e)
        {
            string ZipFileName = Pr_Clone.GetZipFileName();
            if (!string.IsNullOrEmpty(ZipFileName))
                tb_ZipFileName.Text = ZipFileName;
        }

        private void rb_SaveInFolder_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_SaveInFolder.Checked)
                Pr_Clone.saveEnum = Pr_Clone_Class.SaveEnum.InFolder;
            else
                Pr_Clone.saveEnum = Pr_Clone_Class.SaveEnum.InZipFile;

            if (Pr_Clone.saveEnum == Pr_Clone_Class.SaveEnum.InFolder)
            {
                rb_SaveInFolder.Checked = true;
                rb_SaveInZIPFile.Checked = false;

                tb_FolderPath.Enabled = true;
                bt_FolderObzor.Enabled = true;
                tb_ZipFileName.Enabled = false;
                bt_ZipFileObzor.Enabled = false;

            }
            else
            {
                rb_SaveInFolder.Checked = false;
                rb_SaveInZIPFile.Checked = true;

                tb_FolderPath.Enabled = false;
                bt_FolderObzor.Enabled = false;
                tb_ZipFileName.Enabled = true;
                bt_ZipFileObzor.Enabled = true;
            }
        }

        private void rb_SaveInZIPFile_CheckedChanged(object sender, EventArgs e)
        {
            if (!rb_SaveInZIPFile.Checked)
                Pr_Clone.saveEnum = Pr_Clone_Class.SaveEnum.InFolder;
            else
                Pr_Clone.saveEnum = Pr_Clone_Class.SaveEnum.InZipFile;

            if (Pr_Clone.saveEnum == Pr_Clone_Class.SaveEnum.InFolder)
            {
                rb_SaveInFolder.Checked = true;
                rb_SaveInZIPFile.Checked = false;

                tb_FolderPath.Enabled = true;
                bt_FolderObzor.Enabled = true;
                tb_ZipFileName.Enabled = false;
                bt_ZipFileObzor.Enabled = false;

            }
            else
            {
                rb_SaveInFolder.Checked = false;
                rb_SaveInZIPFile.Checked = true;

                tb_FolderPath.Enabled = false;
                bt_FolderObzor.Enabled = false;
                tb_ZipFileName.Enabled = true;
                bt_ZipFileObzor.Enabled = true;
            }
        }

        private void rb_TreeList_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_TreeList.Checked)
                Pr_Clone.treeViewEnum = Pr_Clone_Class.TreeViewEnum.TreeView;
            else
                Pr_Clone.treeViewEnum = Pr_Clone_Class.TreeViewEnum.GridCiew;

            if (Pr_Clone.treeViewEnum == Pr_Clone_Class.TreeViewEnum.TreeView)
            {
                rb_TreeList.Checked = true;
                rb_GridView.Checked = false;
            }
            else
            {
                rb_TreeList.Checked = false;
                rb_GridView.Checked = true;
            }
            Pr_Clone.Bild_Tree();
        }

        private void cb_check_Drw_CheckedChanged(object sender, EventArgs e)
        {
            Pr_Clone.check_Drw = cb_check_Drw.Checked;
            Pr_Clone.Bild_Tree();
        }

        private void cb_check_SP_CheckedChanged(object sender, EventArgs e)
        {
            Pr_Clone.check_SP = cb_check_SP.Checked;
            Pr_Clone.Bild_Tree();
        }

        private void cb_AddPrefix_CheckedChanged(object sender, EventArgs e)
        {
            Pr_Clone.AddPrefix = cb_AddPrefix.Checked;
            if (Pr_Clone.AddPrefix)
            {
                tb_Prefix.Enabled = true;
                Pr_Clone.Prefix_Value = tb_Prefix.Text;
            }
            else
            {
                tb_Prefix.Enabled = false;
                Pr_Clone.Prefix_Value = null;
                Pr_Clone.Prefix_Value = null;
            } 
        }

        private void cb_AddSufix_CheckedChanged(object sender, EventArgs e)
        {
            Pr_Clone.AddSufix = cb_AddSufix.Checked;
            if (Pr_Clone.AddSufix)
            {
                tb_Sufix.Enabled = true;
                Pr_Clone.Sufix_Value= tb_Sufix.Text;
            }
            else
            {
                tb_Sufix.Enabled = false;
                Pr_Clone.Sufix_Value = null;
                Pr_Clone.Sufix_Value = null;
            }
        }

        private void cb_SaveInOneFolder_CheckedChanged(object sender, EventArgs e)
        {
            Pr_Clone.SaveInOneFolder = cb_SaveInOneFolder.Checked;
        }
        private void treeList1_CustomNodeCellEdit(object sender, GetCustomNodeCellEditEventArgs e)
        {
            if (e.Column.FieldName == "Миниатюра")
            {
                e.RepositoryItem = pictureEdit;
            }
        }
         
        private void treeList1_CustomDrawColumnHeader(object sender, CustomDrawColumnHeaderEventArgs e)
        {
            if (e.Column != null && e.Column.FieldName == "Миниатюра")
            {
                Rectangle checkRect = new Rectangle(e.Bounds.Left + 3, e.Bounds.Top + 3, 12, 12);
                ColumnInfo info = (ColumnInfo)e.ObjectArgs;
                info.CaptionRect = new Rectangle(new Point(checkRect.Right + 5, info.CaptionRect.Top), info.CaptionRect.Size);
                e.Painter.DrawObject(info);
                DrawCheckBox(e.Graphics, checkEdit, checkRect, IsColumnChecked(info.Column));
                e.Handled = true;
            }
        }

        protected void DrawCheckBox(Graphics g, RepositoryItemCheckEdit edit, Rectangle r, bool Checked)
        {
            DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo info;
            DevExpress.XtraEditors.Drawing.CheckEditPainter painter;
            DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs args;
            info = edit.CreateViewInfo() as DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo;
            painter = edit.CreatePainter() as DevExpress.XtraEditors.Drawing.CheckEditPainter;
            info.EditValue = Checked;
            info.Bounds = r;
            info.CalcViewInfo(g);
            args = new DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(info, new DevExpress.Utils.Drawing.GraphicsCache(g), r);
            painter.Draw(args);
            args.Cache.Dispose();
        }
        private void EmbeddedCheckBoxChecked(TreeListColumn column)
        {
            checkedColumns[column] = !IsColumnChecked(column);
            if (IsColumnChecked(column))
                treeList1.CheckAll();
            else
                treeList1.UncheckAll();
        }
        bool IsColumnChecked(TreeListColumn column)
        {
            bool isChecked = false;
            checkedColumns.TryGetValue(column, out isChecked);
            return isChecked;

        }

        private void treeList1_MouseUp(object sender, MouseEventArgs e)
        {
            TreeList tree = sender as TreeList;
            Point pt = new Point(e.X, e.Y);
            TreeListHitInfo hit = tree.CalcHitInfo(pt);
            if (hit.Column != null)
            {
                ColumnInfo info = tree.ViewInfo.ColumnsInfo[hit.Column];
                Rectangle checkRect = new Rectangle(info.Bounds.Left + 3, info.Bounds.Top + 3, 12, 12);
                if (checkRect.Contains(pt))
                {
                    EmbeddedCheckBoxChecked(info.Column);
                    //EmbeddedCheckBoxChecked(tree);
                    //throw new DevExpress.Utils.HideException();
                }
            }
        }
    }
}