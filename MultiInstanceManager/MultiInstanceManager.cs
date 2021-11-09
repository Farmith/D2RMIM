using Gma.System.MouseKeyHook;
using MultiInstanceManager.Config;
using MultiInstanceManager.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiInstanceManager
{
    public partial class MultiInstanceManager : Form
    {
        MultiHandler MH;
        Settings settings;
        private bool saveCredentials;
        public MultiInstanceManager(IKeyboardMouseEvents keyboardMouseEvents)
        {
            InitializeComponent();
            saveCredentials = false; // Default to false like everywhere, just incase
            addAccountButton.Click += new EventHandler(addAccountButton_Click);
            launchButton.Click += new EventHandler(launchButton_Click);
            removeButton.Click += new EventHandler(removeButton_Click);
            refreshButton.Click += new EventHandler(refreshButton_Click);
            readmeLink.Click += new EventHandler(readmeLink_Click);
            killHandlesButton.Click += new EventHandler(killHandlesButton_Click);
            dumpRegKeyButton.Click += new EventHandler(dumpRegKeyButton_Click);
            forceExit.CheckedChanged += new EventHandler(forceExit_Changed);
            
            try
            {
                commandLineArguments.Text = ConfigurationManager.AppSettings["cmdArgs"];
            } catch (Exception e)
            {
                commandLineArguments.Text = "";
            }
            commandLineArguments.TextChanged += new EventHandler(commandLineArguments_Changed);
            modifyWindowTitles.Checked = ConfigurationManager.AppSettings.Get("modifyWindowTitles")?.ToString() == "true" ? true : false;
            modifyWindowTitles.CheckedChanged += new EventHandler(modifyWindowTitles_Changed);
            forceExit.Checked = ConfigurationManager.AppSettings.Get("forceExitClients")?.ToString() == "true" ? true : false;

            forceExitToolTip.SetToolTip(forceExit, "ForceExit means, kill the game client once the tokens are set when 'refreshing'");
            MH = new MultiHandler(this, accountList);

            // Prepare keybinds
            Debug.WriteLine("Adding keybinds");
            settings = new Settings();
            settings.LoadWindowKeys();
            Debug.WriteLine("Done with keybinds");
            MH.LoadAccounts();
            saveCredentials = ConfigurationManager.AppSettings.Get("saveCredentials")?.ToString() == "true" ? true : false;
            MH.SetCredentialMode(saveCredentials);
            MH.ToggleWindowTitleMode(modifyWindowTitles.Checked);
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
                Debug.WriteLine("Game name config faulty");
            }
   
            keyboardMouseEvents.KeyPress += (_, args) =>
            {
                // Prepare usage of tab-keys between windows
                Debug.WriteLine("Keypress: " + args.KeyChar);
                if (MH.PriorityWindowFocus())
                {
                    foreach (var binding in settings.KeyToggles)
                    {
                        Debug.WriteLine("Comparing to: " + binding.CharCode.ToString());
                        if (char.TryParse(binding.CharCode.ToString(), out char c))
                        {
                            Debug.WriteLine("Character: " + c);
                            if (args.KeyChar == c)
                            {
                                Debug.WriteLine("Found match: " + c + " Iterator: " + binding.ClientIterator);
                                MH.SwapFocus(binding);
                                args.Handled = true;
                            }
                        }
                    }
                }
            };
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
        public void forceExit_Changed(object sender, EventArgs e)
        {
            AddOrUpdateAppSettings("forceExitClients", forceExit.Checked.ToString());
        }
        public void modifyWindowTitles_Changed(object sender, EventArgs e)
        {
            MH.ToggleWindowTitleMode(modifyWindowTitles.Checked);
        }
        private void commandLineArguments_Changed(object sender, EventArgs e)
        {
            AddOrUpdateAppSettings("cmdArgs",commandLineArguments.Text);
        }
        private void dumpRegKeyButton_Click(object sender, EventArgs e)
        {
            MH.DumpCurrentRegKey();
        }
        private void addAccountButton_Click(object sender, System.EventArgs e)
        {
            MH.Setup("",commandLineArguments.Text);
        }
        private void killHandlesButton_Click(object sender, EventArgs e)
        {
            // ProcessManager.CloseExternalHandles(ConfigurationManager.AppSettings.Get("gameExecutableName")?.ToString());
            // MH.KillGameClientHandles();
        }
        private async void launchButton_Click(object sender, System.EventArgs e)
        {
            MH.ResetSessions();
            if(accountList.CheckedItems.Count != 0)
            {
                for(var x = 0; x < accountList.CheckedItems.Count; x++)
                {
                    try
                    {
                        var checkedItem = accountList.CheckedItems[x].ToString().Split('|')[0].Trim(' ');
                        // Doing this in a thread makes sure the UI doesn't freeze
                        // var LaunchThread = new Thread(() => MH.LaunchWithAccount(checkedItem, commandLineArguments.Text));
                        DisableButtons();
                        var task = Task.Factory.StartNew(() => MH.LaunchWithAccount(checkedItem, commandLineArguments.Text));
                        var result = await task;
                        EnableButtons();
                        // MH.LaunchWithAccount(checkedItem,commandLineArguments.Text);
                    } catch(Exception ex)
                    {
                        Debug.WriteLine("Launch error: " + ex.ToString());
                        // Something went terribly wrong.. 
                    }
                }
                MH.LoadAccounts();
            }
        }
        private void removeButton_Click(object sender, System.EventArgs e)
        {
            if (accountList.CheckedItems.Count != 0)
            {
                for (var x = 0; x < accountList.CheckedItems.Count; x++)
                {
                    try
                    {
                        var checkedItem = accountList.CheckedItems[x].ToString().Split('|')[0];
                        accountList.Items.Remove(accountList.CheckedItems[x]);
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
            MH.ClearDebug();
            if (accountList.CheckedItems.Count != 0)
            {
                for (var x = 0; x < accountList.CheckedItems.Count; x++)
                {
                    try
                    {
                        var checkedItem = accountList.CheckedItems[x].ToString().Split('|');
                        DisableButtons();
                        var task = Task.Factory.StartNew(() => MH.Setup(checkedItem[0].Trim(' '), commandLineArguments.Text, true));
                        var result = await task;
                        EnableButtons();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
                MH.LoadAccounts();
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
