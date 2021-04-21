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
using VSNRM_Kompas.API_Toops;
using System.IO;
using SaveDXF;
using System.IO.Compression;
using System.Diagnostics;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.Utils.Menu;

namespace VSNRM_Kompas.ProjectClone
{
    public partial class Proj_Clone : DevExpress.XtraEditors.XtraForm
    {
        RepositoryItemPictureEdit pictureEdit;
        Pr_Clone_Class Pr_Clone;
        FindAndRepace_Form findAndRepace_Form;
        FindAndRepace_Class findAndRepace_Class;
        RepositoryItemCheckEdit checkEdit;
        Dictionary<TreeListColumn, bool> checkedColumns = new Dictionary<TreeListColumn, bool>();
        public Body body;
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

            if(Donor_treeList != null && Donor_treeList.Nodes.Count != 0)
            {
                ComponentInfo componentInfo = (ComponentInfo)Donor_treeList.Nodes[0].Tag;
                Pr_Clone.FolderPath = $@"{Path.GetDirectoryName(componentInfo.FFN)}\{Path.GetFileNameWithoutExtension(componentInfo.FFN)}";
                Pr_Clone.ZipFileName = $@"{Path.GetDirectoryName(componentInfo.FFN)}\{Path.GetFileNameWithoutExtension(componentInfo.FFN)}.ZIP";
            }

            Pr_Clone.LB_Sborka = lb_Sborka;
            Pr_Clone.LB_Part = lb_Part;
            Pr_Clone.LB_Drw = lb_Drws;
            Pr_Clone.LB_SP = lb_SP;
            Pr_Clone.Bild_Tree();
            checkEdit = (RepositoryItemCheckEdit)treeList1.RepositoryItems.Add("CheckEdit");

            tb_FolderPath.Text = Pr_Clone.FolderPath;
            tb_ZipFileName.Text = Pr_Clone.ZipFileName;
            findAndRepace_Form = new FindAndRepace_Form();
            findAndRepace_Class = new FindAndRepace_Class();

            findAndRepace_Form.FindAndRepace  = findAndRepace_Class;
            Pr_Clone.FindAndRepace = findAndRepace_Class;
        }
        private void Proj_Clone_Load(object sender, EventArgs e)
        {

        }
        private void AddOptionInControls()
        {
            cb_check_Drw.Checked = Pr_Clone.check_Drw;
            cb_check_SP.Checked = Pr_Clone.check_SP;

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
            findAndRepace_Form.treeList = treeList1;
            findAndRepace_Form.ShowDialog();
        }

        private void bt_FolderObzor_Click(object sender, EventArgs e)
        {
            string folderPuth = Pr_Clone.GetFolderName();
            if (!string.IsNullOrEmpty(folderPuth))
            {
                tb_FolderPath.Text = folderPuth;
                Pr_Clone.ZipFileName = $@"{folderPuth}.ZIP";
                tb_ZipFileName.Text = Pr_Clone.ZipFileName;
            }   
        }

        private void bt_ZipFileObzor_Click(object sender, EventArgs e)
        {
            string ZipFileName = Pr_Clone.GetZipFileName();
            if (!string.IsNullOrEmpty(ZipFileName))
            {
                tb_ZipFileName.Text = ZipFileName;
                Pr_Clone.FolderPath = $@"{Path.GetDirectoryName(ZipFileName)}\{Path.GetFileNameWithoutExtension(ZipFileName)}";
                tb_FolderPath.Text = Pr_Clone.FolderPath;
            }   
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
                tb_Prefix.Text = null;
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
                tb_Sufix.Text = null;
            }
        }

        private void cb_SaveInOneFolder_CheckedChanged(object sender, EventArgs e)
        {
            Pr_Clone.SaveInOneFolder = cb_SaveInOneFolder.Checked;
            Pr_Clone.SetOutFolderPathInComponents();
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

        private void tb_Prefix_Properties_EditValueChanged(object sender, EventArgs e)
        {
            Pr_Clone.Prefix_Value = tb_Prefix.Text;
            Pr_Clone.SetPrefixAndSufix();
        }

        private void tb_Sufix_EditValueChanged(object sender, EventArgs e)
        {
            Pr_Clone.Sufix_Value = tb_Sufix.Text;
            Pr_Clone.SetPrefixAndSufix();
        }

        
        private void bt_Save_Click(object sender, EventArgs e)
        {
            if (MainForm.thisDemo)
            {
                MessageBox.Show("Вы используете DEMO-версию продукта " + Application.ProductName + Environment.NewLine +
                                "В DEMO-версии не доступна опция Сохранение/Копирование проекта" + Environment.NewLine +
                                "Для использования всех функций программы, обратитесь к разработчику приложения (Справка - Контакты - Email)", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            body._Pr_Clone_Class = Pr_Clone;
            body.SetLinks(treeList1.GetNodeList());
            if (Pr_Clone.saveEnum == VSNRM_Kompas.ProjectClone.Pr_Clone_Class.SaveEnum.InZipFile)
                ZipFile.CreateFromDirectory(Pr_Clone.FolderPath, Pr_Clone.getFreeFileName(Pr_Clone.ZipFileName));
            MessageBox.Show("Копирование проекта завершено!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bt_Help_Click(object sender, EventArgs e)
        {
            Process.Start("https://youtu.be/Hkf844D2z64");
        }
        private void check_Drawing_Nodes_In_Parent_Node(TreeListNode TLN)
        { 
            ComponentInfo componentInfo = (ComponentInfo)TLN.Tag;
            foreach (TreeListNode node in TLN.Nodes)
            {
                if (node.GetValue("Тип").ToString().ToUpper() == ".CDW" || node.GetValue("Тип").ToString().ToUpper() == ".SPW")
                    node.Checked = TLN.Checked;
            }
            foreach (TreeListNode node in treeList1.GetNodeList())
            {
                ComponentInfo tmp_componentInfo = (ComponentInfo)node.Tag;
                if (tmp_componentInfo.FFN == componentInfo.FFN)
                {
                    node.Checked = TLN.Checked;
                    foreach (TreeListNode subnode in node.Nodes)
                    {
                        if (subnode.GetValue("Тип").ToString().ToUpper() == ".CDW" || subnode.GetValue("Тип").ToString().ToUpper() == ".SPW")
                            subnode.Checked = TLN.Checked;
                    }
                }
            }
        }
        private void treeList1_AfterCheckNode(object sender, NodeEventArgs e)
        {
            TreeListNode TLN = e.Node;
            check_Drawing_Nodes_In_Parent_Node(TLN);
        }

        private void treeList1_NodeCellStyle(object sender, GetCustomNodeCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "Сохранить в имени")
            {
                ComponentInfo componentInfo = (ComponentInfo)e.Node.Tag;
                if (componentInfo == null) return;
                string EditText = e.Node.GetValue("Сохранить в имени").ToString();
                if (EditText != Path.GetFileNameWithoutExtension(componentInfo.FFN))
                {
                    e.Appearance.ForeColor = Color.Green;
                    e.Appearance.Options.UseBackColor = true;
                }
            }
        }
        private void treeList1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Сохранить в имени")
            {
                ComponentInfo componentInfo = (ComponentInfo)e.Node.Tag;
                if (componentInfo == null) return;
                string EditText = e.Node.GetValue("Сохранить в имени").ToString();

                if (string.IsNullOrWhiteSpace(EditText))
                    e.Node.SetValue("Сохранить в имени", e.Node.GetValue("Имя файла"));
                foreach (TreeListNode node in treeList1.GetNodeList())
                {
                    ComponentInfo tmp_componentInfo = (ComponentInfo)node.Tag;
                    if (tmp_componentInfo.FFN == componentInfo.FFN)
                        node.SetValue("Сохранить в имени", EditText);
                }
            }
        }

        private void treeList1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            TreeList tL = sender as TreeList;
            if (tL.Nodes.Count == 0) return;
            TreeListNode SelNode = tL.FocusedNode;
            TreeListHitInfo hitInfo = tL.CalcHitInfo(e.Point);
            
            DXMenuItem menuItem = new DXMenuItem(SelNode.Checked ?  "Вкл подузлы" : "Выкл подузлы", this.SetCheckedSubNodes);
            menuItem.Tag = hitInfo.Column;
            e.Menu.Items.Add(menuItem);
        }
        private void SetCheckedSubNodes(object sender, EventArgs e)
        {
            TreeListColumn clickedColumn = (sender as DXMenuItem).Tag as TreeListColumn;
            TreeList tl = clickedColumn.TreeList;
            TreeListNode node = tl.FocusedNode;
            if (node == null) return;
            bool checkResult = node.Checked;

            foreach(TreeListNode tmp_node in node.Nodes)
            {
                tmp_node.Checked = checkResult;
                check_Drawing_Nodes_In_Parent_Node(tmp_node);
                if (tmp_node.HasChildren) SetCheckedSubNodes(tmp_node, checkResult);
            }
        }
        private void SetCheckedSubNodes(TreeListNode node, bool checkResult)
        {
            foreach (TreeListNode tmp_node in node.Nodes)
            {
                tmp_node.Checked = checkResult;
                check_Drawing_Nodes_In_Parent_Node(tmp_node);
                if (tmp_node.HasChildren) SetCheckedSubNodes(tmp_node, checkResult);
            }
        }
    }
}