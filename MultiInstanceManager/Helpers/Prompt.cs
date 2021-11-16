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
        public static string ShowDialog(string text, string caption, bool isPassword=false)
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
    }
}
