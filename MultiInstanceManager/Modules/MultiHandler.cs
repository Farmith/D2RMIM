using Microsoft.Win32;
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
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        
        [DllImport("user32.dll")]
        private static extern int AttachThreadInput(uint Attach, uint AttachTo, bool fAttach);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint BringWindowToTop(IntPtr hWnd);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool SetWindowText(IntPtr hWnd, string text);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
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
        private Rectangle bounds;
        private Color loginButtonColor = Color.FromArgb(255, 0, 116, 224);
        private Color usernameBoxColor = Color.FromArgb(255, 16, 17, 23);

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
        public bool PriorityWindowFocus()
        {
            var clients = Process.GetProcessesByName(Constants.clientExecutableName);
            var focusWindow = GetForegroundWindow();
            foreach(var client in clients)
            {
                if(focusWindow == client.MainWindowHandle)
                {
                    return true;
                }
            }
            if(focusWindow == Process.GetCurrentProcess().MainWindowHandle)
            {
                return true;
            }
            return false;
        }
        public void ToggleWindowTitleMode(bool mode)
        {
            Debug.WriteLine("Changing window title toggle to: " + mode.ToString());
            modifyWindowTitles = mode;
        }
        public void forceForegroundWindow(IntPtr hwnd)
        {
            uint processId = 0;
            uint windowThreadProcessId = GetWindowThreadProcessId(GetForegroundWindow(), out processId);
            uint currentThreadId = GetCurrentThreadId();
            int CONST_SW_SHOW = 5;
            AttachThreadInput(windowThreadProcessId, currentThreadId, true);
            BringWindowToTop(hwnd);
            ShowWindow(hwnd, CONST_SW_SHOW);
            AttachThreadInput(windowThreadProcessId, currentThreadId, false);
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
                    forceForegroundWindow(binding.WindowHandle);
                } catch (Exception e)
                {
                    // The window has died or been removed or something
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
            WinWait(Constants.bnetLauncherClass);
            SetBnetLauncherPID();
            // Do some magic to enter the credentials
            if (username.Length > 0)
            {
                Thread.Sleep(2000);
                Debug.WriteLine("Finding the login boxes");
                FillLauncherCredentials(launcherProcess, username, password);
                WinWait(Constants.bnetClientClass);
                StartGameWithLauncherButton();
            }
            else
            {
                WinWaitClose(Constants.bnetLauncherClass);
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
        private Bitmap GetScreenshot(Rect rect)
        {
            bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            // var CursorPosition = new Point(Cursor.Position.X - rect.Left, Cursor.Position.Y - rect.Top);
            var result = new Bitmap(bounds.Width, bounds.Height);
            using (var g = Graphics.FromImage(result))
            {
                g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return result;
        }
        private Point FindUsernameBox()
        {
            var foregroundWindowsHandle = GetForegroundWindow();
            var rect = new Rect();
            GetWindowRect(foregroundWindowsHandle, ref rect);

            var bitmap = GetScreenshot(rect);
            var delim = bitmap.Width / 3;
            var lineA = delim * 2;
            var lineMax = bitmap.Height / 2;
            var findColor = usernameBoxColor;
            SetCursorPos(rect.Left + lineA, rect.Top+0);
            for (var i = 0; i < lineMax; i++)
            {
                var color = bitmap.GetPixel(lineA, i);
                if(color == findColor)
                {
                    return new Point(rect.Left+lineA, rect.Top+i);
                }
            }

            return new Point(0, 0);
        }
        private Point FindLoginButton(int xline,int yline)
        {
            var foregroundWindowsHandle = GetForegroundWindow();
            var rect = new Rect();
            GetWindowRect(foregroundWindowsHandle, ref rect);

            var bitmap = GetScreenshot(rect);
            var delim = bitmap.Width / 3;
            var lineA = delim * 2;
            var lineMax = bitmap.Height;
            var findColor = loginButtonColor;
            SetCursorPos(rect.Left+lineA, rect.Top+0);
            for (var i = 0; i < lineMax; i++)
            {
                var color = bitmap.GetPixel(lineA, i);
                if (color == findColor)
                {
                    Debug.WriteLine("Found the color!");
                    return new Point(rect.Left + lineA, rect.Top + i);
                }
                Debug.WriteLine("Color: " + color.R + ":" + color.G + ":" + color.B);
            }

            return new Point(0, 0);
        }
        private Point FindLauncherButton()
        {
            var foregroundWindowsHandle = GetForegroundWindow();
            var rect = new Rect();
            GetWindowRect(foregroundWindowsHandle, ref rect);

            var bitmap = GetScreenshot(rect);
            var delim = bitmap.Width / 3;
            var lineA = 150;
            var lineMax = bitmap.Height -1;
            var findColor = loginButtonColor;
            SetCursorPos(rect.Left + lineA, rect.Top + 0);
            for (var i = lineMax; i > 0; i--)
            {
                var color = bitmap.GetPixel(lineA, i);
                if (color == findColor)
                {
                    Debug.WriteLine("Found the color!");
                    return new Point(rect.Left + lineA, rect.Top + i);
                }
                Debug.WriteLine("Color: " + color.R + ":" + color.G + ":" + color.B);
            }

            return new Point(0, 0);
        }
        private void FillLauncherCredentials(Process launcher, string user, string pass)
        {
            var where = FindUsernameBox();
            if (where.X > 0 && where.Y > 0)
            {
                LeftMouseClick(where.X, where.Y+5);

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
                var button = FindLoginButton(where.X, where.Y);
                while (button.X == 0)
                {
                    Thread.Sleep(100);
                    button = FindLoginButton(where.X, where.Y);
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
        private void StartGameWithLauncherButton()
        {
            Thread.Sleep(100);
            var button = FindLauncherButton();
            while (button.X == 0)
            {
                Thread.Sleep(100);
                button = FindLauncherButton();
            }
            if (button.X > 0 && button.Y > 0)
            {
                LeftMouseClick(button.X, button.Y - 10);
            }
        }
        public void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }
        private void ModifyWindowTitleName(Process process, string displayName)
        {
            Debug.WriteLine("Modding if needed: " + modifyWindowTitles.ToString());
            if (modifyWindowTitles == true)
            {
                var newTitle = "[ " + displayName + " ] Diablo II: Resurrected";
                Debug.WriteLine("Changing window title to: " + newTitle);
                SetWindowText(process.MainWindowHandle, newTitle);
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
                    LogDebug("Closing handle for: " + process.Id);
                    ProcessManager.CloseExternalHandles(process.ProcessName);
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
