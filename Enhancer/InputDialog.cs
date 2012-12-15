using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ESystem = Enhancer.SystemCalls;
using System.Diagnostics;
using System.Drawing;

namespace Enhancer.Forms
{
    public sealed class InputDialog : Form
    {
        private Label messageLabel;
        private TableLayoutPanel tableLayoutPanel;
        private FlowLayoutPanel flowLayoutPanel;
        private FlowLayoutPanel buttonsPanel;
        private List<TextBox> textBoxes;

        private InputDialog()
        {
            ClientSize = new Size(350, ClientSize.Height);

            messageLabel = new Label();
            messageLabel.Location = new Point(12, 12);
            messageLabel.Width = ClientSize.Width - 24;
            messageLabel.AutoSize = true;
            messageLabel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            messageLabel.Font = SystemFonts.DialogFont;

            Button okButton = new Button();
            okButton.Text = ESystem.LoadDialogResultCaption(ESystem.DialogResultCaption.Ok);
            okButton.DialogResult = DialogResult.OK;
            okButton.Click += new EventHandler(Close);

            Button cancelButton = new Button();
            cancelButton.Text = ESystem.LoadDialogResultCaption(ESystem.DialogResultCaption.Cancel);
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Click += new EventHandler(Close);

            textBoxes = new List<TextBox>();

            flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.Left = 12;
            flowLayoutPanel.Width = ClientSize.Width - 24;
            flowLayoutPanel.Height = 0;
            flowLayoutPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            flowLayoutPanel.AutoSize = true;

            tableLayoutPanel = new TableLayoutPanel();

            buttonsPanel = new FlowLayoutPanel();
            buttonsPanel.Left = 12;
            buttonsPanel.Width = ClientSize.Width - 24;
            buttonsPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            buttonsPanel.FlowDirection = FlowDirection.RightToLeft;
            buttonsPanel.Controls.Add(okButton);
            buttonsPanel.Controls.Add(cancelButton);

            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Controls.Add(messageLabel);
            Controls.Add(buttonsPanel);
            AcceptButton = okButton;
            CancelButton = cancelButton;
        }

        private void Close(object sender, EventArgs e)
        {
            // TODO: Check validity.
            Close();
        }

        new public DialogResult ShowDialog(IWin32Window owner)
        {
            Shipout();
            return base.ShowDialog(owner);
        }

        private void Shipout()
        {
            // TODO: Resize window to fit it's content;
            //flowLayoutPanel.Height = fixHeight(flowLayoutPanel);
            buttonsPanel.Top = flowLayoutPanel.Top + flowLayoutPanel.Height + 5;
            buttonsPanel.Height = ClientSize.Height - buttonsPanel.Top - 12;

            ClientSize = new Size(500, buttonsPanel.Top + fixHeight(buttonsPanel));
        }

        private int fixHeight(Control control)
        {
            int sum = 0;
            foreach (Control subControl in control.Controls)
            {
                sum += subControl.Height;
            }
            return sum;
        }

        #region Predicate-less ShowDialogs

        public static InputResult ShowDialog(
            IWin32Window owner,
            string message)
        {
            return ShowDialog(owner, message, o => true, typeof(string));
        }

        public static InputResult ShowDialog(
            IWin32Window owner,
            string message,
            Type inputType)
        {
            return ShowDialog(owner, message, o => true, inputType);
        }

        public static InputResult ShowDialog(
            IWin32Window owner,
            string message,
            Type inputType,
            int inputCount)
        {
            return ShowDialog(owner, message, o => true, inputType, inputCount);
        }

        public static InputResult ShowDialog(
            IWin32Window owner,
            string message,
            params Tuple<Type, string>[] fields)
        {
            return ShowDialog(owner, message, o => true, fields);
        }

        public static InputResult ShowDialog(
            IWin32Window owner,
            string message,
            params Type[] types)
        {
            return ShowDialog(owner, message, o => true, types);
        }

        #endregion Predicate-less ShowDialogs

        #region Predicated ShowDialogs

        public static InputResult ShowDialog(
            IWin32Window owner,
            string message,
            Predicate<object> predicate)
        {
            return ShowDialog(owner, message, predicate, typeof(string));
        }

        public static InputResult ShowDialog(
            IWin32Window owner,
            string message,
            Predicate<object> predicate,
            Type inputType)
        {
            InputDialog dialog = new InputDialog();

            dialog.messageLabel.Text = message;

            // FIX: Leave out flowLayoutPanel.
            dialog.textBoxes.Insert(0, new TextBox());
            dialog.textBoxes[0].WordWrap = false;
            // BUG: doesn't work?
            dialog.textBoxes[0].Width = dialog.flowLayoutPanel.ClientSize.Width;
            dialog.flowLayoutPanel.Controls.Add(dialog.textBoxes[0]);

            dialog.flowLayoutPanel.Top = dialog.messageLabel.Top + dialog.messageLabel.Height + 5;

            dialog.Controls.Add(dialog.flowLayoutPanel);

            DialogResult dres = dialog.ShowDialog(owner);

            InputResult result;
            if (dres == DialogResult.OK)
                result = new InputResult
                {
                    DialogResult = dres,
                    Values = CastArray((from tbox in dialog.textBoxes select tbox.Text).ToArray(), inputType),
                };
            else result = new InputResult { DialogResult = dres };
            return result;
        }

        public static InputResult ShowDialog(
            IWin32Window owner,
            string message,
            Predicate<object> predicate,
            Type inputType,
            int inputCount)
        {
            if (inputCount <= 0) throw new ArgumentException("It must contain atleast one input", "inputCount");
            if (inputCount == 1) return ShowDialog(owner, message, predicate, inputType);
            InputDialog dialog = new InputDialog();

            dialog.messageLabel.Text = message;

            for (int i = 0; i < inputCount; ++i)
            {
                dialog.textBoxes.Insert(i, new TextBox());
                dialog.textBoxes[i].WordWrap = false;
                dialog.textBoxes[i].Width = Math.Max(dialog.flowLayoutPanel.ClientSize.Width / inputCount, 100);
                dialog.flowLayoutPanel.Controls.Add(dialog.textBoxes[i]);
            }

            dialog.flowLayoutPanel.Top = dialog.messageLabel.Top + dialog.messageLabel.Height + 5;

            dialog.Controls.Add(dialog.flowLayoutPanel);

            DialogResult dres = dialog.ShowDialog(owner);

            InputResult result;
            if (dres == DialogResult.OK)
                result = new InputResult
                {
                    DialogResult = dres,
                    Values = CastArray((from tbox in dialog.textBoxes select tbox.Text).ToArray(), inputType),
                };
            else
                result = new InputResult { DialogResult = dres };
            return result;
        }

        public static InputResult ShowDialog(
            IWin32Window owner,
            string message,
            Predicate<object> predicate,
            params Tuple<Type, string>[] fields)
        {
            throw new NotImplementedException();
        }

        public static InputResult ShowDialog(
            IWin32Window owner,
            string message,
            Predicate<object> predicate,
            params Type[] types)
        {
            throw new NotImplementedException();
        }

        #endregion Predicated ShowDialogs

        private static object[] CastArray(string[] array, Type type)
        {
            if (type == typeof(string))
                return array;
            object[] retarray = new object[array.Count()];
            for (int i = 0; i < array.Count(); ++i)
                retarray[i] = Convert.ChangeType(array[i], type);
            return retarray;
        }

        private static object[] CastArray(string[] array, params Type[] types)
        {
            if (types.Count() < array.Count())
                throw new ArgumentException("There is not enough types supplied", "types");
            object[] retarray = new object[array.Count()];
            for (int i = 0; i < array.Count(); ++i)
            {
                if (types[i] == typeof(string))
                    retarray[i] = array[i];
                else
                    retarray[i] = Convert.ChangeType(array[i], types[i]);
            }
            return retarray;
        }

        public string[] Values { get; set; }
        public Predicate<object> Predicate { get; set; }

    }
}
