
namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.rest_button = new System.Windows.Forms.Button();
            this.fist_button = new System.Windows.Forms.Button();
            this.waveIn_button = new System.Windows.Forms.Button();
            this.waveOut_button = new System.Windows.Forms.Button();
            this.fingersSpread_button = new System.Windows.Forms.Button();
            this.reserved1_button = new System.Windows.Forms.Button();
            this.thumbToPinky_button = new System.Windows.Forms.Button();
            this.unknown_button = new System.Windows.Forms.Button();
            this.send_command_button = new System.Windows.Forms.Button();
            this.save_button = new System.Windows.Forms.Button();
            this.save_filename = new System.Windows.Forms.TextBox();
            this.command_chain = new System.Windows.Forms.TextBox();
            this.load_file_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rest_button
            // 
            this.rest_button.Location = new System.Drawing.Point(12, 12);
            this.rest_button.Name = "rest_button";
            this.rest_button.Size = new System.Drawing.Size(75, 23);
            this.rest_button.TabIndex = 1;
            this.rest_button.Text = "Rest";
            this.rest_button.UseVisualStyleBackColor = true;
            this.rest_button.Click += new System.EventHandler(this.rest_button_Click);
            // 
            // fist_button
            // 
            this.fist_button.Location = new System.Drawing.Point(93, 12);
            this.fist_button.Name = "fist_button";
            this.fist_button.Size = new System.Drawing.Size(75, 23);
            this.fist_button.TabIndex = 2;
            this.fist_button.Text = "Fist";
            this.fist_button.UseVisualStyleBackColor = true;
            this.fist_button.Click += new System.EventHandler(this.fist_button_Click);
            // 
            // waveIn_button
            // 
            this.waveIn_button.Location = new System.Drawing.Point(174, 12);
            this.waveIn_button.Name = "waveIn_button";
            this.waveIn_button.Size = new System.Drawing.Size(75, 23);
            this.waveIn_button.TabIndex = 3;
            this.waveIn_button.Text = "Wave In";
            this.waveIn_button.UseVisualStyleBackColor = true;
            this.waveIn_button.Click += new System.EventHandler(this.waveIn_button_Click);
            // 
            // waveOut_button
            // 
            this.waveOut_button.Location = new System.Drawing.Point(255, 12);
            this.waveOut_button.Name = "waveOut_button";
            this.waveOut_button.Size = new System.Drawing.Size(75, 23);
            this.waveOut_button.TabIndex = 4;
            this.waveOut_button.Text = "Wave Out";
            this.waveOut_button.UseVisualStyleBackColor = true;
            this.waveOut_button.Click += new System.EventHandler(this.waveOut_button_Click);
            // 
            // fingersSpread_button
            // 
            this.fingersSpread_button.Location = new System.Drawing.Point(336, 12);
            this.fingersSpread_button.Name = "fingersSpread_button";
            this.fingersSpread_button.Size = new System.Drawing.Size(88, 23);
            this.fingersSpread_button.TabIndex = 5;
            this.fingersSpread_button.Text = "Fingers Spread";
            this.fingersSpread_button.UseVisualStyleBackColor = true;
            this.fingersSpread_button.Click += new System.EventHandler(this.fingersSpread_button_Click);
            // 
            // reserved1_button
            // 
            this.reserved1_button.Location = new System.Drawing.Point(430, 12);
            this.reserved1_button.Name = "reserved1_button";
            this.reserved1_button.Size = new System.Drawing.Size(75, 23);
            this.reserved1_button.TabIndex = 6;
            this.reserved1_button.Text = "Reserved 1";
            this.reserved1_button.UseVisualStyleBackColor = true;
            this.reserved1_button.Click += new System.EventHandler(this.reserved1_button_Click);
            // 
            // thumbToPinky_button
            // 
            this.thumbToPinky_button.Location = new System.Drawing.Point(12, 41);
            this.thumbToPinky_button.Name = "thumbToPinky_button";
            this.thumbToPinky_button.Size = new System.Drawing.Size(80, 23);
            this.thumbToPinky_button.TabIndex = 7;
            this.thumbToPinky_button.Text = "Thumb2Pinky";
            this.thumbToPinky_button.UseVisualStyleBackColor = true;
            this.thumbToPinky_button.Click += new System.EventHandler(this.thumbToPinky_button_Click);
            // 
            // unknown_button
            // 
            this.unknown_button.Location = new System.Drawing.Point(98, 41);
            this.unknown_button.Name = "unknown_button";
            this.unknown_button.Size = new System.Drawing.Size(75, 23);
            this.unknown_button.TabIndex = 8;
            this.unknown_button.Text = "Unknown";
            this.unknown_button.UseVisualStyleBackColor = true;
            this.unknown_button.Click += new System.EventHandler(this.unknown_button_Click);
            // 
            // send_command_button
            // 
            this.send_command_button.Location = new System.Drawing.Point(430, 272);
            this.send_command_button.Name = "send_command_button";
            this.send_command_button.Size = new System.Drawing.Size(75, 23);
            this.send_command_button.TabIndex = 9;
            this.send_command_button.Text = "Send";
            this.send_command_button.UseVisualStyleBackColor = true;
            this.send_command_button.Click += new System.EventHandler(this.send_command_button_Click);
            // 
            // save_button
            // 
            this.save_button.Location = new System.Drawing.Point(430, 243);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(75, 23);
            this.save_button.TabIndex = 10;
            this.save_button.Text = "Save";
            this.save_button.UseVisualStyleBackColor = true;
            this.save_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // save_filename
            // 
            this.save_filename.Location = new System.Drawing.Point(12, 245);
            this.save_filename.Name = "save_filename";
            this.save_filename.Size = new System.Drawing.Size(382, 20);
            this.save_filename.TabIndex = 11;
            // 
            // command_chain
            // 
            this.command_chain.Location = new System.Drawing.Point(12, 275);
            this.command_chain.Name = "command_chain";
            this.command_chain.ReadOnly = true;
            this.command_chain.Size = new System.Drawing.Size(382, 20);
            this.command_chain.TabIndex = 0;
            this.command_chain.Tag = "";
            // 
            // load_file_button
            // 
            this.load_file_button.Location = new System.Drawing.Point(430, 214);
            this.load_file_button.Name = "load_file_button";
            this.load_file_button.Size = new System.Drawing.Size(75, 23);
            this.load_file_button.TabIndex = 12;
            this.load_file_button.Text = "Load";
            this.load_file_button.UseVisualStyleBackColor = true;
            this.load_file_button.Click += new System.EventHandler(this.load_file_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 307);
            this.Controls.Add(this.load_file_button);
            this.Controls.Add(this.save_filename);
            this.Controls.Add(this.save_button);
            this.Controls.Add(this.send_command_button);
            this.Controls.Add(this.unknown_button);
            this.Controls.Add(this.thumbToPinky_button);
            this.Controls.Add(this.reserved1_button);
            this.Controls.Add(this.fingersSpread_button);
            this.Controls.Add(this.waveOut_button);
            this.Controls.Add(this.waveIn_button);
            this.Controls.Add(this.fist_button);
            this.Controls.Add(this.rest_button);
            this.Controls.Add(this.command_chain);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button rest_button;
        private System.Windows.Forms.Button fist_button;
        private System.Windows.Forms.Button waveIn_button;
        private System.Windows.Forms.Button waveOut_button;
        private System.Windows.Forms.Button fingersSpread_button;
        private System.Windows.Forms.Button reserved1_button;
        private System.Windows.Forms.Button thumbToPinky_button;
        private System.Windows.Forms.Button unknown_button;
        private System.Windows.Forms.Button send_command_button;
        private System.Windows.Forms.Button save_button;
        private System.Windows.Forms.TextBox save_filename;
        private System.Windows.Forms.TextBox command_chain;
        private System.Windows.Forms.Button load_file_button;

    }
}

