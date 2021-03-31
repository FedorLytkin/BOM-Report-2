using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
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
    public partial class FindAndRepace_Form : DevExpress.XtraEditors.XtraForm
    {
        FindAndRepace_Class FindAndRepace;
        public TreeList treeList;
        public FindAndRepace_Form()
        {
            InitializeComponent();
            FindAndRepace = new FindAndRepace_Class();
        }
        private void FindAndRepace_Form_Load(object sender, EventArgs e)
        {
            AddControls();
        }
        private void AddControls()
        {
            FindAndRepace.treeList = treeList;
            foreach (string ColName in FindAndRepace.GetTreeListColumnsName())
                cb_FindParams.Properties.Items.Add(ColName);
            if (cb_FindParams.Properties.Items.Count > 0) cb_FindParams.SelectedIndex = 0;
            cb_NotCheckRegister.Checked = FindAndRepace.Register_Without;
            cb_To4noe.Checked = FindAndRepace.To4noe;

            switch (FindAndRepace.find_Method_Enum)
            {
                case FindAndRepace_Class.Find_Method_Enum.Check_Elements:
                    rb_CheckElement.Checked = true;
                    rb_NotCheckElements.Checked = false;
                    rb_ReplaceText.Checked = false;
                    tb_ReplceText.Enabled = false;
                    break;
                case FindAndRepace_Class.Find_Method_Enum.Not_Check_Elements:
                    rb_CheckElement.Checked = false;
                    rb_NotCheckElements.Checked = true;
                    rb_ReplaceText.Checked = true;
                    tb_ReplceText.Enabled = false;
                    break;
                case FindAndRepace_Class.Find_Method_Enum.RepaceText:
                    rb_CheckElement.Checked = false;
                    rb_NotCheckElements.Checked = false;
                    rb_ReplaceText.Checked = true;
                    tb_ReplceText.Enabled = true;
                    break;
            }
        }
        private void bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rb_CheckElement_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_CheckElement.Checked)
            {
                FindAndRepace.find_Method_Enum = FindAndRepace_Class.Find_Method_Enum.Check_Elements;

                switch (FindAndRepace.find_Method_Enum)
                {
                    case FindAndRepace_Class.Find_Method_Enum.Check_Elements:
                        rb_CheckElement.Checked = true;
                        rb_NotCheckElements.Checked = false;
                        rb_ReplaceText.Checked = false;
                        tb_ReplceText.Enabled = false;
                        bt_all.Text = "Проверить все";
                        bt_Next.Text = "Проверить";
                        break;
                    case FindAndRepace_Class.Find_Method_Enum.Not_Check_Elements:
                        rb_CheckElement.Checked = false;
                        rb_NotCheckElements.Checked = true;
                        rb_ReplaceText.Checked = false;
                        tb_ReplceText.Enabled = false;
                        bt_all.Text = "Проверить все";
                        bt_Next.Text = "Проверить";
                        break;
                    case FindAndRepace_Class.Find_Method_Enum.RepaceText:
                        rb_CheckElement.Checked = false;
                        rb_NotCheckElements.Checked = false;
                        rb_ReplaceText.Checked = true;
                        tb_ReplceText.Enabled = true;
                        bt_all.Text = "Заменить все";
                        bt_Next.Text = "Заменить";
                        break;
                }
            }
        }

        private void rb_NotCheckElements_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_NotCheckElements.Checked)
            {
                FindAndRepace.find_Method_Enum = FindAndRepace_Class.Find_Method_Enum.Not_Check_Elements;

                switch (FindAndRepace.find_Method_Enum)
                {
                    case FindAndRepace_Class.Find_Method_Enum.Check_Elements:
                        rb_CheckElement.Checked = true;
                        rb_NotCheckElements.Checked = false;
                        rb_ReplaceText.Checked = false;
                        tb_ReplceText.Enabled = false;
                        bt_all.Text = "Проверить все";
                        bt_Next.Text = "Проверить";
                        break;
                    case FindAndRepace_Class.Find_Method_Enum.Not_Check_Elements:
                        rb_CheckElement.Checked = false;
                        rb_NotCheckElements.Checked = true;
                        rb_ReplaceText.Checked = false;
                        tb_ReplceText.Enabled = false;
                        bt_all.Text = "Проверить все";
                        bt_Next.Text = "Проверить";
                        break;
                    case FindAndRepace_Class.Find_Method_Enum.RepaceText:
                        rb_CheckElement.Checked = false;
                        rb_NotCheckElements.Checked = false;
                        rb_ReplaceText.Checked = true;
                        tb_ReplceText.Enabled = true;
                        bt_all.Text = "Заменить все";
                        bt_Next.Text = "Заменить";
                        break;
                }
            }
        }

        private void rb_ReplaceText_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_ReplaceText.Checked)
            {
                FindAndRepace.find_Method_Enum = FindAndRepace_Class.Find_Method_Enum.RepaceText;

                switch (FindAndRepace.find_Method_Enum)
                {
                    case FindAndRepace_Class.Find_Method_Enum.Check_Elements:
                        rb_CheckElement.Checked = true;
                        rb_NotCheckElements.Checked = false;
                        rb_ReplaceText.Checked = false;
                        tb_ReplceText.Enabled = false;
                        bt_all.Text = "Проверить все";
                        bt_Next.Text = "Проверить";
                        break;
                    case FindAndRepace_Class.Find_Method_Enum.Not_Check_Elements:
                        rb_CheckElement.Checked = false;
                        rb_NotCheckElements.Checked = true;
                        rb_ReplaceText.Checked = false;
                        tb_ReplceText.Enabled = false;
                        bt_all.Text = "Проверить все";
                        bt_Next.Text = "Проверить";
                        break;
                    case FindAndRepace_Class.Find_Method_Enum.RepaceText:
                        rb_CheckElement.Checked = false;
                        rb_NotCheckElements.Checked = false;
                        rb_ReplaceText.Checked = true;
                        tb_ReplceText.Enabled = true;
                        bt_all.Text = "Заменить все";
                        bt_Next.Text = "Заменить";
                        break;
                }
            }
        }

        private void bt_all_Click(object sender, EventArgs e)
        {
            FindAndRepace.FindAll(cb_FindParams.Text, tb_FindText.Text, tb_ReplceText.Text);
        }

        private void tb_FindText_EditValueChanged(object sender, EventArgs e)
        {
            FindAndRepace.FindTextList(cb_FindParams.Text, tb_FindText.Text);
        }

        private void bt_Next_Click(object sender, EventArgs e)
        {
            FindAndRepace.FindNext(cb_FindParams.Text, tb_FindText.Text, tb_ReplceText.Text);
        }

        private void cb_NotCheckRegister_CheckedChanged(object sender, EventArgs e)
        {
            FindAndRepace.Register_Without = cb_NotCheckRegister.Checked;
        }

        private void cb_To4noe_CheckedChanged(object sender, EventArgs e)
        {
            FindAndRepace.To4noe = cb_To4noe.Checked;
        }
    }
}