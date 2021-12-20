
namespace MultiInstanceManager
{
    partial class MultiInstanceManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ColumnHeader ProfileNameHeader;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiInstanceManager));
            this.label1 = new System.Windows.Forms.Label();
            this.launchButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.readmeLink = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.addAccountButton = new System.Windows.Forms.Button();
            this.dumpRegKeyButton = new System.Windows.Forms.Button();
            this.forceExit = new System.Windows.Forms.CheckBox();
            this.forceExitToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.saveAccounInfo = new System.Windows.Forms.CheckBox();
            this.configureAccountsButton = new System.Windows.Forms.Button();
            this.appVersion = new System.Windows.Forms.Label();
            this.toggleAllProfiles = new System.Windows.Forms.CheckBox();
            this.accountListView = new System.Windows.Forms.ListView();
            this.lastUpdatedHeader = new System.Windows.Forms.ColumnHeader();
            ProfileNameHeader = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // ProfileNameHeader
            // 
            ProfileNameHeader.Tag = "profileName";
            ProfileNameHeader.Text = "Profile Name";
            ProfileNameHeader.Width = 130;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(15, 100);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Profiles:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // launchButton
            // 
            this.launchButton.BackColor = System.Drawing.Color.Green;
            this.launchButton.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.launchButton.FlatAppearance.BorderColor = System.Drawing.Color.GreenYellow;
            this.launchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.launchButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.launchButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.launchButton.Location = new System.Drawing.Point(298, 190);
            this.launchButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.launchButton.Name = "launchButton";
            this.launchButton.Size = new System.Drawing.Size(133, 27);
            this.launchButton.TabIndex = 2;
            this.launchButton.Text = "Launch";
            this.launchButton.UseVisualStyleBackColor = false;
            // 
            // refreshButton
            // 
            this.refreshButton.BackColor = System.Drawing.Color.Gray;
            this.refreshButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.refreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.refreshButton.Location = new System.Drawing.Point(298, 224);
            this.refreshButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(133, 27);
            this.refreshButton.TabIndex = 3;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = false;
            // 
            // removeButton
            // 
            this.removeButton.BackColor = System.Drawing.Color.Crimson;
            this.removeButton.Cursor = System.Windows.Forms.Cursors.No;
            this.removeButton.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.removeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removeButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.removeButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.removeButton.Location = new System.Drawing.Point(298, 275);
            this.removeButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(133, 27);
            this.removeButton.TabIndex = 4;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Location = new System.Drawing.Point(19, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(334, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "This application has the ability to launch several D2R instances";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Location = new System.Drawing.Point(19, 43);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Note: Please read the ";
            // 
            // readmeLink
            // 
            this.readmeLink.AutoSize = true;
            this.readmeLink.BackColor = System.Drawing.Color.Transparent;
            this.readmeLink.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.readmeLink.Location = new System.Drawing.Point(145, 43);
            this.readmeLink.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.readmeLink.Name = "readmeLink";
            this.readmeLink.Size = new System.Drawing.Size(53, 15);
            this.readmeLink.TabIndex = 7;
            this.readmeLink.TabStop = true;
            this.readmeLink.Text = "README";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label4.Location = new System.Drawing.Point(204, 43);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "it explains how to use this app";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label5.Location = new System.Drawing.Point(282, 423);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(146, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "Created by: Farmith - 2021";
            // 
            // addAccountButton
            // 
            this.addAccountButton.BackColor = System.Drawing.Color.Gray;
            this.addAccountButton.Cursor = System.Windows.Forms.Cursors.Cross;
            this.addAccountButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.addAccountButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addAccountButton.ForeColor = System.Drawing.Color.Transparent;
            this.addAccountButton.Location = new System.Drawing.Point(298, 123);
            this.addAccountButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.addAccountButton.Name = "addAccountButton";
            this.addAccountButton.Size = new System.Drawing.Size(133, 27);
            this.addAccountButton.TabIndex = 10;
            this.addAccountButton.Text = "Add Profile";
            this.addAccountButton.UseVisualStyleBackColor = false;
            // 
            // dumpRegKeyButton
            // 
            this.dumpRegKeyButton.BackColor = System.Drawing.Color.Gray;
            this.dumpRegKeyButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.dumpRegKeyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dumpRegKeyButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.dumpRegKeyButton.Location = new System.Drawing.Point(299, 320);
            this.dumpRegKeyButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dumpRegKeyButton.Name = "dumpRegKeyButton";
            this.dumpRegKeyButton.Size = new System.Drawing.Size(132, 27);
            this.dumpRegKeyButton.TabIndex = 14;
            this.dumpRegKeyButton.Text = "Dump RegKey";
            this.dumpRegKeyButton.UseVisualStyleBackColor = false;
            // 
            // forceExit
            // 
            this.forceExit.AutoSize = true;
            this.forceExit.BackColor = System.Drawing.Color.Transparent;
            this.forceExit.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.forceExit.Location = new System.Drawing.Point(19, 360);
            this.forceExit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.forceExit.Name = "forceExit";
            this.forceExit.Size = new System.Drawing.Size(77, 19);
            this.forceExit.TabIndex = 16;
            this.forceExit.Text = "Force Exit";
            this.forceExit.UseVisualStyleBackColor = false;
            // 
            // saveAccounInfo
            // 
            this.saveAccounInfo.AutoSize = true;
            this.saveAccounInfo.BackColor = System.Drawing.Color.Transparent;
            this.saveAccounInfo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.saveAccounInfo.Location = new System.Drawing.Point(19, 387);
            this.saveAccounInfo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.saveAccounInfo.Name = "saveAccounInfo";
            this.saveAccounInfo.Size = new System.Drawing.Size(119, 19);
            this.saveAccounInfo.TabIndex = 18;
            this.saveAccounInfo.Text = "Save Accountinfo";
            this.saveAccounInfo.UseVisualStyleBackColor = false;
            // 
            // configureAccountsButton
            // 
            this.configureAccountsButton.BackColor = System.Drawing.Color.Gray;
            this.configureAccountsButton.Cursor = System.Windows.Forms.Cursors.Help;
            this.configureAccountsButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.configureAccountsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.configureAccountsButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.configureAccountsButton.Location = new System.Drawing.Point(299, 157);
            this.configureAccountsButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.configureAccountsButton.Name = "configureAccountsButton";
            this.configureAccountsButton.Size = new System.Drawing.Size(132, 27);
            this.configureAccountsButton.TabIndex = 19;
            this.configureAccountsButton.Text = "Configure";
            this.configureAccountsButton.UseVisualStyleBackColor = false;
            // 
            // appVersion
            // 
            this.appVersion.AutoSize = true;
            this.appVersion.BackColor = System.Drawing.Color.Transparent;
            this.appVersion.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.appVersion.Location = new System.Drawing.Point(15, 423);
            this.appVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.appVersion.Name = "appVersion";
            this.appVersion.Size = new System.Drawing.Size(51, 15);
            this.appVersion.TabIndex = 20;
            this.appVersion.Text = "Version: ";
            // 
            // toggleAllProfiles
            // 
            this.toggleAllProfiles.AutoSize = true;
            this.toggleAllProfiles.BackColor = System.Drawing.Color.Transparent;
            this.toggleAllProfiles.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.toggleAllProfiles.Location = new System.Drawing.Point(125, 100);
            this.toggleAllProfiles.Name = "toggleAllProfiles";
            this.toggleAllProfiles.Size = new System.Drawing.Size(165, 19);
            this.toggleAllProfiles.TabIndex = 21;
            this.toggleAllProfiles.Text = "Select/Deselect All Profiles";
            this.toggleAllProfiles.UseVisualStyleBackColor = false;
            // 
            // accountListView
            // 
            this.accountListView.CheckBoxes = true;
            this.accountListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            ProfileNameHeader,
            this.lastUpdatedHeader});
            this.accountListView.FullRowSelect = true;
            this.accountListView.Location = new System.Drawing.Point(19, 125);
            this.accountListView.Name = "accountListView";
            this.accountListView.Size = new System.Drawing.Size(265, 222);
            this.accountListView.TabIndex = 22;
            this.accountListView.UseCompatibleStateImageBehavior = false;
            // 
            // lastUpdatedHeader
            // 
            this.lastUpdatedHeader.Text = "Last Updated";
            this.lastUpdatedHeader.Width = 130;
            // 
            // MultiInstanceManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(457, 452);
            this.Controls.Add(this.accountListView);
            this.Controls.Add(this.toggleAllProfiles);
            this.Controls.Add(this.appVersion);
            this.Controls.Add(this.configureAccountsButton);
            this.Controls.Add(this.saveAccounInfo);
            this.Controls.Add(this.forceExit);
            this.Controls.Add(this.dumpRegKeyButton);
            this.Controls.Add(this.addAccountButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.readmeLink);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.launchButton);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MultiInstanceManager";
            this.Text = "Diablo II : Resurrected - Multi Instance Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button launchButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel readmeLink;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button addAccountButton;
        private System.Windows.Forms.Button dumpRegKeyButton;
        private System.Windows.Forms.CheckBox forceExit;
        private System.Windows.Forms.ToolTip forceExitToolTip;
        private System.Windows.Forms.CheckBox saveAccounInfo;
        private System.Windows.Forms.Button configureAccountsButton;
        private System.Windows.Forms.Label appVersion;
        private System.Windows.Forms.CheckBox toggleAllProfiles;
        private System.Windows.Forms.ListView accountListView;
        private System.Windows.Forms.ColumnHeader ProfileNameHeader;
        private System.Windows.Forms.ColumnHeader lastUpdatedHeader;
    }
}

