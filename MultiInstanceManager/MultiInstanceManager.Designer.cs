﻿
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiInstanceManager));
            this.label1 = new System.Windows.Forms.Label();
            this.accountList = new System.Windows.Forms.CheckedListBox();
            this.launchButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.readmeLink = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.addAccountButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.commandLineArguments = new System.Windows.Forms.TextBox();
            this.dumpRegKeyButton = new System.Windows.Forms.Button();
            this.forceExit = new System.Windows.Forms.CheckBox();
            this.forceExitToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.modifyWindowTitles = new System.Windows.Forms.CheckBox();
            this.saveAccounInfo = new System.Windows.Forms.CheckBox();
            this.configureAccountsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Accounts:";
            // 
            // accountList
            // 
            this.accountList.FormattingEnabled = true;
            this.accountList.Location = new System.Drawing.Point(16, 107);
            this.accountList.Name = "accountList";
            this.accountList.Size = new System.Drawing.Size(233, 154);
            this.accountList.TabIndex = 1;
            // 
            // launchButton
            // 
            this.launchButton.Location = new System.Drawing.Point(255, 136);
            this.launchButton.Name = "launchButton";
            this.launchButton.Size = new System.Drawing.Size(114, 23);
            this.launchButton.TabIndex = 2;
            this.launchButton.Text = "Launch";
            this.launchButton.UseVisualStyleBackColor = true;
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(255, 165);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(114, 23);
            this.refreshButton.TabIndex = 3;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(255, 238);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(114, 23);
            this.removeButton.TabIndex = 4;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(305, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "This application has the ability to launch several D2R instances";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Note: Please read the ";
            // 
            // readmeLink
            // 
            this.readmeLink.AutoSize = true;
            this.readmeLink.Location = new System.Drawing.Point(124, 37);
            this.readmeLink.Name = "readmeLink";
            this.readmeLink.Size = new System.Drawing.Size(53, 13);
            this.readmeLink.TabIndex = 7;
            this.readmeLink.TabStop = true;
            this.readmeLink.Text = "README";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(175, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "it explains how to use this app";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(242, 367);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Created by: Farmith - 2021";
            // 
            // addAccountButton
            // 
            this.addAccountButton.Location = new System.Drawing.Point(255, 107);
            this.addAccountButton.Name = "addAccountButton";
            this.addAccountButton.Size = new System.Drawing.Size(114, 23);
            this.addAccountButton.TabIndex = 10;
            this.addAccountButton.Text = "Add Account";
            this.addAccountButton.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 264);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(169, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Optional command-line arguments:";
            // 
            // commandLineArguments
            // 
            this.commandLineArguments.Location = new System.Drawing.Point(19, 281);
            this.commandLineArguments.Name = "commandLineArguments";
            this.commandLineArguments.Size = new System.Drawing.Size(230, 20);
            this.commandLineArguments.TabIndex = 12;
            // 
            // dumpRegKeyButton
            // 
            this.dumpRegKeyButton.Location = new System.Drawing.Point(256, 277);
            this.dumpRegKeyButton.Name = "dumpRegKeyButton";
            this.dumpRegKeyButton.Size = new System.Drawing.Size(113, 23);
            this.dumpRegKeyButton.TabIndex = 14;
            this.dumpRegKeyButton.Text = "Dump RegKey";
            this.dumpRegKeyButton.UseVisualStyleBackColor = true;
            // 
            // forceExit
            // 
            this.forceExit.AutoSize = true;
            this.forceExit.Location = new System.Drawing.Point(19, 331);
            this.forceExit.Name = "forceExit";
            this.forceExit.Size = new System.Drawing.Size(73, 17);
            this.forceExit.TabIndex = 16;
            this.forceExit.Text = "Force Exit";
            this.forceExit.UseVisualStyleBackColor = true;
            // 
            // modifyWindowTitles
            // 
            this.modifyWindowTitles.AutoSize = true;
            this.modifyWindowTitles.Location = new System.Drawing.Point(19, 308);
            this.modifyWindowTitles.Name = "modifyWindowTitles";
            this.modifyWindowTitles.Size = new System.Drawing.Size(127, 17);
            this.modifyWindowTitles.TabIndex = 17;
            this.modifyWindowTitles.Text = "Modify Window Titles";
            this.modifyWindowTitles.UseVisualStyleBackColor = true;
            // 
            // saveAccounInfo
            // 
            this.saveAccounInfo.AutoSize = true;
            this.saveAccounInfo.Location = new System.Drawing.Point(178, 308);
            this.saveAccounInfo.Name = "saveAccounInfo";
            this.saveAccounInfo.Size = new System.Drawing.Size(180, 17);
            this.saveAccounInfo.TabIndex = 18;
            this.saveAccounInfo.Text = "Save Accountinfo (Experimental)";
            this.saveAccounInfo.UseVisualStyleBackColor = true;
            // 
            // configureAccountsButton
            // 
            this.configureAccountsButton.Location = new System.Drawing.Point(256, 195);
            this.configureAccountsButton.Name = "configureAccountsButton";
            this.configureAccountsButton.Size = new System.Drawing.Size(113, 23);
            this.configureAccountsButton.TabIndex = 19;
            this.configureAccountsButton.Text = "Configure";
            this.configureAccountsButton.UseVisualStyleBackColor = true;
            this.configureAccountsButton.Click += new System.EventHandler(this.configureAccountsButton_Click);
            // 
            // MultiInstanceManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 392);
            this.Controls.Add(this.configureAccountsButton);
            this.Controls.Add(this.saveAccounInfo);
            this.Controls.Add(this.modifyWindowTitles);
            this.Controls.Add(this.forceExit);
            this.Controls.Add(this.dumpRegKeyButton);
            this.Controls.Add(this.commandLineArguments);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.addAccountButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.readmeLink);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.launchButton);
            this.Controls.Add(this.accountList);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MultiInstanceManager";
            this.Text = "Diablo II : Resurrected - Multi Instance Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox accountList;
        private System.Windows.Forms.Button launchButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel readmeLink;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button addAccountButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox commandLineArguments;
        private System.Windows.Forms.Button dumpRegKeyButton;
        private System.Windows.Forms.CheckBox forceExit;
        private System.Windows.Forms.ToolTip forceExitToolTip;
        private System.Windows.Forms.CheckBox modifyWindowTitles;
        private System.Windows.Forms.CheckBox saveAccounInfo;
        private System.Windows.Forms.Button configureAccountsButton;
    }
}

