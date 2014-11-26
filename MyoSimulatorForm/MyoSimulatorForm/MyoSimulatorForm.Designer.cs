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
            this.scriptPath = new System.Windows.Forms.TextBox();
            this.commandChain = new System.Windows.Forms.TextBox();
            this.gestureLabel = new System.Windows.Forms.Label();
            this.addGestureButton = new System.Windows.Forms.Button();
            this.gestureList = new System.Windows.Forms.ListBox();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recorderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startRecordingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playRecordingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runScriptButton = new System.Windows.Forms.Button();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // sendCommandButton
            // 
            this.sendCommandButton.Location = new System.Drawing.Point(430, 272);
            this.sendCommandButton.Name = "sendCommandButton";
            this.sendCommandButton.Size = new System.Drawing.Size(75, 23);
            this.sendCommandButton.TabIndex = 9;
            this.sendCommandButton.Text = "Send";
            this.sendCommandButton.UseVisualStyleBackColor = true;
            this.sendCommandButton.Click += new System.EventHandler(this.sendCommandButton_Click);
            // 
            // scriptPath
            // 
            this.scriptPath.Location = new System.Drawing.Point(12, 245);
            this.scriptPath.Name = "scriptPath";
            this.scriptPath.ReadOnly = true;
            this.scriptPath.Size = new System.Drawing.Size(382, 20);
            this.scriptPath.TabIndex = 11;
            // 
            // commandChain
            // 
            this.commandChain.Location = new System.Drawing.Point(12, 275);
            this.commandChain.Name = "commandChain";
            this.commandChain.ReadOnly = true;
            this.commandChain.Size = new System.Drawing.Size(382, 20);
            this.commandChain.TabIndex = 0;
            this.commandChain.Tag = "";
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
            // 
            // recorderToolStripMenuItem
            // 
            this.recorderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startRecordingToolStripMenuItem,
            this.playRecordingToolStripMenuItem});
            this.recorderToolStripMenuItem.Name = "recorderToolStripMenuItem";
            this.recorderToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.recorderToolStripMenuItem.Text = "Recorder";
            // 
            // startRecordingToolStripMenuItem
            // 
            this.startRecordingToolStripMenuItem.Name = "startRecordingToolStripMenuItem";
            this.startRecordingToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.startRecordingToolStripMenuItem.Text = "Start Recording";
            this.startRecordingToolStripMenuItem.Click += new System.EventHandler(this.startRecordingToolStripMenuItem_Click);
            // 
            // playRecordingToolStripMenuItem
            // 
            this.playRecordingToolStripMenuItem.Name = "playRecordingToolStripMenuItem";
            this.playRecordingToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.playRecordingToolStripMenuItem.Text = "Play Recording";
            this.playRecordingToolStripMenuItem.Click += new System.EventHandler(this.playRecordingToolStripMenuItem_Click);
            // 
            // runScriptButton
            // 
            this.runScriptButton.Location = new System.Drawing.Point(430, 245);
            this.runScriptButton.Name = "runScriptButton";
            this.runScriptButton.Size = new System.Drawing.Size(75, 23);
            this.runScriptButton.TabIndex = 21;
            this.runScriptButton.Text = "Run Script";
            this.runScriptButton.UseVisualStyleBackColor = true;
            this.runScriptButton.Click += new System.EventHandler(this.runScriptButton_Click);
            // 
            // MyoSimulatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 307);
            this.Controls.Add(this.runScriptButton);
            this.Controls.Add(this.gestureList);
            this.Controls.Add(this.addGestureButton);
            this.Controls.Add(this.gestureLabel);
            this.Controls.Add(this.scriptPath);
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
        private System.Windows.Forms.TextBox scriptPath;
        private System.Windows.Forms.TextBox commandChain;
        private System.Windows.Forms.Label gestureLabel;
        private System.Windows.Forms.Button addGestureButton;
        private System.Windows.Forms.ListBox gestureList;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recorderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startRecordingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playRecordingToolStripMenuItem;
        private System.Windows.Forms.Button runScriptButton;

    }
}

