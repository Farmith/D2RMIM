using Microsoft.Win32;
using MultiInstanceManager.Helpers;
using MultiInstanceManager.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MultiInstanceManager.Modules
{
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
            gameExecutableName = Constants.clientExecutableName + Constants.executableFileExt;
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

            IntPtr bnetClientHandle = IntPtr.Zero;

            // Initialize the processCounter so we know how many we're at.

            if (ProcessManager.blizzardProcessesExists())
            {
                WindowHelper.ShowMessage("Close all D2R/Battle.net related programs first.");
                return Setup(displayName, cmdArgs, true);
            } 
            if(displayName.Length == 0)
            {
                displayName = Prompt.ShowDialog("Enter desired displayname for this account:", "Add Account");
            }
            Credentials bNetCredentials = new Credentials(null,null);
            if (saveCredentials)
            {
                Debug.WriteLine("Fixing credentials");
                bNetCredentials = CredentialHelper.GetVaultCredentials(displayName);
            }
            Debug.WriteLine("User: " + bNetCredentials.User);
            var launcherProcess = LaunchLauncher();
            WindowHelper.WinWait(Constants.bnetLauncherClass);
            SetBnetLauncherPID(launcherProcess);
            // Do some magic to enter the credentials
            if (bNetCredentials?.User?.Length > 0)
            {
                Thread.Sleep(5000); // Wait 5s to start filling in.
                Debug.WriteLine("Finding the login boxes");
                AutomationHelper.FillLauncherCredentials(launcherProcess, bNetCredentials.User, bNetCredentials.Pass);
                bnetClientHandle = WindowHelper.WinWait(Constants.bnetClientClass);
                launcherProcess = ProcessManager.GetProcessByHandle(bnetClientHandle);
                AutomationHelper.StartGameWithLauncherButton();
            }
            else
            {
                WindowHelper.WinWaitClose(Constants.bnetLauncherClass,launcherProcess.MainWindowHandle);
            }
            processCounter++;
            LogDebug("Waiting for Game Client to start");
            var clientExecutable = ProcessManager.ProcessWait(Constants.clientExecutableName,processCounter);
            LogDebug("Client ready, glhf");
            // Wait for the initial token update
            if (modifyWindowTitles)
            {
                WindowHelper.ModifyWindowTitleName(clientExecutable, displayName);
            }
            // Add a way to abort the frenetic clicking
            LogDebug("Clicking away");
            Debug.WriteLine("Clicking away");
            var freneticClickingCTS = new CancellationTokenSource();
            CancellationToken freneticClickingCT = freneticClickingCTS.Token;

            var task = Task.Factory.StartNew(() => AutomationHelper.ClickFreneticallyInsideWindow(freneticClickingCT, clientExecutable, 2),freneticClickingCTS.Token );
            WaitForNewToken(clientExecutable);
            // Then we do it once more, because it may update twice
            WaitForNewToken(clientExecutable, true); // Specify that we want a 30s timeout justincase
            // We are done clicking, for now
            try
            {
                freneticClickingCTS.Cancel();
            }
            catch (OperationCanceledException e)
            {
                Debug.WriteLine("Clicking canceled: " + e);
            }
            finally
            {
                freneticClickingCTS.Dispose();
            }
            // Export the received key using the name of the recipient
            ExportToken(displayName + ".bin");
            LogDebug("Successfully saved new token for " + displayName);

            LogDebug("Closing launcher and client mutex handles");
            CloseBnetLauncher(launcherProcess);
            LogDebug("Launcher closed, killing mutex");
            ProcessManager.CloseExternalHandles(clientExecutable.ProcessName); // Close all found D2R mutex handles
            LogDebug("Mutex killed");
            // CloseMultiProcessHandle(gameProcesses.Last());
            // Kill the Launcher & game client
            try
            {
                if (killProcessesWhenDone)
                {
                    CloseGameClient(clientExecutable.Id);
                }
            } catch (Exception ex)
            {
                // Not able to kill the game client, for whatever reason
                LogDebug(ex.ToString());
            }
            LogDebug("All done, exiting setup thread");
            return true;
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
            CloseBnetLaunchers();
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
            if(modifyWindowTitles)
            {
                WindowHelper.ModifyWindowTitleName(process, accountName);
            }
            // Add a way to abort the frenetic clicking
            LogDebug("Clicking away");
            Debug.WriteLine("Clicking away");
            var freneticClickingCTS = new CancellationTokenSource();
            CancellationToken freneticClickingCT = freneticClickingCTS.Token;

            var task = Task.Factory.StartNew(() => AutomationHelper.ClickFreneticallyInsideWindow(freneticClickingCT, process, 2), freneticClickingCTS.Token);
            // Wait for a new token
            WaitForNewToken(process);
            // Then wait again incase it changes in any forseeable future
            WaitForNewToken(process,true); // Specify that we do want the 30s timeout, just incase
            try
            {
                freneticClickingCTS.Cancel();
            }
            catch (OperationCanceledException e)
            {
                Debug.WriteLine("Clicking canceled: " + e);
            }
            finally
            {
                freneticClickingCTS.Dispose();
            }

            if (ProcessManager.MatchProcess(process))
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
        private void SetBnetLauncherPID(Process launcherProcess)
        {
            WindowHelper.GetWindowThreadProcessId(launcherProcess.MainWindowHandle, out bnetLauncherPID);
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
        private void CloseBnetLauncher(Process bnetLauncher)
        {
            try
            {
                var target = Process.GetProcessById(bnetLauncher.Id);
                if (target.Id > 0)
                {
                    Debug.WriteLine("Killing launcher");
                    target.Kill();
                } else
                {
                    Debug.WriteLine("Couldn't kill launcher?");
                }
                return;
            } catch (Exception ex)
            {
                Debug.WriteLine("Can't kill bnet launcher: " + ex.ToString());
                return;
            }
        }
        private void CloseBnetLaunchers()
        {
            try
            {
                var targets = Process.GetProcessesByName("Battle.net.exe");
                foreach (var target in targets)
                {
                    if (target.Id > 0)
                    {
                        target.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Can't kill bnet launcher: " + ex.ToString());
            }
        }
        private Process LaunchLauncher()
        {
            string installPath = (string)Registry.GetValue(Constants.gameInstallRegKey[0], Constants.gameInstallRegKey[1], "");
            var p = Process.Start(installPath + "\\Diablo II Resurrected Launcher.exe");
            lastWindowHandle = p.MainWindowHandle;
            return p;
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
