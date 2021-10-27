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

namespace MultiInstanceManager
{
    public partial class MultiInstanceManager : Form
    {
        public MultiInstanceManager()
        {
            InitializeComponent();
            addAccountButton.Click += new EventHandler(addAccountButton_Click);
            launchButton.Click += new EventHandler(launchButton_Click);
            removeButton.Click += new EventHandler(removeButton_Click);
            refreshButton.Click += new EventHandler(refreshButton_Click);
            readmeLink.Click += new EventHandler(readmeLink_Click);
        }
        private void addAccountButton_Click(object sender, System.EventArgs e)
        {
            _ = MessageBox.Show("Add Account was clicked");
        }
        private void launchButton_Click(object sender, System.EventArgs e)
        {
            _ = MessageBox.Show("Launch was clicked");
        }
        private void removeButton_Click(object sender, System.EventArgs e)
        {
            _ = MessageBox.Show("Remove was clicked");
        }
        private void refreshButton_Click(object sender, System.EventArgs e)
        {
            _ = MessageBox.Show("Refresh was clicked");
        }
        private void readmeLink_Click(object sender, System.EventArgs e)
        {
            var ePath = Application.ExecutablePath;
            var path = System.IO.Path.GetDirectoryName(ePath);
            Process.Start(path + "\\README.txt");
        }
    }
}
