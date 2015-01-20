namespace MyoSimGUI
{
    partial class MyoSimulatorForm
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
            this.sendCommandButton = new System.Windows.Forms.Button();
            this.gestureLabel = new System.Windows.Forms.Label();
            this.addGestureButton = new System.Windows.Forms.Button();
            this.gestureList = new System.Windows.Forms.ListBox();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recorderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startStopRecordingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playRecordingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.XYZLabel = new System.Windows.Forms.Label();
            this.XYZTextBox = new System.Windows.Forms.TextBox();
            this.AddXYZButton = new System.Windows.Forms.Button();
            this.delayLabel = new System.Windows.Forms.Label();
            this.delayTextBox = new System.Windows.Forms.TextBox();
            this.timeBox = new System.Windows.Forms.TextBox();
            this.timeLabel = new System.Windows.Forms.Label();
            this.commandChain = new System.Windows.Forms.TextBox();
            this.addDelayButton = new System.Windows.Forms.Button();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // sendCommandButton
            // 
            this.sendCommandButton.Location = new System.Drawing.Point(433, 360);
            this.sendCommandButton.Name = "sendCommandButton";
            this.sendCommandButton.Size = new System.Drawing.Size(75, 23);
            this.sendCommandButton.TabIndex = 9;
            this.sendCommandButton.Text = "Send";
            this.sendCommandButton.UseVisualStyleBackColor = true;
            this.sendCommandButton.Click += new System.EventHandler(this.sendCommandButton_Click);
            // 
            // gestureLabel
            // 
            this.gestureLabel.AutoSize = true;
            this.gestureLabel.Location = new System.Drawing.Point(12, 42);
            this.gestureLabel.Name = "gestureLabel";
            this.gestureLabel.Size = new System.Drawing.Size(77, 13);
            this.gestureLabel.TabIndex = 15;
            this.gestureLabel.Text = "Select Gesture";
            // 
            // addGestureButton
            // 
            this.addGestureButton.Location = new System.Drawing.Point(12, 185);
            this.addGestureButton.Name = "addGestureButton";
            this.addGestureButton.Size = new System.Drawing.Size(75, 23);
            this.addGestureButton.TabIndex = 16;
            this.addGestureButton.TabStop = false;
            this.addGestureButton.Text = "Add Gesture";
            this.addGestureButton.UseVisualStyleBackColor = true;
            this.addGestureButton.Click += new System.EventHandler(this.addGestureButton_Click);
            // 
            // gestureList
            // 
            this.gestureList.FormattingEnabled = true;
            this.gestureList.Location = new System.Drawing.Point(12, 58);
            this.gestureList.Name = "gestureList";
            this.gestureList.Size = new System.Drawing.Size(120, 121);
            this.gestureList.TabIndex = 17;
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.recorderToolStripMenuItem});
            this.menu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(520, 24);
            this.menu.TabIndex = 20;
            this.menu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadScriptToolStripMenuItem,
            this.saveScriptToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadScriptToolStripMenuItem
            // 
            this.loadScriptToolStripMenuItem.Name = "loadScriptToolStripMenuItem";
            this.loadScriptToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.loadScriptToolStripMenuItem.Text = "Load Script";
            this.loadScriptToolStripMenuItem.Click += new System.EventHandler(this.loadScriptToolStripMenuItem_Click);
            // 
            // saveScriptToolStripMenuItem
            // 
            this.saveScriptToolStripMenuItem.Name = "saveScriptToolStripMenuItem";
            this.saveScriptToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.saveScriptToolStripMenuItem.Text = "Save Script";
            this.saveScriptToolStripMenuItem.Click += new System.EventHandler(this.saveScriptToolStripMenuItem_Click);
            // 
            // recorderToolStripMenuItem
            // 
            this.recorderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startStopRecordingToolStripMenuItem,
            this.playRecordingToolStripMenuItem});
            this.recorderToolStripMenuItem.Name = "recorderToolStripMenuItem";
            this.recorderToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.recorderToolStripMenuItem.Text = "Recorder";
            // 
            // startStopRecordingToolStripMenuItem
            // 
            this.startStopRecordingToolStripMenuItem.Name = "startStopRecordingToolStripMenuItem";
            this.startStopRecordingToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.startStopRecordingToolStripMenuItem.Text = "Start Recording";
            this.startStopRecordingToolStripMenuItem.Click += new System.EventHandler(this.startStopRecordingToolStripMenuItem_Click);
            // 
            // playRecordingToolStripMenuItem
            // 
            this.playRecordingToolStripMenuItem.Name = "playRecordingToolStripMenuItem";
            this.playRecordingToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.playRecordingToolStripMenuItem.Text = "Play Recording";
            this.playRecordingToolStripMenuItem.Click += new System.EventHandler(this.playRecordingToolStripMenuItem_Click);
            // 
            // XYZLabel
            // 
            this.XYZLabel.AutoSize = true;
            this.XYZLabel.Location = new System.Drawing.Point(12, 211);
            this.XYZLabel.Name = "XYZLabel";
            this.XYZLabel.Size = new System.Drawing.Size(34, 13);
            this.XYZLabel.TabIndex = 21;
            this.XYZLabel.Text = "X Y Z";
            // 
            // XYZTextBox
            // 
            this.XYZTextBox.AcceptsReturn = true;
            this.XYZTextBox.AcceptsTab = true;
            this.XYZTextBox.Location = new System.Drawing.Point(12, 227);
            this.XYZTextBox.Name = "XYZTextBox";
            this.XYZTextBox.Size = new System.Drawing.Size(120, 20);
            this.XYZTextBox.TabIndex = 22;
            // 
            // AddXYZButton
            // 
            this.AddXYZButton.Location = new System.Drawing.Point(12, 292);
            this.AddXYZButton.Name = "AddXYZButton";
            this.AddXYZButton.Size = new System.Drawing.Size(75, 23);
            this.AddXYZButton.TabIndex = 23;
            this.AddXYZButton.TabStop = false;
            this.AddXYZButton.Text = "Add Position";
            this.AddXYZButton.UseVisualStyleBackColor = true;
            this.AddXYZButton.Click += new System.EventHandler(this.addXYZButton_Click);
            // 
            // delayLabel
            // 
            this.delayLabel.AutoSize = true;
            this.delayLabel.Location = new System.Drawing.Point(12, 318);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(56, 13);
            this.delayLabel.TabIndex = 24;
            this.delayLabel.Text = "Delay (ms)";
            // 
            // delayTextBox
            // 
            this.delayTextBox.AcceptsReturn = true;
            this.delayTextBox.AcceptsTab = true;
            this.delayTextBox.Location = new System.Drawing.Point(12, 334);
            this.delayTextBox.Name = "delayTextBox";
            this.delayTextBox.Size = new System.Drawing.Size(120, 20);
            this.delayTextBox.TabIndex = 24;
            // 
            // timeBox
            // 
            this.timeBox.AcceptsReturn = true;
            this.timeBox.AcceptsTab = true;
            this.timeBox.Location = new System.Drawing.Point(12, 266);
            this.timeBox.Name = "timeBox";
            this.timeBox.Size = new System.Drawing.Size(120, 20);
            this.timeBox.TabIndex = 23;
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(12, 250);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(52, 13);
            this.timeLabel.TabIndex = 27;
            this.timeLabel.Text = "Time (ms)";
            // 
            // commandChain
            // 
            this.commandChain.Location = new System.Drawing.Point(138, 58);
            this.commandChain.Multiline = true;
            this.commandChain.Name = "commandChain";
            this.commandChain.Size = new System.Drawing.Size(370, 296);
            this.commandChain.TabIndex = 0;
            this.commandChain.Tag = "";
            // 
            // addDelayButton
            // 
            this.addDelayButton.Location = new System.Drawing.Point(12, 360);
            this.addDelayButton.Name = "addDelayButton";
            this.addDelayButton.Size = new System.Drawing.Size(75, 23);
            this.addDelayButton.TabIndex = 28;
            this.addDelayButton.TabStop = false;
            this.addDelayButton.Text = "Add Delay";
            this.addDelayButton.UseVisualStyleBackColor = true;
            this.addDelayButton.Click += new System.EventHandler(this.addDelayButton_Click);
            // 
            // MyoSimulatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 393);
            this.Controls.Add(this.addDelayButton);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.timeBox);
            this.Controls.Add(this.delayTextBox);
            this.Controls.Add(this.delayLabel);
            this.Controls.Add(this.AddXYZButton);
            this.Controls.Add(this.XYZTextBox);
            this.Controls.Add(this.XYZLabel);
            this.Controls.Add(this.gestureList);
            this.Controls.Add(this.addGestureButton);
            this.Controls.Add(this.gestureLabel);
            this.Controls.Add(this.sendCommandButton);
            this.Controls.Add(this.commandChain);
            this.Controls.Add(this.menu);
            this.MainMenuStrip = this.menu;
            this.Name = "MyoSimulatorForm";
            this.Text = "Myo Simulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MyoSimulatorForm_FormClosing);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button sendCommandButton;
        private System.Windows.Forms.Label gestureLabel;
        private System.Windows.Forms.Button addGestureButton;
        private System.Windows.Forms.ListBox gestureList;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recorderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startStopRecordingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playRecordingToolStripMenuItem;
        private System.Windows.Forms.Label XYZLabel;
        private System.Windows.Forms.TextBox XYZTextBox;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.TextBox timeBox;
        private System.Windows.Forms.Button AddXYZButton;
        private System.Windows.Forms.Label delayLabel;
        private System.Windows.Forms.TextBox delayTextBox;
        private System.Windows.Forms.TextBox commandChain;
        private System.Windows.Forms.Button addDelayButton;
    }
}

