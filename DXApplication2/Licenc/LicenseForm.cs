using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DXF_AutoHelp.Licensing;

namespace SaveDXF
{
    /// <summary>
    /// Окно лицензии: состояние, создание зашифрованного файла-запроса и загрузка файла лицензии от разработчика.
    /// Файл лицензии можно и просто перетащить в окно.
    /// </summary>
    public sealed class LicenseForm : XtraForm
    {
        private static string T(string ru, string en) { return ProgramLicense.T(ru, en); }
        private static ProductLicense Lic { get { return ProgramLicense.License; } }

        private LabelControl lblState;
        private TextEdit txtCustomer, txtContact, txtComment;
        private LabelControl lblResult;

        public LicenseForm()
        {
            Text = Application.ProductName + ". " + T("Лицензия", "License");
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(620, 440);
            AllowDrop = true;
            DragEnter += (s, e) => e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            DragDrop += (s, e) =>
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (files != null && files.Length > 0) ActivateFile(files[0]);
            };
            BuildUi();
            ShowState();
        }

        private void BuildUi()
        {
            const int pad = 12, width = 596, labelWidth = 150;
            int y = pad;

            lblState = new LabelControl
            {
                Location = new Point(pad, y), AutoSizeMode = LabelAutoSizeMode.Vertical, Width = width,
                Appearance = { Font = new Font(Font.FontFamily, 9.5f) }
            };
            Controls.Add(lblState);
            y += 100;

            Controls.Add(new LabelControl
            {
                Text = T("1. Создайте файл-запрос и отправьте его разработчику (почта, Telegram). Файл зашифрован.",
                         "1. Create a request file and send it to the developer (e-mail, Telegram). The file is encrypted."),
                Location = new Point(pad, y)
            });
            y += 22;
            txtCustomer = Field(T("Организация или ФИО:", "Company or name:"), ref y, pad, labelWidth, width);
            txtContact = Field(T("Телефон или e-mail:", "Phone or e-mail:"), ref y, pad, labelWidth, width);
            txtComment = Field(T("Комментарий:", "Comment:"), ref y, pad, labelWidth, width);
            var btnRequest = new SimpleButton { Text = T("Создать файл-запрос…", "Create request file…"), Location = new Point(pad + labelWidth, y), Width = 200 };
            btnRequest.Click += (s, e) => CreateRequest();
            Controls.Add(btnRequest);
            y += 44;

            Controls.Add(new LabelControl
            {
                Text = T("2. Загрузите файл лицензии license_(…).lic от разработчика (или перетащите его в это окно).",
                         "2. Load the license_(…).lic file from the developer (or drag it into this window)."),
                Location = new Point(pad, y)
            });
            y += 22;
            var btnFile = new SimpleButton { Text = T("Загрузить файл лицензии…", "Load license file…"), Location = new Point(pad + labelWidth, y), Width = 200 };
            btnFile.Click += (s, e) => LoadFromFile();
            var btnPaste = new SimpleButton { Text = T("Вставить из буфера", "Paste from clipboard"), Location = new Point(pad + labelWidth + 208, y), Width = 150 };
            btnPaste.Click += (s, e) =>
            {
                if (Clipboard.ContainsText()) ActivateText(Clipboard.GetText());
                else ShowResult(T("В буфере обмена нет текста лицензии.", "There is no license text in the clipboard."), false);
            };
            Controls.Add(btnFile);
            Controls.Add(btnPaste);
            y += 36;

            lblResult = new LabelControl { Location = new Point(pad, y), AutoSizeMode = LabelAutoSizeMode.Vertical, Width = width };
            Controls.Add(lblResult);

            var btnClose = new SimpleButton
            {
                Text = T("Закрыть", "Close"), DialogResult = DialogResult.Cancel, Width = 100,
                Location = new Point(pad + width - 100, ClientSize.Height - pad - 24)
            };
            Controls.Add(btnClose);
            CancelButton = btnClose;
        }

        private TextEdit Field(string caption, ref int y, int pad, int labelWidth, int width)
        {
            Controls.Add(new LabelControl { Text = caption, Location = new Point(pad, y + 3) });
            var edit = new TextEdit { Location = new Point(pad + labelWidth, y), Width = width - labelWidth };
            Controls.Add(edit);
            y += 28;
            return edit;
        }

        private void ShowState()
        {
            lblState.Text = Lic.DescribeCurrent();
            lblState.Appearance.ForeColor = Lic.Source == LicenseSource.Signed ? Color.DarkGreen
                                          : Lic.Source == LicenseSource.Legacy ? Color.DarkGoldenrod
                                          : Color.Firebrick;
            if (Lic.Signed.License != null && string.IsNullOrEmpty(txtCustomer.Text))
                txtCustomer.Text = Lic.Signed.License.Customer;
        }

        private void CreateRequest()
        {
            if (string.IsNullOrWhiteSpace(txtCustomer.Text))
            {
                ShowResult(T("Укажите организацию или ФИО — так разработчик поймёт, чей это запрос.",
                             "Enter the company or name so the developer knows whose request it is."), false);
                txtCustomer.Focus();
                return;
            }
            LicenseRequest request;
            try
            {
                Cursor = Cursors.WaitCursor;
                request = Lic.CreateRequest(txtCustomer.Text.Trim(), txtContact.Text.Trim(), txtComment.Text.Trim());
            }
            catch (Exception ex) { ShowResult(ex.Message, false); return; }
            finally { Cursor = Cursors.Default; }

            using (var d = new SaveFileDialog
            {
                FileName = request.SuggestedFileName + LicenseRequest.FileExtension,
                Filter = T("Файл-запрос", "Request file") + " (*.datq)|*.datq",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            })
            {
                if (d.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    Lic.SaveRequest(request, d.FileName);
                    ShowResult(T("Файл-запрос сохранён: ", "Request file saved: ") + d.FileName, true);
                    Process.Start("explorer.exe", "/select,\"" + d.FileName + "\"");
                }
                catch (Exception ex) { ShowResult(ex.Message, false); }
            }
        }

        private void LoadFromFile()
        {
            using (var d = new OpenFileDialog { Filter = T("Лицензия", "License") + " (*.lic)|*.lic|*.*|*.*" })
                if (d.ShowDialog(this) == DialogResult.OK) ActivateFile(d.FileName);
        }

        private void ActivateFile(string path)
        {
            string text;
            try { text = File.ReadAllText(path); }
            catch (Exception ex) { ShowResult(ex.Message, false); return; }
            ActivateText(text);
        }

        private void ActivateText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) { ShowResult(T("Файл лицензии пуст.", "The license file is empty."), false); return; }
            LicenseCheck check;
            try { check = Lic.Activate(text); }
            catch (Exception ex) { ShowResult(ex.Message, false); return; }

            ShowState();
            ShowResult(check.IsValid ? T("Лицензия активирована.", "License activated.") : Lic.Describe(check), check.IsValid);
        }

        private void ShowResult(string text, bool ok)
        {
            lblResult.Text = text;
            lblResult.Appearance.ForeColor = ok ? Color.DarkGreen : Color.Firebrick;
        }
    }
}
