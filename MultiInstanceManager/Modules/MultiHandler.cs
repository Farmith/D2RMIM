using Microsoft.Win32;
using MultiInstanceManager.Helpers;
using MultiInstanceManager.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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
        Form parent;

        private IntPtr lastWindowHandle;
        private int processCounter;
        private List<Process> gameProcesses;
        private uint bnetLauncherPID;
        private CheckedListBox accountList;
        private List<GameInstance> instances;
        private bool modifyWindowTitles;
        private string gameExecutableName;
        private bool saveCredentials;

        public MultiHandler(Form _parent,CheckedListBox _accountList)
        {
            parent = _parent;
            accountList = _accountList;
            gameProcesses = new List<Process>();
            instances = new List<GameInstance>();
            modifyWindowTitles = false;
            gameExecutableName = "D2R.exe";
            saveCredentials = false;
        }
        public void SetCredentialMode(bool _saveCredentials = false)
        {
            saveCredentials = _saveCredentials;
        }
        public void SetGameExecutableName(string name)
        {
            gameExecutableName = name;
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

        public void ToggleWindowTitleMode(bool mode)
        {
            Debug.WriteLine("Changing window title toggle to: " + mode.ToString());
            modifyWindowTitles = mode;
        }

        public KeyToggle SwapFocus(KeyToggle binding)
        {
            var clientProcesses = Process.GetProcessesByName(Constants.clientExecutableName);
            Debug.WriteLine("Process count: " + clientProcesses.Count());

            // Set focus to specified window
            if (binding.WindowHandle == IntPtr.Zero)
            {
                if (instances.Count == 0 || instances.Count != clientProcesses.Count())
                {
                    instances = new List<GameInstance>(); // TODO: This is bad, now every time ONE game client disconnects, the whole thing is effed
                    for(var x = 0; x < clientProcesses.Count(); x++)
                    {
                        var i = new GameInstance();
                        i.process = clientProcesses[x];
                        i.account = "N/A";
                        instances.Add(i);
                    }
                }
                if (instances.Count >= binding.ClientIterator)
                {
                    Debug.WriteLine("Counts match for: " + binding.ClientIterator);
                    Debug.WriteLine("Process: " + instances[binding.ClientIterator].process.Id);
                    binding.WindowHandle = instances[binding.ClientIterator].process.MainWindowHandle;
                }
            }
            if (binding.WindowHandle != IntPtr.Zero)
            {
                try
                {
                    WindowHelper.forceForegroundWindow(binding.WindowHandle);
                } catch (Exception e)
                {
                    // The window has died or been removed or something
                    Debug.WriteLine("Could not force foreground window: " + e.ToString());
                    binding.WindowHandle = IntPtr.Zero;
                }
            }
            return binding;
        }
        public bool Setup(string displayName = "", string cmdArgs = "", Boolean killProcessesWhenDone = true)
        {
            ClearDebug();
            // Zero out everything
            lastWindowHandle = IntPtr.Zero;
            processCounter = 0;
            bnetLauncherPID = 0;
            gameProcesses = new List<Process>();
            instances = new List<GameInstance>();

            // Initialize the processCounter so we know how many we're at.

            if (blizzardProcessesExists())
            {
                _ = MessageBox.Show("Close all D2R/Battle.net related programs first.");
                return Setup(displayName, cmdArgs, true);
            } 
            if(displayName.Length == 0)
            {
                displayName = Prompt.ShowDialog("Enter desired displayname for this account:", "Add Account");
            }
            var bnetCredentials = Vault.GetCredential("D2RMIM-" + displayName);
            var username = "";
            var password = "";
            if(saveCredentials)
            {
                Debug.WriteLine("Disp: " + displayName + " User: " + bnetCredentials?.User + " Pass: <youwish>");
                if(bnetCredentials == null || (bnetCredentials?.User?.Length < 3 || bnetCredentials?.Pass?.Length < 3)) { 
                    // The user has input some crap into the credentials, just delete and ask again
                    Vault.RemoveCredentials("D2RMIM-" + displayName);
                    username = Prompt.ShowDialog("Enter your battle.net username: ", "Account Data Required");
                    password = Prompt.ShowDialog("Enter your battle.net password: ", "Account Data Required");
                    Vault.SetCredentials("D2RMIM-" + displayName, username, password); // Save credentials in windows credential store on local computer
                } else
                {
                    Debug.WriteLine("Using saved credentials");
                    username = bnetCredentials.User;
                    password = bnetCredentials.Pass;
                }
            }
            var launcherProcess = LaunchLauncher();
            WindowHelper.WinWait(Constants.bnetLauncherClass);
            SetBnetLauncherPID();
            // Do some magic to enter the credentials
            if (username.Length > 0)
            {
                Thread.Sleep(2000);
                Debug.WriteLine("Finding the login boxes");
                FillLauncherCredentials(launcherProcess, username, password);
                WindowHelper.WinWait(Constants.bnetClientClass);
                AutomationHelper.StartGameWithLauncherButton();
            }
            else
            {
                WindowHelper.WinWaitClose(Constants.bnetLauncherClass,launcherProcess.MainWindowHandle);
            }
            processCounter++;
            LogDebug("Waiting for Game Client to start");
            ProcessWait(Constants.clientExecutableName);
            LogDebug("Client ready, glhf");
            // Wait for the initial token update
            ModifyWindowTitleName(gameProcesses[processCounter - 1],displayName);
            WaitForNewToken(gameProcesses[processCounter - 1]);
            // Then we do it once more, because it may update twice
            WaitForNewToken(gameProcesses[processCounter - 1], true); // Specify that we want a 30s timeout justincase
            // Export the received key using the name of the recipient
            ExportToken(displayName + ".bin");
            LogDebug("Successfully saved new token for " + displayName);

            LogDebug("Closing launcher and client mutex handles");
            CloseBnetLauncher();
            LogDebug("Launcher closed, killing mutex");
            ProcessManager.CloseExternalHandles(Constants.clientExecutableName); // Close all found D2R mutex handles
            LogDebug("Mutex killed");
            // CloseMultiProcessHandle(gameProcesses.Last());
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
            LogDebug("All done, exiting setup thread");
            return true;
        }
        private void FillLauncherCredentials(Process launcher, string user, string pass)
        {
            var where = AutomationHelper.FindUsernameBox();
            if (where.X > 0 && where.Y > 0)
            {
                AutomationHelper.LeftMouseClick(where.X, where.Y+5);

                // We should be in the text-box now, we hope
                SendKeys.SendWait("^(a)");
                foreach (char c in user) {
                    SendKeys.SendWait(c.ToString());
                    Thread.Sleep(5);
                }
                Thread.Sleep(100);
                SendKeys.SendWait("{TAB}");
                Debug.WriteLine("Filling out password");
                Thread.Sleep(5);
                foreach (char c in pass)
                {
                    SendKeys.SendWait(c.ToString());
                    Thread.Sleep(5);
                }
                Thread.Sleep(100);
                Debug.WriteLine("Finding login button");
                var button = AutomationHelper.FindLoginButton(where.X, where.Y);
                while (button.X == 0)
                {
                    Thread.Sleep(100);
                    button = AutomationHelper.FindLoginButton(where.X, where.Y);
                }
                // Sleep a little, then tab to it
                Debug.WriteLine("Tabbing to button!");
                Thread.Sleep(100);
                SendKeys.SendWait("{TAB}");
                Thread.Sleep(15);
                SendKeys.SendWait("{TAB}");
                Thread.Sleep(5);
                SendKeys.SendWait("{ENTER}");
                // We should be logging in now!
                Debug.WriteLine("Should be logging in now");
            }
        }


        private void ModifyWindowTitleName(Process process, string displayName)
        {
            Debug.WriteLine("Modding if needed: " + modifyWindowTitles.ToString());
            if (modifyWindowTitles == true)
            {
                var newTitle = "[ " + displayName + " ] Diablo II: Resurrected";
                Debug.WriteLine("Changing window title to: " + newTitle);
                WindowHelper.SetWindowText(process.MainWindowHandle, newTitle);
            }
        }

        public void KillGameClientHandles()
        {
            var name = gameExecutableName.Substring(0, gameExecutableName.Length - 4);
            var processes = Process.GetProcessesByName(name);
            if (processes.Length > 0)
            {
                foreach (var process in processes)
                {
                    ProcessManager.CloseExternalHandles(process.ProcessName);
                }
            }
        }

        public void ResetSessions()
        {
            CloseBnetLauncher();
            gameProcesses = new List<Process>();
            lastWindowHandle = new IntPtr(0);
            processCounter = 0;
            bnetLauncherPID = 0;
            instances = new List<GameInstance>();
        }
        public bool LaunchWithAccount(string accountName,string cmdArgs = "")
        {
            LogDebug("Launching account ("+(instances.Count+1)+"): '" + accountName + "'");
            UseAccountToken(accountName);
            var process = LaunchGame(accountName, cmdArgs);
            LogDebug("Process should be: " + process.Id);
            Debug.WriteLine("Checking if we need to mod window titles");
            ModifyWindowTitleName(process, accountName);
            Debug.WriteLine("Modding done (if requested)");
            // Wait for a new token
            WaitForNewToken(process);
            // Then wait again incase it changes in any forseeable future
            WaitForNewToken(process,true); // Specify that we do want the 30s timeout, just incase

            Process exists = Process.GetProcessById(process.Id);
            if (exists.Id == process.Id)
            {
                LogDebug("Process seems to be alive: " + process.Id);
                ExportToken(accountName + ".bin");
                LogDebug("Closing mutex handles");
                ProcessManager.CloseExternalHandles(process.ProcessName); // kill all D2R mutex handles
                gameProcesses.Add(process);
            } else
            {
                LogDebug("Can't seem to find 'this' instance as a process?");
            }
            LogDebug("All done, exiting this thread");
            return true;

        }
       
        private Process LaunchGame(string accountName, string cmdArgs = "")
        {
            string installPath = (string)Registry.GetValue(Constants.gameInstallRegKey[0], Constants.gameInstallRegKey[1], "");
            var process = new Process();
            process.StartInfo.FileName = installPath + "\\" + gameExecutableName;
            process.StartInfo.Arguments = cmdArgs;
            process.Start();
            var thisInstance = new GameInstance { account = accountName, process = process };
            instances.Add(thisInstance);
            var name = gameExecutableName.Substring(0, gameExecutableName.Length - 4);
            var processes = Process.GetProcessesByName(name);
            var alive = false;
            // Sanity check to make sure the process is alive before we return it. trust me its required.
            while (!alive)
            {
                foreach(var p in processes)
                {
                    if(p.Id == process.Id && p.MainWindowHandle != IntPtr.Zero)
                    {
                        alive = true;
                    }
                }
                if(!alive)
                    processes = Process.GetProcessesByName(name);
            }
            return process;
        }
        private void _loadAccounts()
        {
            var ePath = Application.ExecutablePath;
            var path = Path.GetDirectoryName(ePath);
            accountList.Items.Clear();
            var accounts = Directory.GetFiles(path, "*.bin");
            foreach (var account in accounts)
            {
                var lastWrite = File.GetLastWriteTime(account);
                var fileName = Path.GetFileNameWithoutExtension(account) + " | " + lastWrite.ToString();
                accountList.Items.Add(fileName);
            }
        }
        public void LoadAccounts()
        {
            _loadAccounts();
        }
        private void SetBnetLauncherPID()
        {
            WindowHelper.GetWindowThreadProcessId(lastWindowHandle, out bnetLauncherPID);
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
                Debug.WriteLine(ex);
                return;
            }

        }
        private void CloseBnetLauncher()
        {
            try
            {
                var target = Process.GetProcessById((int)bnetLauncherPID);
                if (target.Id > 0)
                {
                    target.Kill();
                }
                return;
            } catch (Exception ex)
            {
                Debug.WriteLine("Can't kill bnet launcher: " + ex.ToString());
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

            long startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long elapsedTime = 0;
            var maxWaitTime = 60000; // 60s.

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
                Debug.WriteLine(ex);
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
}
