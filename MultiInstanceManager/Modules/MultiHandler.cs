using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MultiInstanceManager.Modules
{
    class MultiHandler
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        Form parent;

        private IntPtr bnetLauncherHandle;
        public MultiHandler(Form _parent)
        {
            parent = _parent;
        }
        Boolean blizzardProcessesExists()
        {
            var clients = Process.GetProcessesByName(Constants.clientExecutableName);
            var launchers = Process.GetProcessesByName(Constants.launcherExecutableName);
            var battlenets = Process.GetProcessesByName(Constants.battleNetExecutableName);

            if (clients.Length > 0 || launchers.Length > 0 || battlenets.Length > 0)
                return true;
            return false;
        }
        public void Setup(string displayName = "")
        {
            if(blizzardProcessesExists())
            {
                _ = MessageBox.Show("Close all D2R/Battle.net related programs first.");
                return;
            } 
            if(displayName.Length == 0)
            {
                displayName = Prompt.ShowDialog("Enter desired displayname for this account:", "Add Account");
            }
            ShowToolTip("Launching battle.net to set up " + displayName);
            var launcherProcess = LaunchLauncher();
            WinWait(Constants.bnetLauncherClass);
            WinWaitClose(Constants.bnetLauncherClass);
            ShowToolTip("Launcher ready, insert credentials ;)");
        }

        private Process LaunchLauncher()
        {
            string installPath = (string)Registry.GetValue(Constants.gameInstallRegKey[0], Constants.gameInstallRegKey[1], "");
            return Process.Start(installPath + "\\Diablo II Resurrected Launcher.exe");
        }
        private void ShowToolTip(string toolTipText)
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.AutoPopDelay = 5000;
            var location = new Point(50, 50);
            ToolTip1.Show(toolTipText,parent,50,50,5000);
        }
        private Boolean HasClass(IntPtr windowHandle, string windowClass)
        {
            StringBuilder lpsClassName = new StringBuilder();
            var classes = GetClassName(windowHandle, lpsClassName, 30);
            LogDebug("Current windowClass: " + lpsClassName.ToString() + "\r\n");
            if (lpsClassName.ToString() == windowClass)
                return true;
            return false;
        }
        private void WinWait(string windowClass)
        {
            IntPtr windowHandle = GetForegroundWindow();
            int maxWait = 100;
            int waitTime = 0;
            while (!HasClass(windowHandle,windowClass) && waitTime < maxWait ) 
            {
                LogDebug("Could not find window of class: " + windowClass);
                Thread.Sleep(100);
                windowHandle = GetForegroundWindow();
                waitTime++;
                if (waitTime >= maxWait)
                    LogDebug("Giving up finding window");
            }
            bnetLauncherHandle = windowHandle;
            LogDebug("Found window, yeet");
        }
        private void WinWaitClose(string windowClass)
        {
            IntPtr windowHandle = GetForegroundWindow();
            int maxWait = 100;
            int waitTime = 0;
            while (HasClass(windowHandle, windowClass) && waitTime < maxWait)
            {
                LogDebug("We still waiting");
                Thread.Sleep(100);
                windowHandle = GetForegroundWindow();
                waitTime++;
                if (waitTime >= maxWait)
                    LogDebug("Giving up waiting");
            }
            LogDebug("Finally its closed");
        }
        private void LogDebug(string text)
        {
            File.AppendAllText("debug.log", text + "\r\n");
        }
    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
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

            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Continue", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
