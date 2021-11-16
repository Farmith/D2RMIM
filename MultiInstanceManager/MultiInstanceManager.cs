using Dfust.Hotkeys;
using MultiInstanceManager.Config;
using MultiInstanceManager.Helpers;
using MultiInstanceManager.Modules;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Dfust.Hotkeys.Enums;

namespace MultiInstanceManager
{
    public partial class MultiInstanceManager : Form
    {
        MultiHandler MH;
        Settings settings;
        AccountConfiguration accountConfig;
        public MultiInstanceManager()
        {
            InitializeComponent();
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
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            appVersion.Text = String.Format("Version: {0}", version);
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
            // TODO: This feature should also try to clear the Windows Credential Store of credentials.
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
