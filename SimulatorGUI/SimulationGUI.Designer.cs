namespace SimulatorGUI
{
    partial class SimulationGUI
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
            this.ModuleList = new System.Windows.Forms.ListBox();
            this.AddButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.GraphTabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.StartButton = new System.Windows.Forms.Button();
            this.StatusBox = new System.Windows.Forms.GroupBox();
            this.BaseLoad = new System.Windows.Forms.Label();
            this.CurrentTimeLabel = new System.Windows.Forms.Label();
            this.DayCountLabel = new System.Windows.Forms.Label();
            this.DayTimeLabel = new System.Windows.Forms.Label();
            this.ConvertedTimeLabel = new System.Windows.Forms.Label();
            this.ModuleNameBox = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openButton = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.RelativeHumidity = new System.Windows.Forms.Label();
            this.GraphTabs.SuspendLayout();
            this.StatusBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ModuleList
            // 
            this.ModuleList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ModuleList.FormattingEnabled = true;
            this.ModuleList.Location = new System.Drawing.Point(13, 26);
            this.ModuleList.Name = "ModuleList";
            this.ModuleList.Size = new System.Drawing.Size(130, 394);
            this.ModuleList.TabIndex = 0;
            // 
            // AddButton
            // 
            this.AddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddButton.Location = new System.Drawing.Point(13, 452);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(57, 26);
            this.AddButton.TabIndex = 1;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RemoveButton.Location = new System.Drawing.Point(86, 452);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(57, 26);
            this.RemoveButton.TabIndex = 2;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // GraphTabs
            // 
            this.GraphTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GraphTabs.Controls.Add(this.tabPage1);
            this.GraphTabs.Controls.Add(this.tabPage2);
            this.GraphTabs.Location = new System.Drawing.Point(149, 27);
            this.GraphTabs.Name = "GraphTabs";
            this.GraphTabs.SelectedIndex = 0;
            this.GraphTabs.Size = new System.Drawing.Size(843, 393);
            this.GraphTabs.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(835, 367);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(835, 367);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // StartButton
            // 
            this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StartButton.Location = new System.Drawing.Point(862, 426);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(130, 52);
            this.StartButton.TabIndex = 4;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // StatusBox
            // 
            this.StatusBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusBox.Controls.Add(this.BaseLoad);
            this.StatusBox.Controls.Add(this.CurrentTimeLabel);
            this.StatusBox.Controls.Add(this.DayTimeLabel);
            this.StatusBox.Controls.Add(this.DayCountLabel);
            this.StatusBox.Controls.Add(this.ConvertedTimeLabel);
            this.StatusBox.Location = new System.Drawing.Point(500, 420);
            this.StatusBox.Name = "StatusBox";
            this.StatusBox.Size = new System.Drawing.Size(300, 65);
            this.StatusBox.TabIndex = 5;
            this.StatusBox.TabStop = false;
            this.StatusBox.Text = "Status";
            // 
            // BaseLoad
            // 
            this.BaseLoad.AutoSize = true;
            this.BaseLoad.Location = new System.Drawing.Point(7, 33);
            this.BaseLoad.Name = "BaseLoad";
            this.BaseLoad.Size = new System.Drawing.Size(90, 13);
            this.BaseLoad.TabIndex = 1;
            this.BaseLoad.Text = "Base Load: 0 kW";
            // 
            // CurrentTimeLabel
            // 
            this.CurrentTimeLabel.AutoSize = true;
            this.CurrentTimeLabel.Location = new System.Drawing.Point(7, 20);
            this.CurrentTimeLabel.Name = "CurrentTimeLabel";
            this.CurrentTimeLabel.Size = new System.Drawing.Size(79, 13);
            this.CurrentTimeLabel.TabIndex = 0;
            this.CurrentTimeLabel.Text = "Current Time: 0";
            this.CurrentTimeLabel.Click += new System.EventHandler(this.CurrentTimeLabel_Click);

            this.ConvertedTimeLabel.AutoSize = true;
            this.ConvertedTimeLabel.Location = new System.Drawing.Point(7, 48);
            this.ConvertedTimeLabel.Name = "ConvertedTimeLabel";
            this.ConvertedTimeLabel.Size = new System.Drawing.Size(79, 13);
            this.ConvertedTimeLabel.TabIndex = 0;
            this.ConvertedTimeLabel.Text = "Converted Time: 0";
            this.ConvertedTimeLabel.Click += new System.EventHandler(this.ConvertedTimeLabel_Click);

            this.DayTimeLabel.AutoSize = true;
            this.DayTimeLabel.Location = new System.Drawing.Point(180, 20);
            this.DayTimeLabel.Name = "CurrentTimeLabel";
            this.DayTimeLabel.Size = new System.Drawing.Size(79, 13);
            this.DayTimeLabel.TabIndex = 0;
            this.DayTimeLabel.Text = "Day Time Status: False";
            this.DayTimeLabel.Click += new System.EventHandler(this.DayTimeLabel_Click);

            this.DayCountLabel.AutoSize = true;
            this.DayCountLabel.Location = new System.Drawing.Point(180, 33);
            this.DayCountLabel.Name = "DayCountLabel";
            this.DayCountLabel.Size = new System.Drawing.Size(79, 13);
            this.DayCountLabel.TabIndex = 0;
            this.DayCountLabel.Text = "Day Number: 0";
            this.DayCountLabel.Click += new System.EventHandler(this.DayCountLabel_Click);

            // 
            // ModuleNameBox
            // 
            this.ModuleNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ModuleNameBox.Location = new System.Drawing.Point(12, 426);
            this.ModuleNameBox.Name = "ModuleNameBox";
            this.ModuleNameBox.Size = new System.Drawing.Size(131, 20);
            this.ModuleNameBox.TabIndex = 6;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1004, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openButton,
            this.SaveButton});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openButton
            // 
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(103, 22);
            this.openButton.Text = "Open";
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(103, 22);
            this.SaveButton.Text = "Save";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // RelativeHumidity
            // 
            this.RelativeHumidity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RelativeHumidity.AutoSize = true;
            this.RelativeHumidity.Location = new System.Drawing.Point(153, 427);
            this.RelativeHumidity.Name = "RelativeHumidity";
            this.RelativeHumidity.Size = new System.Drawing.Size(0, 13);
            this.RelativeHumidity.TabIndex = 8;
            this.RelativeHumidity.Click += new System.EventHandler(this.label1_Click);
            // 
            // SimulationGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 494);
            this.Controls.Add(this.RelativeHumidity);
            this.Controls.Add(this.ModuleNameBox);
            this.Controls.Add(this.StatusBox);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.GraphTabs);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.ModuleList);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SimulationGUI";
            this.Text = "Lunar Base Simulator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.GraphTabs.ResumeLayout(false);
            this.StatusBox.ResumeLayout(false);
            this.StatusBox.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ModuleList;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.TabControl GraphTabs;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.GroupBox StatusBox;
        private System.Windows.Forms.Label CurrentTimeLabel;
        private System.Windows.Forms.Label ConvertedTimeLabel;
        private System.Windows.Forms.Label DayTimeLabel;
        private System.Windows.Forms.Label DayCountLabel;
        private System.Windows.Forms.TextBox ModuleNameBox;
        private System.Windows.Forms.Label BaseLoad;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openButton;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.Label RelativeHumidity;
    }
}

