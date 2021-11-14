using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using MultiInstanceManager.Helpers;
using MultiInstanceManager.Structs;

namespace MultiInstanceManager
{
    public partial class AccountConfiguration : Form
    {
        private List<AccountBinary> Accounts;
        private bool hotKeyPressRegistered = false;
        private string hotKeyPressString = "";
        private Keys? modifier = null;
        private Keys? hotkey = null;
        private HotKey _HotKey;
        private Account _Account;

        public AccountConfiguration()
        {
            InitializeComponent();
            resetRegion();
            FillAccounts();
            selectAccount.SelectedIndexChanged += new EventHandler(selectAccount_SelectedIndexChanged);

            // These below may not be neccessary because: Save Button
            useDefaultGame.CheckedChanged += new EventHandler(useDefaultGame_CheckedChanged);
            skipIntroVideos.CheckedChanged += new EventHandler(skipIntroVideos_CheckedChanged);
            hotKeyKey.KeyDown += hotKeyKey_KeyDown;
            hotKeyKey.KeyPress += hotKeyKey_KeyPress;
            hotKeyKey.KeyUp += hotKeyKey_KeyUp;

            saveConfig.Click += SaveConfiguration;
        }
        private void LoadAccount()
        {
            _Account = FileHelper.LoadAccountConfiguration(selectAccount.SelectedItem.ToString());
            if (_Account != null)
            {
                modifyWindowTitles.Checked = _Account.ModifyWindowtitles;
                skipIntroVideos.Checked = _Account.SkipCinematics;
                enableHotkeys.Checked = _Account.WindowHotKey.Enabled;
                var extraMod = "";
                if (_Account.WindowHotKey.ModifierKey.ToString().Length > 0)
                {
                    extraMod = _Account.WindowHotKey.ModifierKey.ToString() + "+";
                }
                currentHotKey.Text = extraMod + "[" + _Account.WindowHotKey.Key.ToString() + "]";
                gameExecutableName.Text = _Account.GameExecutable;
                installationPath.Text = _Account.InstallationPath;
                useDefaultGame.Checked = _Account.UseDefaultGameInstallation;
            } else
            {
                Console.WriteLine("Previous config is null");
            }
        }
        private void FillAccounts()
        {
            selectAccount.Items.Clear();
            Accounts = FileHelper.GetAccountsByFolder();
            foreach(var account in Accounts)
            {
                selectAccount.Items.Add(account.AccountName);
            }
        }
        private void SaveConfiguration(object sender, EventArgs e)
        {
            Account config = new Account();

            config.DisplayName = selectAccount.SelectedItem.ToString();
            if (!useDefaultGame.Checked) {
                config.InstallationPath = installationPath.Text;
                config.GameExecutable = gameExecutableName.Text;
            } else
            {
                try
                {
                    config.InstallationPath = (string)Registry.GetValue(Constants.gameInstallRegKey[0], Constants.gameInstallRegKey[1], "");
                } catch (Exception ex)
                {
                    Log.Debug("Can not find installation in registry: " + e.ToString());
                    config.InstallationPath = "";
                }
                config.GameExecutable = Constants.clientExecutableName;
            }
            LaunchSettings launchSettings = new LaunchSettings();
            launchSettings.PreLaunchCommands = preLaunchCmd.Text;
            launchSettings.PostLaunchCommands = postLaunchCmd.Text;
            launchSettings.LaunchArguments = gameLaunchArgs.Text;

            if(int.TryParse(windowXposition.Text,out int x))
            {
                launchSettings.WindowX = x;
            }
            if (int.TryParse(windowXposition.Text, out int y))
            {
                launchSettings.WindowY = y;
            }
            config.LaunchOptions = launchSettings;

            config.SkipCinematics = skipIntroVideos.Checked;
            config.ModifyWindowtitles = modifyWindowTitles.Checked;
            config.WindowHotKey = _HotKey;

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
        private void hotKeyKey_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("Key: " + e.KeyCode.ToString());
            

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
                Debug.WriteLine("New hotkey: " + _hotkey + " Modifier: " + modifier.ToString());
                hotKeyPressString = _hotkey;
                if(hotKeyPressString.Length > 0)
                {
                    hotkey = e.KeyCode;
                    _HotKey = new HotKey { ModifierKey = modifier, Key = e.KeyCode, Enabled = true };
                    hotKeyPressRegistered = true;
                }
            }
        }
        private void hotKeyKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void hotKeyKey_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // Check for the flag being set in the KeyDown event.

            if (hotKeyPressRegistered == true && e.Modifiers == Keys.None)
            {
                var newText = "";
                // Stop the character from being entered into the control since it is non-numerical.
                if (modifier.ToString().Length > 0)
                    newText = modifier.ToString() + "+";
                currentHotKey.Text = newText + "["+hotKeyPressString+"]";
                hotKeyKey.Text = "";
                hotKeyPressRegistered = false;
            } else
            {
                Debug.WriteLine("Still borked...");
            }


        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void selectAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void useDefaultGame_CheckedChanged(object sender, EventArgs e)
        {
            if (useDefaultGame.Checked)
            {
                installationPath.Text = (string)Registry.GetValue(Constants.gameInstallRegKey[0], Constants.gameInstallRegKey[1], "");
                gameExecutableName.Text = Constants.clientExecutableName + Constants.executableFileExt;
            }
        }

        private void skipIntroVideos_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void selectedRegion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
