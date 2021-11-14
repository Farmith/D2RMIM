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
        private void SaveConfiguration()
        {
            Account config = new Account();

            config.DisplayName = selectAccount.SelectedIndex.ToString();
            if (!useDefaultGame.Checked) {
                config.InstallationPath = installationPath.Text;
                config.GameExecutable = gameExecutableName.Text;
            } else
            {
                try
                {
                    config.InstallationPath = (string)Registry.GetValue(Constants.gameInstallRegKey[0], Constants.gameInstallRegKey[1], "");
                } catch (Exception e)
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

            HotKey windowHotKey = new HotKey();
            var mod = "";
            switch (hotKeyModifier.Text)
            {
            }
            // windowHotKey.ModifierKey = hotKeyModifier.SelectedIndex.ToString();
            windowHotKey.Key = hotKeyKey.Text.ToCharArray().First();

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
            if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
            {
                Debug.WriteLine("Key is a keypad press");
                // Keypad, only supported for now.
                switch(e.KeyCode)
                {
                    case Keys.NumPad0:
                        hotKeyKey.Text = "Numpad.0";
                        break;
                    case Keys.NumPad1:
                        hotKeyKey.Text = "Numpad.1";
                        break;
                    case Keys.NumPad2:
                        hotKeyKey.Text = "Numpad.2";
                        break;
                    case Keys.NumPad3:
                        hotKeyKey.Text = "Numpad.3";
                        break;
                    case Keys.NumPad4:
                        hotKeyKey.Text = "Numpad.4";
                        break;
                    case Keys.NumPad5:
                        hotKeyKey.Text = "Numpad.5";
                        break;
                    case Keys.NumPad6:
                        hotKeyKey.Text = "Numpad.6";
                        break;
                    case Keys.NumPad7:
                        hotKeyKey.Text = "Numpad.7";
                        break;
                    case Keys.NumPad8:
                        hotKeyKey.Text = "Numpad.8";
                        break;
                    default:
                        break;
                }
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
