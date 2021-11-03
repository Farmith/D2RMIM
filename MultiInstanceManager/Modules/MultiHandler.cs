using Microsoft.Win32;
using System;
using System.Collections;
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
    class GameInstance
    {
        public string account;
        public Process process;
    }
    class MultiHandler
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        Form parent;

        private IntPtr lastWindowHandle;
        private int processCounter;
        private List<Process> gameProcesses;
        private uint bnetLauncherPID;
        private CheckedListBox accountList;
        private Boolean firstTokenTry;
        private byte[] token;
        private List<GameInstance> instances;

        public MultiHandler(Form _parent,CheckedListBox _accountList)
        {
            parent = _parent;
            accountList = _accountList;
            gameProcesses = new List<Process>();
            instances = new List<GameInstance>();
        }
        Boolean blizzardProcessesExists()
        {
            // var clients = Process.GetProcessesByName(Constants.clientExecutableName);
            var launchers = Process.GetProcessesByName(Constants.launcherExecutableName);
            var battlenets = Process.GetProcessesByName(Constants.battleNetExecutableName);

            if (/*clients.Length > 0 || */launchers.Length > 0 || battlenets.Length > 0)
                return true;
            return false;
        }

        public void Setup(string displayName = "", Boolean killProcessesWhenDone = true)
        {
            ClearDebug();
            // Zero out everything
            lastWindowHandle = IntPtr.Zero;
            processCounter = 0;
            bnetLauncherPID = 0;
            firstTokenTry = true;
            gameProcesses = new List<Process>();
            instances = new List<GameInstance>();

            // Initialize the processCounter so we know how many we're at.

            if (blizzardProcessesExists())
            {
                _ = MessageBox.Show("Close all D2R/Battle.net related programs first.");
                Setup(displayName, true);
                return;
            } 
            if(displayName.Length == 0)
            {
                displayName = Prompt.ShowDialog("Enter desired displayname for this account:", "Add Account");
            }
            ShowToolTip("Launching battle.net to set up " + displayName);
            var launcherProcess = LaunchLauncher();
            WinWait(Constants.bnetLauncherClass);
            SetBnetLauncherPID();
            WinWaitClose(Constants.bnetLauncherClass);
            processCounter++;
            LogDebug("Waiting for Game Client to start");
            ProcessWait(Constants.clientExecutableName);
            LogDebug("Client ready, glhf");
            CloseBnetLauncher(); // Kill the launcher, we don't need it anymore
            // Wait for the initial token update
            WaitForNewToken(gameProcesses[processCounter - 1]);
            // Then we do it once more, because it may update twice
            WaitForNewToken(gameProcesses[processCounter - 1], true); // Specify that we want a 30s timeout justincase
            // Export the received key using the name of the recipient
            ExportToken(displayName + ".bin");
            LogDebug("Successfully saved new token for " + displayName);
            CloseMultiProcessHandle(gameProcesses.Last());
            // Kill the Launcher & game client
            try
            {
                if (killProcessesWhenDone)
                {
                    CloseGameClient(gameProcesses[processCounter - 1].Id);
                }
            } catch (Exception ex)
            {
                // Not able to kill the game client, for whatever reason
                LogDebug(ex.ToString());
            }
            LoadAccounts();
        }
        private string ComspecGetOutput(string command,string arguments)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = command;
            p.StartInfo.Arguments = arguments;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }
        public void KillGameClientHandles()
        {
            var processes = Process.GetProcessesByName("D2R");
            if (processes.Length > 0)
            {
                foreach (var process in processes)
                {
                    LogDebug("Closing handle for: " + process.Id);
                    CloseMultiProcessHandle(process);
                }
            } else
            {
                LogDebug("No D2R processes found");
            }
        }
        public void ResetSessions()
        {
            CloseBnetLauncher();
            gameProcesses = new List<Process>();
            lastWindowHandle = new IntPtr(0);
            processCounter = 0;
            bnetLauncherPID = 0;
            firstTokenTry = true;
            instances = new List<GameInstance>();
    }
        public void LaunchWithAccount(string accountName)
        {
            LogDebug("Launching account ("+(instances.Count+1)+"): '" + accountName + "'");
            UseAccountToken(accountName);
            var process = LaunchGame(accountName);
            LogDebug("Process should be: " + process.Id);
            ProcessWait(Constants.clientExecutableName);
            // Wait for a new token
            WaitForNewToken(process);
            // Then wait again incase it changes in any forseeable future
            WaitForNewToken(process,true); // Specify that we do want the 30s timeout, just incase

            Process exists = Process.GetProcessById(process.Id);
            if (exists.Id == process.Id)
            {
                LogDebug("Process seems to be alive: " + process.Id);
                ExportToken(accountName + ".bin");
                CloseMultiProcessHandle(process);
                gameProcesses.Add(process);
            } else
            {
                LogDebug("Can't seem to find 'this' instance as a process?");
            }

        }
        private void CloseMultiProcessHandle(Process process)
        {
            var processName = '"' + process.ProcessName + ".exe" + '"';
            LogDebug("Closing multi-process handle(s) for " + processName);
            var handle64str = "handle64.exe";
            var handle64opts = "-a -p " + processName + " Instances";
            LogDebug("Running: " + handle64str + " " + handle64opts);
            var handleData = ComspecGetOutput(handle64str,handle64opts);
            LogDebug("Handle data: " + handleData);
            handleData.Replace('\r', ' ');
            var lines = handleData.Split('\n');
            string handle = "";
            string result = "";
            if(lines.Length > 0)
            {
                LogDebug("More than 0 lines, thats good");
                foreach(var line in lines) { 
                    if(line.Contains("D2R.exe"))
                    {
                        result = line;
                        LogDebug("Result: " + lines);
                    }
                }
            } else
            {
                LogDebug("Could not get handle data from handle64");
            }
            if(result.Length > 0)
            {
                var data = result.Split(new string[] { ": " },StringSplitOptions.None);
                if(data.Length > 0)
                {
                    LogDebug("Handle infos: " + data.ToString());
                    handle = data[2].Substring(data[2].Length - 8,8);
                    LogDebug("Handle: " + handle);
                }
                if(handle.Length > 0)
                {
                    LogDebug("Closing handle: " + handle);
                    LogDebug(ComspecGetOutput("handle64.exe", " -c " + handle + " -p \"" + process.Id + "\" -y"));
                }
            } else
            {
                LogDebug("No handle infos...");
            }
            LogDebug("All done, next!");
        }
        private Process LaunchGame(string accountName)
        {
            string installPath = (string)Registry.GetValue(Constants.gameInstallRegKey[0], Constants.gameInstallRegKey[1], "");
            var process =  Process.Start(installPath + "\\D2R.exe");
            var thisInstance = new GameInstance { account = accountName, process = process };
            instances.Add(thisInstance);
            return process;
        }
        public void LoadAccounts()
        {
            var ePath = Application.ExecutablePath;
            var path = System.IO.Path.GetDirectoryName(ePath);
            accountList.Items.Clear();
            var accounts = Directory.GetFiles(path, "*.bin");
            foreach(var account in accounts)
            {
                var lastWrite = File.GetLastWriteTime(account);
                var fileName = Path.GetFileNameWithoutExtension(account) + " | " + lastWrite.ToString();
                accountList.Items.Add(fileName);
            }
        }
        private void SetBnetLauncherPID()
        {
            GetWindowThreadProcessId(lastWindowHandle, out bnetLauncherPID);
        }
        private void CloseGameClient(int pid)
        {
            try
            {
                var target = Process.GetProcessById(pid);
                target.Kill();
                return;
            }
            catch (Exception ex)
            {
                return;
            }

        }
        private void CloseBnetLauncher()
        {
            try
            {
                var target = Process.GetProcessById((int)bnetLauncherPID);
                target.Kill();
                return;
            } catch (Exception ex)
            {
                return;
            }

        }
        private Process LaunchLauncher()
        {
            string installPath = (string)Registry.GetValue(Constants.gameInstallRegKey[0], Constants.gameInstallRegKey[1], "");
            var p = Process.Start(installPath + "\\Diablo II Resurrected Launcher.exe");
            lastWindowHandle = p.MainWindowHandle;
            return p;
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
            int maxWait = 2000;
            int waitTime = 0;
            while (!HasClass(windowHandle,windowClass) && waitTime < maxWait ) 
            {
                LogDebug("Could not find window of class: " + windowClass);
                Thread.Sleep(100);
                windowHandle = GetForegroundWindow();
                waitTime++;
            }
            if (waitTime >= maxWait)
                LogDebug("Giving up finding window");
            else
                lastWindowHandle = windowHandle;
            LogDebug("Found window, yeet");
        }
        private void WinWaitClose(string windowClass)
        {
            int maxWait = 2000;
            int waitTime = 0;
            while (HasClass(lastWindowHandle, windowClass) && waitTime < maxWait)
            {
                LogDebug("We still waiting");
                Thread.Sleep(100);
                waitTime++;
                if (waitTime >= maxWait)
                    LogDebug("Giving up waiting");
            }
            LogDebug("Finally its closed");
        }

        private void ProcessWait(string processName)
        {
            LogDebug("Waiting for process: " + processName);
            Process[] processes = Process.GetProcessesByName(processName);
            int maxWait = 2000;
            int waitTime = 0;
            while(processes.Length < processCounter && waitTime < maxWait)
            {
                LogDebug("Still waiting for Game Client: " + processName);
                Thread.Sleep(100);
                processes = Process.GetProcessesByName(processName);
                waitTime++;
                if (waitTime >= maxWait)
                    LogDebug("Giving up waiting for Game Client");
            }
            if (processes.Length == processCounter)
            {
                gameProcesses.Add(processes[processCounter-1]);
            }
            LogDebug("Finally the Game Client is up");
        }

        private void WaitForNewToken(Process process,Boolean timeout = false)
        {
            if (!IsProcessRunning(process))
                return;

            byte[] CurrentKey = new byte[20];

            CurrentKey = (byte[])Registry.GetValue(Constants.accountRegKey[0], Constants.accountRegKey[1], "");
            var PrevKey = CurrentKey; //  new byte[50];

//            if (firstTokenTry)
//            {
//                PrevKey = CurrentKey;
//                firstTokenTry = false;
//            } 
//            else
//                PrevKey = token;

            long startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long elapsedTime = 0;
            var maxWaitTime = 30000; // 15s.

            while (StructuralComparisons.StructuralEqualityComparer.Equals(CurrentKey, PrevKey) && (elapsedTime < maxWaitTime))
            {
                LogDebug("Waiting for Token to update ("+elapsedTime+"/"+maxWaitTime+")");
                if (!IsProcessRunning(process))
                {
                    LogDebug("Client exited, aborting..");
                    return;
                }
                CurrentKey = (byte[])Registry.GetValue(Constants.accountRegKey[0], Constants.accountRegKey[1], "");
                if(timeout == true)
                    elapsedTime = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - startTime;
            }
            if (!StructuralComparisons.StructuralEqualityComparer.Equals(CurrentKey, PrevKey))
            {
                LogDebug("Token updated");
                // token = CurrentKey;
            } else
            {
                LogDebug("Token remains the same");
            }
        }
        public void DumpCurrentRegKey()
        {
            ExportToken("dump.bin");
        }
        private void ExportToken(string fileName)
        {
            LogDebug("Writing to file: " + fileName);
            File.WriteAllBytes(fileName, (byte[])Registry.GetValue(Constants.accountRegKey[0], Constants.accountRegKey[1], ""));
        }
        private void UseAccountToken(string accountName)
        {
            LogDebug("Reading from file: " + accountName + ".bin");
            var data = File.ReadAllBytes(accountName + ".bin");
            Registry.SetValue(Constants.accountRegKey[0], Constants.accountRegKey[1], data, RegistryValueKind.Binary);
        }
        private Boolean IsProcessRunning(Process process)
        {
            try
            {
                var id = Process.GetProcessById(process.Id);
            } catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public void ClearDebug()
        {
            File.WriteAllText("debug.log", "\r\n");
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
