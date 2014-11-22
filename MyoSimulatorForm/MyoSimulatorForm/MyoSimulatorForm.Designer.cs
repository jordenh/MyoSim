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
            this.saveButton = new System.Windows.Forms.Button();
            this.saveFilename = new System.Windows.Forms.TextBox();
            this.commandChain = new System.Windows.Forms.TextBox();
            this.loadFileButton = new System.Windows.Forms.Button();
            this.gestureLabel = new System.Windows.Forms.Label();
            this.addGestureButton = new System.Windows.Forms.Button();
            this.gestureList = new System.Windows.Forms.ListBox();
            this.readTestButton = new System.Windows.Forms.Button();
            this.writeTestButton = new System.Windows.Forms.Button();
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
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(430, 243);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 10;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // saveFilename
            // 
            this.saveFilename.Location = new System.Drawing.Point(12, 245);
            this.saveFilename.Name = "saveFilename";
            this.saveFilename.Size = new System.Drawing.Size(382, 20);
            this.saveFilename.TabIndex = 11;
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
            // loadFileButton
            // 
            this.loadFileButton.Location = new System.Drawing.Point(430, 214);
            this.loadFileButton.Name = "loadFileButton";
            this.loadFileButton.Size = new System.Drawing.Size(75, 23);
            this.loadFileButton.TabIndex = 12;
            this.loadFileButton.Text = "Load";
            this.loadFileButton.UseVisualStyleBackColor = true;
            this.loadFileButton.Click += new System.EventHandler(this.loadFileButton_Click);
            // 
            // gestureLabel
            // 
            this.gestureLabel.AutoSize = true;
            this.gestureLabel.Location = new System.Drawing.Point(12, 19);
            this.gestureLabel.Name = "gestureLabel";
            this.gestureLabel.Size = new System.Drawing.Size(77, 13);
            this.gestureLabel.TabIndex = 15;
            this.gestureLabel.Text = "Select Gesture";
            // 
            // addGestureButton
            // 
            this.addGestureButton.Location = new System.Drawing.Point(141, 83);
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
            this.gestureList.Location = new System.Drawing.Point(15, 35);
            this.gestureList.Name = "gestureList";
            this.gestureList.Size = new System.Drawing.Size(120, 121);
            this.gestureList.TabIndex = 17;
            // 
            // readTestButton
            // 
            this.readTestButton.Location = new System.Drawing.Point(430, 185);
            this.readTestButton.Name = "readTestButton";
            this.readTestButton.Size = new System.Drawing.Size(75, 23);
            this.readTestButton.TabIndex = 18;
            this.readTestButton.Text = "Read Test";
            this.readTestButton.UseVisualStyleBackColor = true;
            this.readTestButton.Click += new System.EventHandler(this.readTestButton_Click);
            // 
            // writeTestButton
            // 
            this.writeTestButton.Location = new System.Drawing.Point(430, 156);
            this.writeTestButton.Name = "writeTestButton";
            this.writeTestButton.Size = new System.Drawing.Size(75, 23);
            this.writeTestButton.TabIndex = 19;
            this.writeTestButton.Text = "Write Test";
            this.writeTestButton.UseVisualStyleBackColor = true;
            this.writeTestButton.Click += new System.EventHandler(this.writeTestButton_Click);
            // 
            // MyoSimulatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 307);
            this.Controls.Add(this.writeTestButton);
            this.Controls.Add(this.readTestButton);
            this.Controls.Add(this.gestureList);
            this.Controls.Add(this.addGestureButton);
            this.Controls.Add(this.gestureLabel);
            this.Controls.Add(this.loadFileButton);
            this.Controls.Add(this.saveFilename);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.sendCommandButton);
            this.Controls.Add(this.commandChain);
            this.Name = "MyoSimulatorForm";
            this.Text = "Myo Simulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MyoSimulatorForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button sendCommandButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TextBox saveFilename;
        private System.Windows.Forms.TextBox commandChain;
        private System.Windows.Forms.Button loadFileButton;
        private System.Windows.Forms.Label gestureLabel;
        private System.Windows.Forms.Button addGestureButton;
        private System.Windows.Forms.ListBox gestureList;
        private System.Windows.Forms.Button readTestButton;
        private System.Windows.Forms.Button writeTestButton;

    }
}

