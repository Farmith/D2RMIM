﻿using Dfust.Hotkeys;
using MultiInstanceManager.Config;
using MultiInstanceManager.Helpers;
using MultiInstanceManager.Modules;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using static Dfust.Hotkeys.Enums;
using System.Threading;

namespace MultiInstanceManager
{
    public partial class MultiInstanceManager : Form
    {
        MultiHandler MH;
        Settings settings;
        AccountConfiguration accountConfig;
        PluginManager pluginManager;

        public MultiInstanceManager()
        {
            InitializeComponent();

            // Fetch the commandline arguments first and foremost:
            var CommandLineArguments = CMDLineHelper.GetArguments();

            // Load all plugins
            pluginManager = new PluginManager();
            try
            {
                pluginManager.LoadPlugins();
            }
            catch (Exception e)
            {
                Log.Debug(string.Format("Plugins couldn't be loaded: {0}", e.Message));
            }

            Log.Debug("Setting up plugin communication in a separate thread");
            var pluginHandlerCTS = new CancellationTokenSource();
            CancellationToken pluginHandlerCT = pluginHandlerCTS.Token;
            var pluginHandlerTask = Task.Factory.StartNew(() => pluginManager.MessageHandler(pluginHandlerCT), pluginHandlerCTS.Token);

            addAccountButton.Click += new EventHandler(addAccountButton_Click);
            launchButton.Click += new EventHandler(launchButton_Click);
            removeButton.Click += new EventHandler(removeButton_Click);
            refreshButton.Click += new EventHandler(refreshButton_Click);
            readmeLink.Click += new EventHandler(readmeLink_Click);
            dumpRegKeyButton.Click += new EventHandler(dumpRegKeyButton_Click);
            forceExit.CheckedChanged += new EventHandler(forceExit_Changed);
            forceExit.Checked = ConfigurationManager.AppSettings.Get("forceExitClients")?.ToString() == "true" ? true : false;
            saveAccounInfo.Checked = ConfigurationManager.AppSettings.Get("saveCredentials")?.ToString() == "true" ? true : false;
            saveAccounInfo.CheckedChanged += new EventHandler(saveAccounInfo_Changed);
            // Reset log once per start of application
            if(!CommandLineArguments.TryGetValue("keeplogs",out List<String> parm))
            {
                Log.Empty();
            }
            /* 
             * Clean any pre-1.6.2 version profiles to use a new name
             */
            FileHelper.MoveOldProfileConfigurations();

            // Show version
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            appVersion.Text = (String.Format("Version: {0}.{1}.{2} Build: {3}", version.Major, version.Minor, version.Build, version.Revision));
            /*
             * Account configuration stuff
             * 
             */
            configureAccountsButton.Click += new EventHandler(configureAccountsButton_Click);
            accountConfig = new AccountConfiguration();
            accountConfig.Disposed += new EventHandler(accountConfig_Disposed);

            forceExitToolTip.SetToolTip(forceExit, "ForceExit means, kill the game client once the tokens are set when 'refreshing'");
            MH = new MultiHandler(this, accountList);
            MH.SetCredentialMode(saveAccounInfo.Checked);
            // Start the Monitors early:
            MH.StartProcessMonitor();
            MH.StartAudioMonitor();

            // Prepare keybinds
            Debug.WriteLine("Adding keybinds");
            settings = new Settings();
            settings.LoadWindowKeys();
            Debug.WriteLine("Done with keybinds");
            MH.LoadProfiles();
            try
            {
                var gn = ConfigurationManager.AppSettings.Get("gameExecutableName").ToString();
                if (gn.Length > 5)
                {
                    MH.SetGameExecutableName(gn);
                }
            } 
            catch (Exception e)
            {
                Debug.WriteLine("Game name config faulty: " + e.ToString());
            }
            
            /*
             * HotKey stuffs
             * 
             */

            var hotkeyCollection = new HotkeyCollection(Scope.Global);
            var store = MH.GetAllProfiles();
            foreach (var a in store)
            {
                var bind = a.WindowHotKey;
                if (bind == null)
                    continue;
                var hotkey = (Keys)bind.Key | (Keys)bind.ModifierKey;
                hotkeyCollection.RegisterHotkey(hotkey, (e) =>
                {
                    Debug.WriteLine("Found recipient for hotkey: " + bind.Key.ToString() + " mod: " + bind.ModifierKey.ToString());
                    Debug.WriteLine("Account: " + a.DisplayName);
                    MH.SwapFocus(a);
                });
            }
            // Auto-launch features:
            bool launchWhenAllRefreshed = false;
            if (CommandLineArguments.TryGetValue("relaunch", out List<String> laf))
            {
                Log.Debug("Auto-starting all profiles we refresh, after last refresh");
                launchWhenAllRefreshed = true;
            }
            if (CommandLineArguments.TryGetValue("autorefresh", out List<String> profs))
            {
                Log.Debug("Refreshing: " + profs.Count + " profiles on start due to cmd-line request");
                RefreshProfiles(profs,launchWhenAllRefreshed);
            }
            if (CommandLineArguments.TryGetValue("autostart", out List<String> autostarts) && !launchWhenAllRefreshed)
            {
                Log.Debug("Auto-starting: " + autostarts.Count + " profiles on start due to cmd-line request");
                LaunchProfiles(autostarts);
            }

        }
        public async Task LaunchProfiles(List<String> profiles)
        {
            foreach (var profile in profiles)
            {
                try
                {
                    DisableButtons();
                    Log.Debug("Launching profile: " + profile);
                    var task = Task.Factory.StartNew(() => MH.LaunchWithAccount(profile.Trim(' ')));
                    var result = await task;
                    EnableButtons();
                }
                catch (Exception e)
                {
                    Log.Debug("Could not start profile: " + profile + " error: ");
                    Log.Debug(e.ToString());
                }
            }
            Log.Debug("All profiles should be launched");
        }
        public async Task RefreshProfiles(List<String> profiles,bool launchWhenAllRefreshed)
        {
            foreach (var profile in profiles)
            {
                try
                {
                    DisableButtons();
                    var task = Task.Factory.StartNew(() => MH.Setup(profile.Trim(' '), true));
                    var result = await task;
                    EnableButtons();
                }
                catch (Exception e)
                {
                    Log.Debug("Could not refresh: " + profile + " error: ");
                    Log.Debug(e.ToString());
                }
            }
            if(launchWhenAllRefreshed)
            {
                Log.Debug("Restarting all previous profiles due to commandline request");
                Log.Debug("Waiting a moment for refreshed client to completely die");
                Thread.Sleep(2000);
                var task = Task.Factory.StartNew(() => LaunchProfiles(profiles));
                var result = await task;
            }
        }
        public static void AddOrUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
        public void accountConfig_Disposed(object sender, EventArgs e)
        {
            accountConfig = new AccountConfiguration();
            accountConfig.Disposed += new EventHandler(accountConfig_Disposed);
            configureAccountsButton.Enabled = true;
        }
        public void configureAccountsButton_Click(object sender, EventArgs e)
        {
            configureAccountsButton.Enabled = false;
            accountConfig.Shown += accountConfig.OnShown;
            accountConfig.SetMultiHandler(MH);
            accountConfig.Show();
        }
        public void saveAccounInfo_Changed(object sender, EventArgs e)
        {
            AddOrUpdateAppSettings("saveCredentials", saveAccounInfo.Checked.ToString());
            MH.SetCredentialMode(saveAccounInfo.Checked);
        }
        public void forceExit_Changed(object sender, EventArgs e)
        {
            AddOrUpdateAppSettings("forceExitClients", forceExit.Checked.ToString());
        }
        private void dumpRegKeyButton_Click(object sender, EventArgs e)
        {
            MH.DumpCurrentRegKey();
        }
        private async void addAccountButton_Click(object sender, EventArgs e)
        {
            DisableButtons();
            var task = Task.Factory.StartNew(() => MH.Setup());
            var result = await task;
            EnableButtons();
            MH.LoadProfiles();
        }
        private async void launchButton_Click(object sender, EventArgs e)
        {
            MH.ResetSessions();
            if(accountList.CheckedItems.Count != 0)
            {
                for(var x = 0; x < accountList.CheckedItems.Count; x++)
                {
                    try
                    {
                        var checkedItem = accountList.CheckedItems[x].ToString().Split('|')[0].Trim(' ');
                        // Doing this in a task makes sure the UI doesn't freeze
                        DisableButtons();
                        var task = Task.Factory.StartNew(() => MH.LaunchWithAccount(checkedItem));
                        var result = await task;
                        EnableButtons();
                    } catch(Exception ex)
                    {
                        Debug.WriteLine("Launch error: " + ex.ToString());
                        // Something went terribly wrong.. 
                    }
                }
                MH.LoadProfiles();
            }
        }
        private void removeButton_Click(object sender, System.EventArgs e)
        {
            if (accountList.CheckedItems.Count > 0)
            {
                for (var x = 0; x < accountList.CheckedItems.Count; x++)
                {
                    try
                    {
                        var checkedItem = accountList.CheckedItems[x].ToString().Split('|')[0].Trim(' ');
                        accountList.Items.Remove(accountList.CheckedItems[x]);
                        Log.Debug("Removing Vault credentials for: " + checkedItem);
                        CredentialHelper.RemoveVaultCredentials(checkedItem);
                        Log.Debug("Deleting file: " + checkedItem + ".bin");
                        File.Delete(checkedItem + ".bin");
                    }
                    catch (Exception ex)
                    {
                        Log.Debug("Something went wrong while removing profiles:");
                        Log.Debug(ex.ToString());
                        Debug.WriteLine(ex);
                        // Something went terribly wrong.. 
                    }
                }
            }
        }
        private void EnableButtons()
        {
            removeButton.Enabled = true;
            addAccountButton.Enabled = true;
            launchButton.Enabled = true;
            refreshButton.Enabled = true;
        }
        private void DisableButtons()
        {
            removeButton.Enabled = false;
            addAccountButton.Enabled = false;
            launchButton.Enabled = false;
            refreshButton.Enabled = false;

        }
        private async void refreshButton_Click(object sender, System.EventArgs e)
        {
            Log.Clear();
            if (accountList.CheckedItems.Count != 0)
            {
                for (var x = 0; x < accountList.CheckedItems.Count; x++)
                {
                    try
                    {
                        var checkedItem = accountList.CheckedItems[x].ToString().Split('|');
                        DisableButtons();
                        var task = Task.Factory.StartNew(() => MH.Setup(checkedItem[0].Trim(' '), true));
                        var result = await task;
                        EnableButtons();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
                MH.LoadProfiles();
            }
        }
        private void readmeLink_Click(object sender, System.EventArgs e)
        {
            var ePath = Application.ExecutablePath;
            var path = System.IO.Path.GetDirectoryName(ePath);
            Process.Start(path + "\\README.txt");
        }

    }
}
