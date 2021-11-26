#nullable enable
#pragma warning disable CA1416 // Validate platform compatibility

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
    public class MultiHandler
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
        private List<Profile> profileStore;
        public static List<ActiveWindow>? activeWindows;
        private CancellationTokenSource? processMonitorCTS;
        private CancellationTokenSource? audioMonitorCTS;
        private Task? processMonitorTask;
        private Task? audioMonitorTask;
        public MultiHandler(Form _parent, CheckedListBox _accountList)
        {
            parent = _parent;
            accountList = _accountList;
            gameProcesses = new List<Process>();
            instances = new List<GameInstance>();
            modifyWindowTitles = false;
            gameExecutableName = Constants.clientExecutableName + Constants.executableFileExt;
            saveCredentials = false;
            profileStore = new List<Profile>();
            activeWindows = new List<ActiveWindow>();
        }
        public ActiveWindow? GetActiveWindow(string displayname)
        {
            if (activeWindows != null)
            {
                foreach (var window in activeWindows)
                {
                    if (window.Profile.DisplayName == displayname)
                        return window;
                }
            }
            return null;
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

        public ActiveWindow SwapFocus(Profile account)
        {
            var activeWindow = activeWindows?.Find(x => x.Profile.DisplayName == account.DisplayName);
            if (activeWindow == null)
            {
                Log.Debug("Active window is null for: " + account.DisplayName);
                return new ActiveWindow();
            }

            var clientProcesses = Process.GetProcessesByName(activeWindow.Profile.GameExecutable);
            Debug.WriteLine("Game Process count: " + clientProcesses.Count());

            // Set focus to specified window
            /*
            if (activeWindow.Process.MainWindowHandle == IntPtr.Zero)
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
            */

            if (activeWindow.Process.MainWindowHandle != IntPtr.Zero)
            {
                Log.Debug("We found a handle to this game client..");
                try
                {
                    WindowHelper.forceForegroundWindow(activeWindow.Process.MainWindowHandle);
                }
                catch (Exception e)
                {
                    // The window has died or been removed or something
                    Debug.WriteLine("Could not force foreground window: " + e.ToString());
                    activeWindows?.Remove(activeWindow);
                }
            }
            return activeWindow;
        }
        public bool Setup(string displayName = "", Boolean killProcessesWhenDone = true)
        {
            Log.Clear();
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
                return Setup(displayName, true);
            }
            if (displayName.Length == 0)
            {
                displayName = Prompt.ShowDialog("Enter desired displayname for this account:", "Add Account");
            }
            Credentials? bNetCredentials = new Credentials(null, null);
            if (saveCredentials)
            {
                Debug.WriteLine("Fixing credentials");
                bNetCredentials = CredentialHelper.GetVaultCredentials(displayName);
                if (bNetCredentials != null)
                    Debug.WriteLine("User: " + bNetCredentials.User);
            }
            // Set the region
            Profile? profile = FileHelper.LoadProfileConfiguration(displayName);

            if (profile?.Region?.Length > 0)
            {
                switch (profile.Region)
                {
                    case "Europe":
                        ChangeRealm("EU");
                        break;
                    case "Americas":
                        ChangeRealm("US");
                        break;
                    case "Asia":
                        ChangeRealm("KR");
                        break;
                    default:
                        break;
                }
            }
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
                WindowHelper.WinWaitClose(Constants.bnetLauncherClass, launcherProcess.MainWindowHandle);
            }
            processCounter++;
            Log.Debug("Waiting for Game Client to start");
            var clientExecutable = ProcessManager.ProcessWait(Constants.clientExecutableName, processCounter);
            if (clientExecutable == null)
                return false;
            Log.Debug("Client ready, glhf");

            // Wait for the initial token update
            if (modifyWindowTitles)
            {
                WindowHelper.ModifyWindowTitleName(clientExecutable, displayName);
                clientExecutable.Refresh();
                // This should only be done if the user explicitly asks for it:
                Log.Debug("Handle: " + clientExecutable.MainWindowHandle.ToString());
                WindowHelper.SetWindowApplicationId(clientExecutable.MainWindowHandle, "D2R" + displayName);
            }
            // Add a way to abort the frenetic clicking
            Log.Debug("Clicking away");
            Debug.WriteLine("Clicking away");
            var freneticClickingCTS = new CancellationTokenSource();
            CancellationToken freneticClickingCT = freneticClickingCTS.Token;
            var task = Task.Factory.StartNew(() => AutomationHelper.SpaceMan(freneticClickingCT, clientExecutable, 2), freneticClickingCTS.Token);
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
            Log.Debug("Successfully saved new token for " + displayName);

            Log.Debug("Closing launcher and client mutex handles");
            if (launcherProcess != null)
                CloseBnetLauncher(launcherProcess);
            Log.Debug("Launcher closed, killing mutex");
            // ProcessManager.CloseExternalHandles(clientExecutable.ProcessName); // Don't need to close mutexes when setting up anymore
            // Log.Debug("Mutex killed");
            // CloseMultiProcessHandle(gameProcesses.Last());
            // Kill the Launcher & game client
            try
            {
                if (killProcessesWhenDone)
                {
                    CloseGameClient(clientExecutable.Id);
                }
            }
            catch (Exception ex)
            {
                // Not able to kill the game client, for whatever reason
                Log.Debug(ex.ToString());
            }
            Log.Debug("All done, exiting setup thread");
            return true;
        }
        public List<Profile> GetAllProfiles()
        {
            return profileStore;
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
        public async Task LaunchWithAccount(string accountName, string cmdArgs = "")
        {
            Log.Debug("Launching account (" + (instances.Count + 1) + "): '" + accountName + "'");
            Profile? profile = FileHelper.LoadProfileConfiguration(accountName);

            if (profile?.Region?.Length > 0)
            {
                switch (profile.Region)
                {
                    case "Europe":
                        ChangeRealm("EU");
                        break;
                    case "Americas":
                        ChangeRealm("US");
                        break;
                    case "Asia":
                        ChangeRealm("KR");
                        break;
                    default:
                        break;
                }
            }
            UseAccountToken(accountName);
            cmdArgs = profile?.LaunchOptions.LaunchArguments.Length > 0 ? profile.LaunchOptions.LaunchArguments : "";
            // Check if we need to do some magic with the Settings.json
            if (profile != null && profile.SeparateJsonSettings)
            {
                FileHelper.ReplaceJSONSettingsFile(profile.DisplayName);
            }
            string? preLaunchCmds = null;
            if (profile != null && profile.LaunchOptions.PreLaunchCommands.Length > 0)
            {
                preLaunchCmds = profile.LaunchOptions.PreLaunchCommands;
                Log.Debug("Pre-launch command: " + preLaunchCmds);
            }

            if (preLaunchCmds != null)
            {
                // ProcessManager.CreateProcessAsUser(preLaunchCmds,String.Empty);
                Log.Debug("Attempting to start: " + preLaunchCmds);
                var preProcess = ProcessManager.DeElevatedProcess(preLaunchCmds);
                // ProcessManager.StartProcess(preLaunchCmds);
            }
            string? installPath = null;
            if (profile != null && profile.InstallationPath.Length > 0)
            {
                installPath = profile.InstallationPath;
            }
            string? gameExe = null;
            if (profile != null && profile.GameExecutable.Length > 0)
            {
                gameExe = profile.GameExecutable;
            }
            Log.Debug("Launching game with: " + installPath + gameExe + Constants.executableFileExt);
            var process = LaunchGame(accountName, cmdArgs, installPath, gameExe);
            Log.Debug("Process should be: " + process.Id);

            // Start the handle killer early
            // ProcessManager.CloseExternalHandles(process.ProcessName); // kill all D2R mutex handles
            var handleKillerTask = Task.Factory.StartNew(() => ProcessManager.CloseExternalHandles(process.ProcessName));

            if (profile != null && profile.ModifyWindowtitles)
            {
                WindowHelper.ModifyWindowTitleName(process, accountName);
            }
            if (profile != null && profile.SeparateTaskbarIcons)
            {
                Log.Debug("Handle: " + process.MainWindowHandle.ToString());
                WindowHelper.SetWindowApplicationId(process.MainWindowHandle, "D2R" + accountName);
            }
            if (profile != null && (profile.LaunchOptions.WindowX >= 0 || profile.LaunchOptions.WindowY >= 0))
            {
                WindowHelper.SetWindowPosition(process.MainWindowHandle, profile.LaunchOptions.WindowX, profile.LaunchOptions.WindowY);
            }
            // Add a way to abort the frenetic clicking
            Log.Debug("Clicking away");
            Debug.WriteLine("Clicking away");
            var freneticClickingCTS = new CancellationTokenSource();
            CancellationToken freneticClickingCT = freneticClickingCTS.Token;

            // var task = Task.Factory.StartNew(() => AutomationHelper.ClickFreneticallyInsideWindow(freneticClickingCT, process, 2), freneticClickingCTS.Token);
            var task = Task.Factory.StartNew(() => AutomationHelper.SpaceMan(freneticClickingCT, process, 2), freneticClickingCTS.Token);
            // Wait for a new token
            WaitForNewToken(process);
            // Then wait again incase it changes in any forseeable future
            WaitForNewToken(process, true); // Specify that we do want the 30s timeout, just incase
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
                Log.Debug("Process seems to be alive: " + process.Id);
                if (profile != null && (profile.LaunchOptions.WindowX != 0 || profile.LaunchOptions.WindowY != 0))
                {
                    WindowHelper.SetWindowPosition(process.MainWindowHandle, profile.LaunchOptions.WindowX, profile.LaunchOptions.WindowY);
                }
                ExportToken(accountName + ".bin");
                // Log.Debug("Closing mutex handles");
                activeWindows?.Add(new ActiveWindow { Process = process, Profile = profile });
                // ProcessManager.CloseExternalHandles(process.ProcessName); // kill all D2R mutex handles
                // Thread.Sleep(500); // Small delay added
                var handleKillerResult = await handleKillerTask; // Wait for the handlekiller to finish at this point, it should already be done.
                gameProcesses.Add(process);
            }
            else
            {
                Log.Debug("Can't seem to find 'this' instance as a process?");
            }
            /*
             * Launch (any) post launch commands saved for this profile
             * 
             */
            string? postLaunchCmds = null;
            if (profile != null && profile.LaunchOptions.PostLaunchCommands.Length > 0)
            {
                postLaunchCmds = profile.LaunchOptions.PostLaunchCommands;
            }
            if (postLaunchCmds != null)
            {
                var postProcess = ProcessManager.DeElevatedProcess(postLaunchCmds);
            }
            Log.Debug("All done, exiting this thread");
        }
        private Process LaunchGame(string profile, string cmdArgs = "", string ? _installPath = null, string? _exeName = null)
        {
            _exeName = _exeName != null ? _exeName + Constants.executableFileExt : gameExecutableName;
            _installPath = _installPath ?? Registry.GetValue(Constants.gameInstallRegKey[0], Constants.gameInstallRegKey[1], "") as string;
            var forcedArgs = " -uid osi -launcher";
            var process = ProcessManager.DeElevatedProcess(_installPath + "\\" + _exeName + " " + cmdArgs + forcedArgs);

            Log.Debug("Game should have started by this point.");
            var thisInstance = new GameInstance { account = profile, process = process };
            instances.Add(thisInstance);
            var name = _exeName.Substring(0, _exeName.Length - 4);
            var processes = Process.GetProcessesByName(name);
            var alive = false;
            // Sanity check to make sure the process is alive before we return it. trust me its required.
            while (!alive)
            {
                foreach (var p in processes)
                {
                    if (p.Id == process.Id && p.MainWindowHandle != IntPtr.Zero)
                    {
                        alive = true;
                    }
                }
                if (!alive)
                    processes = Process.GetProcessesByName(name);
            }
            return process;
        }
        private Process LaunchGameOld(string profile, string cmdArgs = "", string? _installPath = null, string? _exeName = null)
        {
            var process = new Process();
            _exeName = _exeName != null ? _exeName + Constants.executableFileExt : gameExecutableName;
            _installPath = _installPath ?? Registry.GetValue(Constants.gameInstallRegKey[0], Constants.gameInstallRegKey[1], "") as string;

            process.StartInfo.FileName = _installPath + "\\" + _exeName;
            process.StartInfo.Arguments = cmdArgs;
            process.Start();
            Log.Debug("Game should have started by this point.");
            var thisInstance = new GameInstance { account = profile, process = process };
            instances.Add(thisInstance);
            var name = _exeName.Substring(0, _exeName.Length - 4);
            var processes = Process.GetProcessesByName(name);
            var alive = false;
            // Sanity check to make sure the process is alive before we return it. trust me its required.
            while (!alive)
            {
                foreach (var p in processes)
                {
                    if (p.Id == process.Id && p.MainWindowHandle != IntPtr.Zero)
                    {
                        alive = true;
                    }
                }
                if (!alive)
                    processes = Process.GetProcessesByName(name);
            }
            return process;
        }
        public void LoadProfiles()
        {
            accountList.Items.Clear();
            var profiles = FileHelper.GetProfilesByFolder();
            profileStore.Clear();
            foreach (var profile in profiles)
            {
                var fileName = profile.AccountName + " | " + profile.LastWriteTime.ToString();
                Profile? a = FileHelper.LoadProfileConfiguration(profile.AccountName);
                if (a != null)
                    profileStore.Add(a);
                accountList.Items.Add(fileName);
            }
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
                }
                else
                {
                    Debug.WriteLine("Couldn't kill launcher?");
                }
                return;
            }
            catch (Exception ex)
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
            string? installPath = Registry.GetValue(Constants.gameInstallRegKey[0], Constants.gameInstallRegKey[1], "") as string;
            if (installPath == null)
            {
                Log.Debug("Installation registry is broken, can not continue.");
                throw (new Exception("D2R Installation broken, can not find registry value for installation path"));
            }
            else
            {
                var p = Process.Start(installPath + "\\Diablo II Resurrected Launcher.exe");
                lastWindowHandle = p.MainWindowHandle;
                return p;
            }
        }
        private void ChangeRealm(string realm) => Registry.SetValue(Constants.clientRegionKey[0], Constants.clientRegionKey[1], realm, RegistryValueKind.String);

        private void WaitForNewToken(Process process, Boolean timeout = false)
        {
            if (!ProcessManager.IsProcessRunning(process))
            {
                Log.Debug("Process has died or exited.");
                return;
            }

            byte[]? CurrentKey = new byte[20];

            CurrentKey = Registry.GetValue(Constants.accountRegKey[0], Constants.accountRegKey[1], "") as byte[];
            if(CurrentKey == null)
            {
                // Something is severely messed up here, can't get the values from registry.
                Log.Debug("Unable to get WEB-TOKEN from registry, registry corrupt?");
                throw (new Exception("Unable to get WEB-TOKEN from registry"));
            }
            var PrevKey = CurrentKey; //  new byte[50];

            long startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long elapsedTime = 0;
            var maxWaitTime = 60000; // 60s.

            while (StructuralComparisons.StructuralEqualityComparer.Equals(CurrentKey, PrevKey) && (elapsedTime < maxWaitTime))
            {
                Log.Debug("Waiting for Token to update (" + elapsedTime + "/" + maxWaitTime + ")");
                if (!ProcessManager.IsProcessRunning(process))
                {
                    Log.Debug("Client exited, aborting..");
                    return;
                }
                CurrentKey = Registry.GetValue(Constants.accountRegKey[0], Constants.accountRegKey[1], "") as byte[];
                if (timeout == true)
                    elapsedTime = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - startTime;
            }
            if (!StructuralComparisons.StructuralEqualityComparer.Equals(CurrentKey, PrevKey))
            {
                Log.Debug("Token updated");
                // token = CurrentKey;
            }
            else
            {
                Log.Debug("Token remains the same");
            }
        }
        public void DumpCurrentRegKey()
        {
            ExportToken("dump.bin");
        }
        private void ExportToken(string fileName)
        {
            Log.Debug("Writing to file: " + fileName);
            byte[]? registryBytes = Registry.GetValue(Constants.accountRegKey[0], Constants.accountRegKey[1], "") as byte[];
            if(registryBytes != null) 
                File.WriteAllBytes(fileName, registryBytes);
        }
        private void UseAccountToken(string accountName)
        {
            Log.Debug("Reading from file: " + accountName + ".bin");
            var data = File.ReadAllBytes(accountName + ".bin");
            Registry.SetValue(Constants.accountRegKey[0], Constants.accountRegKey[1], data, RegistryValueKind.Binary);
        }

        public void StartProcessMonitor()
        {
            processMonitorCTS = new CancellationTokenSource();
            CancellationToken processMonitorCT = processMonitorCTS.Token;
            processMonitorTask = Task.Factory.StartNew(() => ProcessMonitor(processMonitorCT), processMonitorCTS.Token);
        }
        public void StartAudioMonitor()
        {
            audioMonitorCTS = new CancellationTokenSource();
            CancellationToken audioMonitorCT = audioMonitorCTS.Token;
            audioMonitorTask = Task.Factory.StartNew(() => AudioMonitor(audioMonitorCT), audioMonitorCTS.Token);
        }
        public void StopProcessMonitor()
        {
            try
            {
                processMonitorCTS?.Cancel();
            }
            catch (OperationCanceledException e)
            {
                Debug.WriteLine("Stopped ProcessMonitor: " + e);
            }
            finally
            {
                processMonitorCTS?.Dispose();
            }
        }
        public void StopAudioMonitor()
        {
            try
            {
                audioMonitorCTS?.Cancel();
            }
            catch (OperationCanceledException e)
            {
                Debug.WriteLine("Stopped AudioMonitor: " + e);
            }
            finally
            {
                audioMonitorCTS?.Dispose();
            }
        }
        public void ProcessMonitor(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            var keepGoing = true;
            while (keepGoing)
            {
                if (ct.IsCancellationRequested)
                {
                    keepGoing = false;
                }
                else
                {
                    // Iterate all windows we SHOULD have:
                    try
                    {
                        if (activeWindows != null)
                        {
                            foreach (var activeWindow in activeWindows)
                            {
                                if (!ProcessManager.IsProcessRunning(activeWindow.Process))
                                {
                                    Log.Debug("Window for profile: " + activeWindow.Profile.DisplayName + " has exited");
                                    // It has died, so we need to ask the user if we should restart it
                                    var confirmed = Prompt.ConfirmDialog("Restarting client in {timeout} seconds", "Client exited");
                                    if (confirmed)
                                    {
                                        // Restart the client
                                        Log.Debug("Restarting client: " + activeWindow.Profile.DisplayName);
                                        var profileName = activeWindow.Profile.DisplayName;
                                        activeWindows.Remove(activeWindow);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed, on purpose
                                        LaunchWithAccount(profileName);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed, on purpose
                                    }
                                    else
                                    {
                                        Log.Debug("Profile: " + activeWindow.Profile.DisplayName + " exited, not restarting due to user request.");
                                        activeWindows.Remove(activeWindow);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Debug("Iterating active windows failed: " + e.ToString());
                    }
                }
                Thread.Sleep(1000);
            }
        }
        public void AudioMonitor(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            var keepGoing = true;
            while (keepGoing)
            {
                if (ct.IsCancellationRequested)
                {
                    keepGoing = false;
                }
                else
                {
                    try
                    {
                        if (activeWindows != null)
                        {
                            foreach (var activeWindow in activeWindows)
                            {
                                if (activeWindow == null || activeWindow.Profile.MuteWhenMinimized == false)
                                    continue;

                                if (activeWindow.VolumeControl == null)
                                {
                                    Log.Debug("Fetching volume control for: " + activeWindow.Profile.DisplayName);
                                    activeWindow.VolumeControl = AudioHelper.GetWindowAudioControl(activeWindow.Process);
                                }
                                if (WindowHelper.IsMinimized(activeWindow.Process))
                                {
                                    try
                                    {
                                        bool isMuted;
                                        Guid bs = Guid.Empty;
                                        activeWindow.VolumeControl.GetMute(out isMuted);
                                        if (!isMuted)
                                        {
                                            Log.Debug("Muting window for: " + activeWindow.Profile.DisplayName);
                                            activeWindow.VolumeControl.SetMute(true, bs);
                                        }
                                    }
                                    catch (Exception me)
                                    {
                                        Log.Debug("Could not mute window for: " + activeWindow.Profile.DisplayName);
                                        Log.Debug(me.ToString());
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        bool isMuted;
                                        Guid bs = Guid.Empty;
                                        activeWindow.VolumeControl.GetMute(out isMuted);
                                        if (isMuted)
                                        {
                                            Log.Debug("UnMuting window for: " + activeWindow.Profile.DisplayName);
                                            activeWindow.VolumeControl.SetMute(false, bs);
                                        }
                                    }
                                    catch (Exception me)
                                    {
                                        Log.Debug("Could not UnMute window for: " + activeWindow.Profile.DisplayName);
                                        Log.Debug(me.ToString());
                                    }
                                }
                                Thread.Sleep(50); // Small delay here too
                            }
                        }
                    } catch (Exception e)
                    {
                        Log.Debug("Ammount of windows changed: " + e.ToString());
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}
