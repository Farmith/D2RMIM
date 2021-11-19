using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiInstanceManager.Helpers
{
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption, bool isPassword = false)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            textLabel.Width = TextRenderer.MeasureText(textLabel.Text, textLabel.Font).Width;

            Button confirmation = new Button() { Text = "Continue", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            TextBox textBox = new TextBox();
            if (isPassword)
            {
                textBox = new TextBox() { Left = 50, Top = 50, Width = 400, PasswordChar = '*' };
            }
            else
            {
                textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            }
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            Log.Debug("Password: " + textBox.Text);
            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
        public static Boolean ConfirmDialog(string text,string caption, int timeout=15)
        {
            var originalText = text;
            var showText = originalText.Replace("{timeout}", timeout.ToString());
            // The form itself:
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = showText };
            textLabel.Width = TextRenderer.MeasureText(textLabel.Text, textLabel.Font).Width;

            Button yesButton = new Button() { Text = "Yes", Left = 250, Width = 100, Top = 70, DialogResult = DialogResult.Yes };
            yesButton.Click += (sender, e) => { prompt.Close(); };
            Button noButton = new Button() { Text = "No", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.No };
            yesButton.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(yesButton);
            prompt.Controls.Add(noButton);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = yesButton;

            // Various timers needed:
            var closeTimer = new System.Timers.Timer(timeout * 1000) { AutoReset = false };
            closeTimer.Elapsed += delegate
            {
                IntPtr hWnd = WindowHelper.FindWindowByCaption(IntPtr.Zero, caption);
                if (hWnd.ToInt32() != 0) AutomationHelper.PostMessage(hWnd, AutomationHelper.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            };
            closeTimer.Enabled = true;
            var countdownTimer = new System.Timers.Timer(1000) { AutoReset = false };
            var elapsedSeconds = 0;
            var stopCounting = false;
            countdownTimer.Elapsed += delegate
            {
                elapsedSeconds += 1;
                IntPtr hWnd = WindowHelper.FindWindowByCaption(IntPtr.Zero, caption);
                if (hWnd.ToInt32() != 0)
                {
                    Log.Debug("Ticking the countdown: " + elapsedSeconds);
                    if (elapsedSeconds == timeout)
                    {
                        stopCounting = true;
                    }
                    if (textLabel.InvokeRequired)
                    {
                        textLabel.BeginInvoke((MethodInvoker)delegate () { textLabel.Text = originalText.Replace("{timeout}", (timeout - elapsedSeconds).ToString()); });
                        if(!stopCounting)
                            countdownTimer.Start();
                    } else
                    {
                        textLabel.Text = originalText.Replace("{timeout}", (timeout - elapsedSeconds).ToString());
                        if(!stopCounting)
                            countdownTimer.Start();
                    }
                }
            };
            countdownTimer.Enabled = true;

            // Show the dialog and wait for result as long as we can
            try
            {
                var result = prompt.ShowDialog();

                if (result == DialogResult.No)
                {
                    return false;
                }
            } catch (Exception dx)
            {
                Log.Debug("Confirmation dialog error: " + dx.ToString());
            }
            return true;
        }
    }
}
