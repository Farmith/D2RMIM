using MultiInstanceManager.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiInstanceManager
{
    public partial class MultiInstanceManager : Form
    {
        MultiHandler MH;
        public MultiInstanceManager()
        {
            InitializeComponent();
            addAccountButton.Click += new EventHandler(addAccountButton_Click);
            launchButton.Click += new EventHandler(launchButton_Click);
            removeButton.Click += new EventHandler(removeButton_Click);
            refreshButton.Click += new EventHandler(refreshButton_Click);
            readmeLink.Click += new EventHandler(readmeLink_Click);
            killHandlesButton.Click += new EventHandler(killHandlesButton_Click);
            dumpRegKeyButton.Click += new EventHandler(dumpRegKeyButton_Click);
            MH = new MultiHandler(this,accountList);
            MH.LoadAccounts();
        }
        private void dumpRegKeyButton_Click(object sender, EventArgs e)
        {
            MH.DumpCurrentRegKey();
        }
        private void addAccountButton_Click(object sender, System.EventArgs e)
        {
            MH.Setup();
        }
        private void killHandlesButton_Click(object sender, EventArgs e)
        {
            ProcessManager.CloseExternalHandles();
            // MH.KillGameClientHandles();
        }
        private void launchButton_Click(object sender, System.EventArgs e)
        {
            MH.ResetSessions();
            if(accountList.CheckedItems.Count != 0)
            {
                for(var x = 0; x < accountList.CheckedItems.Count; x++)
                {
                    try
                    {
                        var checkedItem = accountList.CheckedItems[x].ToString().Split('|')[0].Trim(' ');

                        MH.LaunchWithAccount(checkedItem);
                    } catch(Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        // Something went terribly wrong.. 
                    }
                }
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
        private void refreshButton_Click(object sender, System.EventArgs e)
        {
            MH.ClearDebug();
            if (accountList.CheckedItems.Count != 0)
            {
                for (var x = 0; x < accountList.CheckedItems.Count; x++)
                {
                    try
                    {
                        var checkedItem = accountList.CheckedItems[x].ToString().Split('|');

                        MH.Setup(checkedItem[0].Trim(' '),true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
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
