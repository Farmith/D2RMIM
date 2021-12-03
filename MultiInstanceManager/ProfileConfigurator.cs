#pragma warning disable CA1416 // Validate platform compatibility

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using MultiInstanceManager.Helpers;
using MultiInstanceManager.Modules;
using MultiInstanceManager.Structs;

namespace MultiInstanceManager
{
    public partial class AccountConfiguration : Form
    {
        private List<AccountBinary>? Profiles;
        private bool hotKeyPressRegistered = false;
        private string hotKeyPressString = "";
        private Keys? modifier = null;
        private Keys? hotkey = null;
        private HotKey? _HotKey;
        private Profile? _Profile;
        private MultiHandler? MH;

        public AccountConfiguration()
        {
            InitializeComponent();
            selectAccount.SelectedIndexChanged += new EventHandler(selectAccount_SelectedIndexChanged);

            // These below may not be neccessary because: Save Button
            useDefaultGame.CheckedChanged += new EventHandler(useDefaultGame_CheckedChanged);
            skipIntroVideos.CheckedChanged += new EventHandler(skipIntroVideos_CheckedChanged);
            // this.OnShown += AccountConfiguration_Shown;
            hotKeyKey.KeyDown += hotKeyKey_KeyDown;
            hotKeyKey.KeyPress += hotKeyKey_KeyPress;
            hotKeyKey.KeyUp += hotKeyKey_KeyUp;

            saveConfig.Click += SaveConfiguration;
            browseForInstallationButton.Click += browseForInstallationButton_Click;
            grabWindowXYButton.Click += grabWindowXYButton_Click;
            grabXYTooltip.SetToolTip(grabWindowXYButton, "Grabs the current x/y coordinates of this profiles game window\r\nrequires that the client is started.");
        }
        public void SetMultiHandler(MultiHandler handler)
        {
            MH = handler;
        }
        public void OnShown(object? sender, EventArgs e)
        {
            Log.Debug("Resetting innards of config");
            resetRegion();
            FillAccounts();
            DefaultSettings();
        }
        private void DefaultSettings()
        {
            currentHotKey.Text = "";
            skipIntroVideos.Checked = false;
            modifyWindowTitles.Checked = false;
            useDefaultGame.Checked = false;
            enableHotkeys.Checked = false;
            separateTaskbarItems.Checked = false;
            gameExecutableName.Text = "";
            installationPath.Text = "";
            preLaunchCmd.Text = "";
            postLaunchCmd.Text = "";
            gameLaunchArgs.Text = "";
            windowXposition.Text = "";
            windowYposition.Text = "";
            resetRegion();
        }
        private void LoadProfile()
        {
            DefaultSettings();
            if (selectAccount.SelectedItem == null)
                return;
            var selection = selectAccount.SelectedItem.ToString();
            if (selection == null)
                return;
            _Profile = FileHelper.LoadProfileConfiguration(selection);
            if (_Profile != null)
            {
                modifyWindowTitles.Checked = _Profile.ModifyWindowtitles;
                skipIntroVideos.Checked = _Profile.SkipCinematics;
                if (_Profile.WindowHotKey != null)
                {
                    _HotKey = _Profile.WindowHotKey;
                    modifier = _HotKey.ModifierKey;
                    hotkey = _HotKey.Key;
                    enableHotkeys.Checked = _Profile.WindowHotKey.Enabled;
                    var extraMod = "";
                    if (_Profile.WindowHotKey.ModifierKey?.ToString().Length > 0)
                    {
                        extraMod = _Profile.WindowHotKey.ModifierKey.ToString() + "+";
                    }
                    currentHotKey.Text = extraMod + "[" + _Profile.WindowHotKey.Key.ToString() + "]";
                }
                gameExecutableName.Text = _Profile.GameExecutable;
                installationPath.Text = _Profile.InstallationPath;
                useDefaultGame.Checked = _Profile.UseDefaultGameInstallation;
                separateTaskbarItems.Checked = _Profile.SeparateTaskbarIcons;
                windowXposition.Text = _Profile.LaunchOptions?.WindowX?.ToString();
                windowYposition.Text = _Profile.LaunchOptions?.WindowY?.ToString();
                gameLaunchArgs.Text = _Profile.LaunchOptions?.LaunchArguments;
                preLaunchCmd.Text = _Profile.LaunchOptions?.PreLaunchCommands;
                postLaunchCmd.Text = _Profile.LaunchOptions?.PostLaunchCommands;
                separateJsonSettings.Checked = _Profile.SeparateJsonSettings;
                separateTaskbarItems.Checked = _Profile.SeparateTaskbarIcons;
                muteWhenMinimized.Checked = _Profile.MuteWhenMinimized;
                if(_Profile.SeparateJsonSettings)
                {
                    // Make sure there is a JSON file to use for Settings.
                    FileHelper.CreateJSONSettings(_Profile?.DisplayName);
                }
                SelecteRegion(_Profile?.Region);
            } else
            {
                Log.Debug("Previous config is null");
            }
        }
        private void SelecteRegion(string? region)
        {
            if (region == null || selectedRegion == null || selectedRegion.Items.Count < 1)
                return;
            for(int i = 0; i < selectedRegion.Items.Count; i++)
            {
                if(selectedRegion.Items[i].ToString()?.CompareTo(region) == 0)
                {
                    selectedRegion.SelectedIndex = i;
                    return;
                }
            }
            selectedRegion.SelectedIndex = -1;
        }
        private void FillAccounts()
        {
            selectAccount.Items.Clear();
            Profiles = FileHelper.GetProfilesByFolder();
            foreach(var profile in Profiles)
            {
                selectAccount.Items.Add(profile.AccountName);
            }
        }
        private void SaveConfiguration(object? sender, EventArgs e)
        {
            Profile config = new Profile();

            config.DisplayName = selectAccount.SelectedItem.ToString();
            if (!useDefaultGame.Checked) {
                config.InstallationPath = installationPath.Text;
                config.GameExecutable = gameExecutableName.Text;
            } 
            else
            {
                try
                {
                    config.InstallationPath = (string?)Registry.GetValue(Constants.gameInstallRegKey[0], Constants.gameInstallRegKey[1], "");
                } catch (Exception ex)
                {
                    Log.Debug("Can not find installation in registry: " + ex.ToString());
                    config.InstallationPath = "";
                }
                config.GameExecutable = Constants.clientExecutableName;
            }
            LaunchSettings launchSettings = new LaunchSettings();
            launchSettings.PreLaunchCommands = preLaunchCmd.Text;
            launchSettings.PostLaunchCommands = postLaunchCmd.Text;
            launchSettings.LaunchArguments = gameLaunchArgs.Text;

            if(selectedRegion.SelectedIndex > 0)
                config.Region = selectedRegion.SelectedItem.ToString();

            if(int.TryParse(windowXposition.Text,out int x))
            {
                launchSettings.WindowX = x;
            } else
            {
                launchSettings.WindowX = null;
            }
            if (int.TryParse(windowYposition.Text, out int y))
            {
                launchSettings.WindowY = y;
            } else
            {
                launchSettings.WindowY = null;
            }
            config.LaunchOptions = launchSettings;

            config.SkipCinematics = skipIntroVideos.Checked;
            config.ModifyWindowtitles = modifyWindowTitles.Checked;
            config.WindowHotKey = _HotKey;
            config.SeparateJsonSettings = separateJsonSettings.Checked;
            config.SeparateTaskbarIcons = separateTaskbarItems.Checked;
            config.MuteWhenMinimized = muteWhenMinimized.Checked;
            if (config.SeparateJsonSettings)
            {
                // Make sure there is a JSON file to use for Settings.
                FileHelper.CreateJSONSettings(config.DisplayName);
            }
            FileHelper.SaveAccountConfiguration(config);

        }
        private void resetRegion()
        {
            selectedRegion.Items.Clear();
            selectedRegion.Items.Add("");
            selectedRegion.Items.Add("Europe");
            selectedRegion.Items.Add("Americas");
            selectedRegion.Items.Add("Asia");
        }
        private void hotKeyKey_KeyDown(object? sender, KeyEventArgs e)
        {
            switch(Control.ModifierKeys)
            {
                case Keys.Alt:
                    modifier = Control.ModifierKeys;
                    break;
                case Keys.Shift:
                    modifier = Control.ModifierKeys;
                    break;
                case Keys.LShiftKey:
                    modifier = Control.ModifierKeys;
                    break;
                case Keys.RShiftKey:
                    modifier = Control.ModifierKeys;
                    break;
                case Keys.Control:
                    modifier = Control.ModifierKeys;
                    break;
                default:
                    modifier = null;
                    break;

            }
            if (Control.ModifierKeys == modifier)
            {
                Debug.WriteLine("Modifier handled: " + modifier.ToString());
            } else
            {
                Debug.WriteLine("Modifier not handled: " + Control.ModifierKeys.ToString());
            }
            if (e.KeyCode > Keys.NumPad0 || e.KeyCode < Keys.NumPad9)
            {
                Debug.WriteLine("Current modifier: " + modifier.ToString());
                Debug.WriteLine("Key is a keypad press");
                // Keypad, only supported for now.
                var _hotkey = "";
                switch(e.KeyCode)
                {
                    case Keys.NumPad0:
                        _hotkey = "Numpad.0";
                        break;
                    case Keys.NumPad1:
                        _hotkey = "Numpad.1";
                        break;
                    case Keys.NumPad2:
                        _hotkey = "Numpad.2";
                        break;
                    case Keys.NumPad3:
                        _hotkey = "Numpad.3";
                        break;
                    case Keys.NumPad4:
                        _hotkey = "Numpad.4";
                        break;
                    case Keys.NumPad5:
                        _hotkey = "Numpad.5";
                        break;
                    case Keys.NumPad6:
                        _hotkey = "Numpad.6";
                        break;
                    case Keys.NumPad7:
                        _hotkey = "Numpad.7";
                        break;
                    case Keys.NumPad8:
                        _hotkey = "Numpad.8";
                        break;
                    case Keys.Insert:
                        _hotkey = "Numpad.Insert";
                        break;
                    case Keys.Delete:
                        _hotkey = "Numpad.Delete";
                        break;
                    case Keys.End:
                        _hotkey = "Numpad.End";
                        break;
                    case Keys.Down:
                        _hotkey = "Numpad.Down";
                        break;
                    case Keys.PageDown:
                        _hotkey = "Numpad.Pgdn";
                        break;
                    case Keys.Left:
                        _hotkey = "Numpad.Left";
                        break;
                    case Keys.Clear:
                        _hotkey = "Numpad.Clear";
                        break;
                    case Keys.Home:
                        _hotkey = "Numpad.Home";
                        break;
                    case Keys.Up:
                        _hotkey = "Numpad.Up";
                        break;
                    case Keys.PageUp:
                        _hotkey = "Numpad.Pgup";
                        break;
                    case Keys.Right:
                        _hotkey = "Numpad.Right";
                        break;
                    default:
                        break;
                }
                // Debug.WriteLine("New hotkey: " + _hotkey + " Modifier: " + modifier.ToString());
                hotKeyPressString = _hotkey;
                if(hotKeyPressString.Length > 0)
                {
                    hotkey = e.KeyCode;
                    _HotKey = new HotKey { ModifierKey = modifier, Key = e.KeyCode, Enabled = true };
                    hotKeyPressRegistered = true;
                }
            }
        }
        private void hotKeyKey_KeyPress(object? sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void hotKeyKey_KeyUp(object? sender, System.Windows.Forms.KeyEventArgs e)
        {
            // Check for the flag being set in the KeyDown event.

            if (hotKeyPressRegistered == true && e.Modifiers == Keys.None)
            {
                var newText = "";
                // Stop the character from being entered into the control since it is non-numerical.
                if (modifier.ToString()?.Length > 0)
                    newText = modifier.ToString() + "+";
                currentHotKey.Text = newText + "["+hotKeyPressString+"]";
                hotKeyKey.Text = "";
                hotKeyPressRegistered = false;
            } else
            {
                // Debug.WriteLine("Still borked...");
            }


        }
        private void selectAccount_SelectedIndexChanged(object? sender, EventArgs e)
        {
            LoadProfile();
        }

        private void useDefaultGame_CheckedChanged(object? sender, EventArgs e)
        {
            if (useDefaultGame.Checked)
            {
                installationPath.Text = (string?)Registry.GetValue(Constants.gameInstallRegKey[0], Constants.gameInstallRegKey[1], "");
                gameExecutableName.Text = Constants.clientExecutableName;
            }
        }

        private void skipIntroVideos_CheckedChanged(object? sender, EventArgs e)
        {

        }

        private void selectedRegion_SelectedIndexChanged(object? sender, EventArgs e)
        {

        }
        private void browseForInstallationButton_Click(object? sender, EventArgs e)
        {
            int size = -1;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.InitialDirectory = (string?)Registry.GetValue(Constants.gameInstallRegKey[0], Constants.gameInstallRegKey[1], "");
            openFileDialog1.Filter = "Executables (*.exe)|*.exe";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string filepath = openFileDialog1.FileName;
                string? path = Path.GetDirectoryName(filepath);
                string file = Path.GetFileNameWithoutExtension(filepath);
                
                installationPath.Text = path;
                gameExecutableName.Text = file;
            }
            Console.WriteLine(size); // <-- Shows file size in debugging mode.
            Console.WriteLine(result); // <-- For debugging use.
        }
        private void grabWindowXYButton_Click(object? sender, EventArgs e)
        {
            if (_Profile == null || _Profile.DisplayName == null)
                return;
            var window = MH?.GetActiveWindow(_Profile.DisplayName);
            if(window != null)
            {
                try
                {
                    var rect = new Rect();
                    if (window.Process == null)
                        return;
                    WindowHelper.GetWindowRect(window.Process.MainWindowHandle, ref rect);
                    windowXposition.Text = rect.Left.ToString();
                    windowYposition.Text = rect.Top.ToString();
                } catch (Exception re)
                {
                    Log.Debug("Can not find window rect for: " + _Profile.DisplayName);
                    Log.Debug(re.ToString());
                }
            }
        } 
    }
}
