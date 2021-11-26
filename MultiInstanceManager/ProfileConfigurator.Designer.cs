
namespace MultiInstanceManager
{
    partial class AccountConfiguration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountConfiguration));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.useDefaultGame = new System.Windows.Forms.CheckBox();
            this.gameExecutableName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.browseForInstallationButton = new System.Windows.Forms.Button();
            this.installationPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grabWindowXYButton = new System.Windows.Forms.Button();
            this.muteWhenMinimized = new System.Windows.Forms.CheckBox();
            this.separateJsonSettings = new System.Windows.Forms.CheckBox();
            this.separateTaskbarItems = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.windowYposition = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.windowXposition = new System.Windows.Forms.TextBox();
            this.gameLaunchArgs = new System.Windows.Forms.TextBox();
            this.postLaunchCmd = new System.Windows.Forms.TextBox();
            this.preLaunchCmd = new System.Windows.Forms.TextBox();
            this.selectedRegion = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.modifyWindowTitles = new System.Windows.Forms.CheckBox();
            this.skipIntroVideos = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.selectAccount = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.currentHotKey = new System.Windows.Forms.TextBox();
            this.hotKeyKey = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.enableHotkeys = new System.Windows.Forms.CheckBox();
            this.saveConfig = new System.Windows.Forms.Button();
            this.grabXYTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.useDefaultGame);
            this.groupBox1.Controls.Add(this.gameExecutableName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.browseForInstallationButton);
            this.groupBox1.Controls.Add(this.installationPath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(15, 47);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(444, 140);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Game Details";
            // 
            // useDefaultGame
            // 
            this.useDefaultGame.AutoSize = true;
            this.useDefaultGame.Location = new System.Drawing.Point(334, 104);
            this.useDefaultGame.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.useDefaultGame.Name = "useDefaultGame";
            this.useDefaultGame.Size = new System.Drawing.Size(86, 19);
            this.useDefaultGame.TabIndex = 5;
            this.useDefaultGame.Text = "Use Default";
            this.useDefaultGame.UseVisualStyleBackColor = true;
            this.useDefaultGame.CheckedChanged += new System.EventHandler(this.useDefaultGame_CheckedChanged);
            // 
            // gameExecutableName
            // 
            this.gameExecutableName.Location = new System.Drawing.Point(10, 104);
            this.gameExecutableName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gameExecutableName.Name = "gameExecutableName";
            this.gameExecutableName.Size = new System.Drawing.Size(311, 23);
            this.gameExecutableName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(211, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Game Executable Name (Without .exe)";
            // 
            // browseForInstallationButton
            // 
            this.browseForInstallationButton.Location = new System.Drawing.Point(329, 51);
            this.browseForInstallationButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.browseForInstallationButton.Name = "browseForInstallationButton";
            this.browseForInstallationButton.Size = new System.Drawing.Size(88, 27);
            this.browseForInstallationButton.TabIndex = 2;
            this.browseForInstallationButton.Text = "browse";
            this.browseForInstallationButton.UseVisualStyleBackColor = true;
            // 
            // installationPath
            // 
            this.installationPath.Location = new System.Drawing.Point(10, 53);
            this.installationPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.installationPath.Name = "installationPath";
            this.installationPath.Size = new System.Drawing.Size(311, 23);
            this.installationPath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Installation Folder";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grabWindowXYButton);
            this.groupBox2.Controls.Add(this.separateJsonSettings);
            this.groupBox2.Controls.Add(this.separateTaskbarItems);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.windowYposition);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.windowXposition);
            this.groupBox2.Controls.Add(this.gameLaunchArgs);
            this.groupBox2.Controls.Add(this.postLaunchCmd);
            this.groupBox2.Controls.Add(this.preLaunchCmd);
            this.groupBox2.Controls.Add(this.selectedRegion);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.modifyWindowTitles);
            this.groupBox2.Controls.Add(this.skipIntroVideos);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(14, 194);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new System.Drawing.Size(525, 254);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Launch Settings";
            // 
            // grabWindowXYButton
            // 
            this.grabWindowXYButton.Location = new System.Drawing.Point(420, 113);
            this.grabWindowXYButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grabWindowXYButton.Name = "grabWindowXYButton";
            this.grabWindowXYButton.Size = new System.Drawing.Size(88, 27);
            this.grabWindowXYButton.TabIndex = 18;
            this.grabWindowXYButton.Text = "Grab X/Y";
            this.grabWindowXYButton.UseVisualStyleBackColor = true;
            // 
            // muteWhenMinimized
            // 
            this.muteWhenMinimized.AutoSize = true;
            this.muteWhenMinimized.Location = new System.Drawing.Point(7, 83);
            this.muteWhenMinimized.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.muteWhenMinimized.Name = "muteWhenMinimized";
            this.muteWhenMinimized.Size = new System.Drawing.Size(267, 19);
            this.muteWhenMinimized.TabIndex = 17;
            this.muteWhenMinimized.Text = "Mute client when minimized (requires plugin)";
            this.muteWhenMinimized.UseVisualStyleBackColor = true;
            // 
            // separateJsonSettings
            // 
            this.separateJsonSettings.AutoSize = true;
            this.separateJsonSettings.Location = new System.Drawing.Point(15, 217);
            this.separateJsonSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.separateJsonSettings.Name = "separateJsonSettings";
            this.separateJsonSettings.Size = new System.Drawing.Size(162, 19);
            this.separateJsonSettings.TabIndex = 16;
            this.separateJsonSettings.Text = "Use Separate settings.json";
            this.separateJsonSettings.UseVisualStyleBackColor = true;
            // 
            // separateTaskbarItems
            // 
            this.separateTaskbarItems.AutoSize = true;
            this.separateTaskbarItems.Location = new System.Drawing.Point(201, 189);
            this.separateTaskbarItems.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.separateTaskbarItems.Name = "separateTaskbarItems";
            this.separateTaskbarItems.Size = new System.Drawing.Size(144, 19);
            this.separateTaskbarItems.TabIndex = 15;
            this.separateTaskbarItems.Text = "Separate Taskbar Icons";
            this.separateTaskbarItems.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(331, 119);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 15);
            this.label12.TabIndex = 14;
            this.label12.Text = "Y:";
            // 
            // windowYposition
            // 
            this.windowYposition.Location = new System.Drawing.Point(358, 115);
            this.windowYposition.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.windowYposition.Name = "windowYposition";
            this.windowYposition.Size = new System.Drawing.Size(54, 23);
            this.windowYposition.TabIndex = 13;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(222, 119);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 15);
            this.label11.TabIndex = 12;
            this.label11.Text = "X:";
            // 
            // windowXposition
            // 
            this.windowXposition.Location = new System.Drawing.Point(248, 115);
            this.windowXposition.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.windowXposition.Name = "windowXposition";
            this.windowXposition.Size = new System.Drawing.Size(54, 23);
            this.windowXposition.TabIndex = 11;
            // 
            // gameLaunchArgs
            // 
            this.gameLaunchArgs.Location = new System.Drawing.Point(205, 82);
            this.gameLaunchArgs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gameLaunchArgs.Name = "gameLaunchArgs";
            this.gameLaunchArgs.Size = new System.Drawing.Size(210, 23);
            this.gameLaunchArgs.TabIndex = 10;
            // 
            // postLaunchCmd
            // 
            this.postLaunchCmd.Location = new System.Drawing.Point(205, 52);
            this.postLaunchCmd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.postLaunchCmd.Name = "postLaunchCmd";
            this.postLaunchCmd.Size = new System.Drawing.Size(210, 23);
            this.postLaunchCmd.TabIndex = 9;
            // 
            // preLaunchCmd
            // 
            this.preLaunchCmd.Location = new System.Drawing.Point(205, 22);
            this.preLaunchCmd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.preLaunchCmd.Name = "preLaunchCmd";
            this.preLaunchCmd.Size = new System.Drawing.Size(210, 23);
            this.preLaunchCmd.TabIndex = 8;
            // 
            // selectedRegion
            // 
            this.selectedRegion.FormattingEnabled = true;
            this.selectedRegion.Location = new System.Drawing.Point(255, 157);
            this.selectedRegion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.selectedRegion.Name = "selectedRegion";
            this.selectedRegion.Size = new System.Drawing.Size(67, 23);
            this.selectedRegion.TabIndex = 7;
            this.selectedRegion.SelectedIndexChanged += new System.EventHandler(this.selectedRegion_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(197, 162);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 15);
            this.label7.TabIndex = 6;
            this.label7.Text = "Region:";
            // 
            // modifyWindowTitles
            // 
            this.modifyWindowTitles.AutoSize = true;
            this.modifyWindowTitles.Location = new System.Drawing.Point(15, 189);
            this.modifyWindowTitles.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.modifyWindowTitles.Name = "modifyWindowTitles";
            this.modifyWindowTitles.Size = new System.Drawing.Size(141, 19);
            this.modifyWindowTitles.TabIndex = 5;
            this.modifyWindowTitles.Text = "Modify Window Titles";
            this.modifyWindowTitles.UseVisualStyleBackColor = true;
            // 
            // skipIntroVideos
            // 
            this.skipIntroVideos.AutoSize = true;
            this.skipIntroVideos.Location = new System.Drawing.Point(15, 159);
            this.skipIntroVideos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.skipIntroVideos.Name = "skipIntroVideos";
            this.skipIntroVideos.Size = new System.Drawing.Size(114, 19);
            this.skipIntroVideos.TabIndex = 4;
            this.skipIntroVideos.Text = "Skip Intro Videos";
            this.skipIntroVideos.UseVisualStyleBackColor = true;
            this.skipIntroVideos.CheckedChanged += new System.EventHandler(this.skipIntroVideos_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(62, 115);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 15);
            this.label6.TabIndex = 3;
            this.label6.Text = "Game Window Position";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(52, 87);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(140, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "Game launch arguments:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(94, 57);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "Post-launch cmd:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(100, 27);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Pre-launch cmd:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 20);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 15);
            this.label8.TabIndex = 2;
            this.label8.Text = "Select Profile:";
            // 
            // selectAccount
            // 
            this.selectAccount.FormattingEnabled = true;
            this.selectAccount.Location = new System.Drawing.Point(131, 15);
            this.selectAccount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.selectAccount.Name = "selectAccount";
            this.selectAccount.Size = new System.Drawing.Size(206, 23);
            this.selectAccount.TabIndex = 3;
            this.selectAccount.SelectedIndexChanged += new System.EventHandler(this.selectAccount_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.currentHotKey);
            this.groupBox3.Controls.Add(this.hotKeyKey);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.enableHotkeys);
            this.groupBox3.Location = new System.Drawing.Point(476, 47);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Size = new System.Drawing.Size(444, 141);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Hot Key";
            // 
            // currentHotKey
            // 
            this.currentHotKey.Enabled = false;
            this.currentHotKey.Location = new System.Drawing.Point(289, 23);
            this.currentHotKey.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.currentHotKey.Name = "currentHotKey";
            this.currentHotKey.Size = new System.Drawing.Size(139, 23);
            this.currentHotKey.TabIndex = 5;
            // 
            // hotKeyKey
            // 
            this.hotKeyKey.Location = new System.Drawing.Point(289, 61);
            this.hotKeyKey.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.hotKeyKey.MaxLength = 40;
            this.hotKeyKey.Name = "hotKeyKey";
            this.hotKeyKey.Size = new System.Drawing.Size(139, 23);
            this.hotKeyKey.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(201, 65);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 15);
            this.label10.TabIndex = 3;
            this.label10.Text = "New HotKey:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(187, 27);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 15);
            this.label9.TabIndex = 1;
            this.label9.Text = "Current HotKey:";
            // 
            // enableHotkeys
            // 
            this.enableHotkeys.AutoSize = true;
            this.enableHotkeys.Location = new System.Drawing.Point(8, 23);
            this.enableHotkeys.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.enableHotkeys.Name = "enableHotkeys";
            this.enableHotkeys.Size = new System.Drawing.Size(147, 19);
            this.enableHotkeys.TabIndex = 0;
            this.enableHotkeys.Text = "Enable Hotkey (Global)";
            this.enableHotkeys.UseVisualStyleBackColor = true;
            // 
            // saveConfig
            // 
            this.saveConfig.Location = new System.Drawing.Point(372, 14);
            this.saveConfig.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.saveConfig.Name = "saveConfig";
            this.saveConfig.Size = new System.Drawing.Size(88, 27);
            this.saveConfig.TabIndex = 5;
            this.saveConfig.Text = "Save";
            this.saveConfig.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.checkBox1);
            this.groupBox4.Controls.Add(this.muteWhenMinimized);
            this.groupBox4.Location = new System.Drawing.Point(546, 194);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(360, 254);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Plugin settings";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(7, 108);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(189, 19);
            this.checkBox1.TabIndex = 18;
            this.checkBox1.Text = "Restart client when crash/close";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(21, 25);
            this.label13.MaximumSize = new System.Drawing.Size(355, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(310, 45);
            this.label13.TabIndex = 19;
            this.label13.Text = "The settings below are dependent on plugins, if you have removed any plugin from " +
    "the plugin/ folder then the corresponding checkbox below has no functionality";
            // 
            // AccountConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 465);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.saveConfig);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.selectAccount);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "AccountConfiguration";
            this.Text = "Profile Configuration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox gameExecutableName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button browseForInstallationButton;
        private System.Windows.Forms.TextBox installationPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox selectedRegion;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox modifyWindowTitles;
        private System.Windows.Forms.CheckBox skipIntroVideos;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox gameLaunchArgs;
        private System.Windows.Forms.TextBox postLaunchCmd;
        private System.Windows.Forms.TextBox preLaunchCmd;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox selectAccount;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox enableHotkeys;
        private System.Windows.Forms.TextBox hotKeyKey;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button saveConfig;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox windowXposition;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox windowYposition;
        private System.Windows.Forms.CheckBox useDefaultGame;
        private System.Windows.Forms.TextBox currentHotKey;
        private System.Windows.Forms.CheckBox separateTaskbarItems;
        private System.Windows.Forms.CheckBox separateJsonSettings;
        private System.Windows.Forms.CheckBox muteWhenMinimized;
        private System.Windows.Forms.Button grabWindowXYButton;
        private System.Windows.Forms.ToolTip grabXYTooltip;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label13;
    }
}