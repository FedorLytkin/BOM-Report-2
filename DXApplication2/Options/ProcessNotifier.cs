using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace VSNRM_Kompas.Options
{
    /// <summary>
    /// Способ вывода ошибок во время процесса (сканирование, экспорт).
    /// </summary>
    public enum Notification_Method_Enum
    {
        [Description("Уведомлять ошибку в отдельном сообщении")]
        MessagesBox = 0,
        [Description("Уведомлять обо всех ошибках в одном сообщении, в конце")]
        NotificationListForm = 1
    }

    /// <summary>
    /// Вывод ошибок во время процессов (сканирование, экспорт).
    /// Режим задаётся параметрами NotificationScan / NotificationExport:
    ///  - MessagesBox: каждая ошибка сразу показывается отдельным сообщением (как раньше);
    ///  - NotificationListForm: ошибки копятся и показываются одним окном в конце процесса.
    /// Begin/End могут вкладываться друг в друга: итог показывает только внешний End.
    /// </summary>
    public static class ProcessNotifier
    {
        private static int depth;
        private static Notification_Method_Enum method = Notification_Method_Enum.MessagesBox;
        private static string processName;
        private static readonly List<string> errors = new List<string>();

        public static bool Collecting => depth > 0 && method == Notification_Method_Enum.NotificationListForm;

        public static void Begin(Notification_Method_Enum notificationMethod, string ProcessName)
        {
            if (depth++ > 0) return;
            method = notificationMethod;
            processName = ProcessName;
            errors.Clear();
        }
        public static void End()
        {
            if (depth == 0 || --depth > 0) return;
            if (method == Notification_Method_Enum.NotificationListForm && errors.Count > 0)
                ShowErrorList(processName, errors);
            errors.Clear();
            method = Notification_Method_Enum.MessagesBox;
        }
        /// <summary>
        /// Ошибка процесса: в режиме списка откладывается до End, иначе показывается сразу.
        /// </summary>
        public static void Error(string MsgText, string Caption = null, MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            if (Collecting)
            {
                errors.Add(MsgText);
                return;
            }
            MessageBox.Show(MsgText, Caption ?? Application.ProductName, MessageBoxButtons.OK, icon);
        }
        /// <summary>
        /// Ошибка, которая раньше не выводилась сообщением (только в лог): попадает лишь в итоговый список.
        /// </summary>
        public static void AddToList(string MsgText)
        {
            if (Collecting) errors.Add(MsgText);
        }

        private static void ShowErrorList(string ProcessName, List<string> errorList)
        {
            //одинаковые ошибки выводим один раз с количеством повторов
            var grouped = errorList.GroupBy(er => er).Select(g => g.Count() > 1 ? $"{g.Key} (x{g.Count()})" : g.Key).ToList();
            string text = string.Join(Environment.NewLine + Environment.NewLine,
                grouped.Select((er, i) => $"{i + 1}. {er.Replace("\r\n", "\n").Replace("\n", Environment.NewLine + "    ")}"));

            using (Form form = new Form())
            {
                form.Text = $"{Application.ProductName}. {ProcessName}";
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Size = new Size(720, 450);
                form.MinimizeBox = false;
                form.ShowIcon = false;
                form.ShowInTaskbar = false;

                Label label = new Label
                {
                    Dock = DockStyle.Top,
                    Height = 30,
                    Padding = new Padding(8, 8, 8, 0),
                    Text = string.Format("В процессе возникли ошибки: {0}", errorList.Count)
                };
                TextBox textBox = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Both,
                    WordWrap = false,
                    BackColor = SystemColors.Window,
                    Text = text
                };
                FlowLayoutPanel buttons = new FlowLayoutPanel
                {
                    Dock = DockStyle.Bottom,
                    Height = 40,
                    FlowDirection = FlowDirection.RightToLeft,
                    Padding = new Padding(4)
                };
                Button ok = new Button { Text = "OK", DialogResult = DialogResult.OK, Width = 90 };
                Button copy = new Button { Text = "Копировать", Width = 90 };
                copy.Click += (s, e) => Clipboard.SetText(text);
                buttons.Controls.Add(ok);
                buttons.Controls.Add(copy);

                form.Controls.Add(textBox);
                form.Controls.Add(label);
                form.Controls.Add(buttons);
                form.AcceptButton = ok;
                form.CancelButton = ok;
                textBox.Select(0, 0);
                form.ShowDialog();
            }
        }
    }

    /// <summary>
    /// Заполнение и чтение выпадающих списков "Уведомления об ошибках" в окне настроек
    /// (сами списки - cb_NotificationScan / cb_NotificationExport - лежат в дизайнере формы).
    /// </summary>
    public static class NotificationOptionsCombo
    {
        /// <summary>Заполняет список вариантами Notification_Method_Enum и выбирает value</summary>
        public static void Fill(DevExpress.XtraEditors.ComboBoxEdit combo, Notification_Method_Enum value)
        {
            combo.Properties.Items.Clear();
            //пункты идут в порядке значений Notification_Method_Enum: SelectedIndex == значение enum
            foreach (Notification_Method_Enum m in Enum.GetValues(typeof(Notification_Method_Enum)))
                combo.Properties.Items.Add(GetDescription(m));
            combo.SelectedIndex = (int)value;
        }
        /// <summary>Выбранный в списке способ уведомления</summary>
        public static Notification_Method_Enum GetValue(DevExpress.XtraEditors.ComboBoxEdit combo) =>
            combo.SelectedIndex >= 0 ? (Notification_Method_Enum)combo.SelectedIndex : Notification_Method_Enum.MessagesBox;
        private static string GetDescription(Notification_Method_Enum value)
        {
            var field = typeof(Notification_Method_Enum).GetField(value.ToString());
            var attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attr != null ? attr.Description : value.ToString();
        }
    }
}