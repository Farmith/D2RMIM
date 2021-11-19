
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
            this.grabWindowXYButton = new System.Windows.Forms.Button();
            this.grabXYTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.groupBox1.Location = new System.Drawing.Point(13, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(381, 121);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Game Details";
            // 
            // useDefaultGame
            // 
            this.useDefaultGame.AutoSize = true;
            this.useDefaultGame.Location = new System.Drawing.Point(286, 90);
            this.useDefaultGame.Name = "useDefaultGame";
            this.useDefaultGame.Size = new System.Drawing.Size(82, 17);
            this.useDefaultGame.TabIndex = 5;
            this.useDefaultGame.Text = "Use Default";
            this.useDefaultGame.UseVisualStyleBackColor = true;
            this.useDefaultGame.CheckedChanged += new System.EventHandler(this.useDefaultGame_CheckedChanged);
            // 
            // gameExecutableName
            // 
            this.gameExecutableName.Location = new System.Drawing.Point(9, 90);
            this.gameExecutableName.Name = "gameExecutableName";
            this.gameExecutableName.Size = new System.Drawing.Size(267, 20);
            this.gameExecutableName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(191, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Game Executable Name (Without .exe)";
            // 
            // browseForInstallationButton
            // 
            this.browseForInstallationButton.Location = new System.Drawing.Point(282, 44);
            this.browseForInstallationButton.Name = "browseForInstallationButton";
            this.browseForInstallationButton.Size = new System.Drawing.Size(75, 23);
            this.browseForInstallationButton.TabIndex = 2;
            this.browseForInstallationButton.Text = "browse";
            this.browseForInstallationButton.UseVisualStyleBackColor = true;
            // 
            // installationPath
            // 
            this.installationPath.Location = new System.Drawing.Point(9, 46);
            this.installationPath.Name = "installationPath";
            this.installationPath.Size = new System.Drawing.Size(267, 20);
            this.installationPath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Installation Folder";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grabWindowXYButton);
            this.groupBox2.Controls.Add(this.muteWhenMinimized);
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
            this.groupBox2.Location = new System.Drawing.Point(12, 168);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(450, 220);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Launch Settings";
            // 
            // muteWhenMinimized
            // 
            this.muteWhenMinimized.AutoSize = true;
            this.muteWhenMinimized.Location = new System.Drawing.Point(172, 188);
            this.muteWhenMinimized.Name = "muteWhenMinimized";
            this.muteWhenMinimized.Size = new System.Drawing.Size(155, 17);
            this.muteWhenMinimized.TabIndex = 17;
            this.muteWhenMinimized.Text = "Mute client when minimized";
            this.muteWhenMinimized.UseVisualStyleBackColor = true;
            // 
            // separateJsonSettings
            // 
            this.separateJsonSettings.AutoSize = true;
            this.separateJsonSettings.Location = new System.Drawing.Point(13, 188);
            this.separateJsonSettings.Name = "separateJsonSettings";
            this.separateJsonSettings.Size = new System.Drawing.Size(152, 17);
            this.separateJsonSettings.TabIndex = 16;
            this.separateJsonSettings.Text = "Use Separate settings.json";
            this.separateJsonSettings.UseVisualStyleBackColor = true;
            // 
            // separateTaskbarItems
            // 
            this.separateTaskbarItems.AutoSize = true;
            this.separateTaskbarItems.Location = new System.Drawing.Point(172, 164);
            this.separateTaskbarItems.Name = "separateTaskbarItems";
            this.separateTaskbarItems.Size = new System.Drawing.Size(140, 17);
            this.separateTaskbarItems.TabIndex = 15;
            this.separateTaskbarItems.Text = "Separate Taskbar Icons";
            this.separateTaskbarItems.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(284, 103);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 13);
            this.label12.TabIndex = 14;
            this.label12.Text = "Y:";
            // 
            // windowYposition
            // 
            this.windowYposition.Location = new System.Drawing.Point(307, 100);
            this.windowYposition.Name = "windowYposition";
            this.windowYposition.Size = new System.Drawing.Size(47, 20);
            this.windowYposition.TabIndex = 13;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(190, 103);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "X:";
            // 
            // windowXposition
            // 
            this.windowXposition.Location = new System.Drawing.Point(213, 100);
            this.windowXposition.Name = "windowXposition";
            this.windowXposition.Size = new System.Drawing.Size(47, 20);
            this.windowXposition.TabIndex = 11;
            // 
            // gameLaunchArgs
            // 
            this.gameLaunchArgs.Location = new System.Drawing.Point(176, 71);
            this.gameLaunchArgs.Name = "gameLaunchArgs";
            this.gameLaunchArgs.Size = new System.Drawing.Size(181, 20);
            this.gameLaunchArgs.TabIndex = 10;
            // 
            // postLaunchCmd
            // 
            this.postLaunchCmd.Location = new System.Drawing.Point(176, 45);
            this.postLaunchCmd.Name = "postLaunchCmd";
            this.postLaunchCmd.Size = new System.Drawing.Size(181, 20);
            this.postLaunchCmd.TabIndex = 9;
            // 
            // preLaunchCmd
            // 
            this.preLaunchCmd.Location = new System.Drawing.Point(176, 19);
            this.preLaunchCmd.Name = "preLaunchCmd";
            this.preLaunchCmd.Size = new System.Drawing.Size(181, 20);
            this.preLaunchCmd.TabIndex = 8;
            // 
            // selectedRegion
            // 
            this.selectedRegion.FormattingEnabled = true;
            this.selectedRegion.Location = new System.Drawing.Point(219, 136);
            this.selectedRegion.Name = "selectedRegion";
            this.selectedRegion.Size = new System.Drawing.Size(58, 21);
            this.selectedRegion.TabIndex = 7;
            this.selectedRegion.SelectedIndexChanged += new System.EventHandler(this.selectedRegion_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(169, 140);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Region:";
            // 
            // modifyWindowTitles
            // 
            this.modifyWindowTitles.AutoSize = true;
            this.modifyWindowTitles.Location = new System.Drawing.Point(13, 164);
            this.modifyWindowTitles.Name = "modifyWindowTitles";
            this.modifyWindowTitles.Size = new System.Drawing.Size(127, 17);
            this.modifyWindowTitles.TabIndex = 5;
            this.modifyWindowTitles.Text = "Modify Window Titles";
            this.modifyWindowTitles.UseVisualStyleBackColor = true;
            // 
            // skipIntroVideos
            // 
            this.skipIntroVideos.AutoSize = true;
            this.skipIntroVideos.Location = new System.Drawing.Point(13, 138);
            this.skipIntroVideos.Name = "skipIntroVideos";
            this.skipIntroVideos.Size = new System.Drawing.Size(106, 17);
            this.skipIntroVideos.TabIndex = 4;
            this.skipIntroVideos.Text = "Skip Intro Videos";
            this.skipIntroVideos.UseVisualStyleBackColor = true;
            this.skipIntroVideos.CheckedChanged += new System.EventHandler(this.skipIntroVideos_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(53, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Game Window Position";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Game launch arguments:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(81, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Post-launch cmd:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(86, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Pre-launch cmd:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Select Profile:";
            // 
            // selectAccount
            // 
            this.selectAccount.FormattingEnabled = true;
            this.selectAccount.Location = new System.Drawing.Point(112, 13);
            this.selectAccount.Name = "selectAccount";
            this.selectAccount.Size = new System.Drawing.Size(177, 21);
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
            this.groupBox3.Location = new System.Drawing.Point(13, 394);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(381, 93);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Hot Key";
            // 
            // currentHotKey
            // 
            this.currentHotKey.Enabled = false;
            this.currentHotKey.Location = new System.Drawing.Point(248, 20);
            this.currentHotKey.Name = "currentHotKey";
            this.currentHotKey.Size = new System.Drawing.Size(120, 20);
            this.currentHotKey.TabIndex = 5;
            // 
            // hotKeyKey
            // 
            this.hotKeyKey.Location = new System.Drawing.Point(248, 53);
            this.hotKeyKey.MaxLength = 40;
            this.hotKeyKey.Name = "hotKeyKey";
            this.hotKeyKey.Size = new System.Drawing.Size(120, 20);
            this.hotKeyKey.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(172, 56);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "New HotKey:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(160, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Current HotKey:";
            // 
            // enableHotkeys
            // 
            this.enableHotkeys.AutoSize = true;
            this.enableHotkeys.Location = new System.Drawing.Point(7, 20);
            this.enableHotkeys.Name = "enableHotkeys";
            this.enableHotkeys.Size = new System.Drawing.Size(135, 17);
            this.enableHotkeys.TabIndex = 0;
            this.enableHotkeys.Text = "Enable Hotkey (Global)";
            this.enableHotkeys.UseVisualStyleBackColor = true;
            // 
            // saveConfig
            // 
            this.saveConfig.Location = new System.Drawing.Point(319, 12);
            this.saveConfig.Name = "saveConfig";
            this.saveConfig.Size = new System.Drawing.Size(75, 23);
            this.saveConfig.TabIndex = 5;
            this.saveConfig.Text = "Save";
            this.saveConfig.UseVisualStyleBackColor = true;
            // 
            // grabWindowXYButton
            // 
            this.grabWindowXYButton.Location = new System.Drawing.Point(360, 98);
            this.grabWindowXYButton.Name = "grabWindowXYButton";
            this.grabWindowXYButton.Size = new System.Drawing.Size(75, 23);
            this.grabWindowXYButton.TabIndex = 18;
            this.grabWindowXYButton.Text = "Grab X/Y";
            this.grabWindowXYButton.UseVisualStyleBackColor = true;
            // 
            // AccountConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 508);
            this.Controls.Add(this.saveConfig);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.selectAccount);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AccountConfiguration";
            this.Text = "Profile Configuration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
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
    }
}