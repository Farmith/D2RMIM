using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public AccountConfiguration()
        {
            InitializeComponent();
            resetRegion();
            selectAccount.SelectedIndexChanged += new EventHandler(selectAccount_SelectedIndexChanged);

            // These below may not be neccessary because: Save Button
            useDefaultGame.CheckedChanged += new EventHandler(useDefaultGame_CheckedChanged);
            skipIntroVideos.CheckedChanged += new EventHandler(skipIntroVideos_CheckedChanged);
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

        }

        private void skipIntroVideos_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void selectedRegion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
