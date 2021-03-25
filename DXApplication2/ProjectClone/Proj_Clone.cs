using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VSNRM_Kompas.ProjectClone
{
    public partial class Proj_Clone : DevExpress.XtraEditors.XtraForm
    {
        Pr_Clone_Class Pr_Clone;
        public Proj_Clone()
        {
            InitializeComponent();
            Pr_Clone = new Pr_Clone_Class();
            AddOptionInControls();
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
        }

        private void cb_check_Drw_CheckedChanged(object sender, EventArgs e)
        {
            Pr_Clone.check_Drw = cb_check_Drw.Checked;
        }

        private void cb_check_SP_CheckedChanged(object sender, EventArgs e)
        {
            Pr_Clone.check_SP = cb_check_SP.Checked;
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
    }
}