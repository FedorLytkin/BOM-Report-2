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
using VSNRM_Kompas;
using VSNRM_Kompas.Options;

namespace About_Control
{
    public partial class AboutForm : DevExpress.XtraEditors.XtraForm
    {
        CFG_Class optionClass;
        public AboutForm()
        {
            InitializeComponent();
            optionClass = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).Main_Options;

            this.Text = $"О программе {Application.ProductName}";

            labelProductName.Text += ": " + Application.ProductName;
            labelVersion.Text += ": " + Application.ProductVersion;
            labelCompanyName.Text += ": " + Application.CompanyName;
            labelCopyright.Text += ": Copyright © 2020 - " + DateTime.Now.Year.ToString();
            textBoxDescription.Text += SysInformation();
        }
        string SysInformation()
        {
            string result = null;
            string BildDate = optionClass.OptionInformation.Bild_Create_Date;
            string BD_Version = optionClass.OptionInformation.BD_Version;
            string Cad_Name = optionClass.OptionInformation.Cad_Name;
            string Email = optionClass.OptionInformation.Email;
            string Phone = optionClass.OptionInformation.Phone;

            result = 
$@"
Исполняемый файл: {Application.ExecutablePath}

Версия исполняемого файла: {Application.ProductVersion} 

Дата сборки: {BildDate}

Версия файла настроек: {BD_Version}

CAD-система: {Cad_Name}

Рекомендованная версия CAD-системы: {optionClass.OptionInformation.Cad_Version_Recomend}

Установленная версия CAD-системы: {optionClass.OptionInformation.Cad_VersionInPC}

Электронная почта: {Email}

Телефон: {Phone}
";
            return result;
        }
        private void bt_Ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}