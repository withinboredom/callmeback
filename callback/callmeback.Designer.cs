namespace callback
{
    partial class callmeback
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
            this.OptionsButton = new System.Windows.Forms.Button();
            this.ActivatedBox = new System.Windows.Forms.CheckBox();
            this.QuitButton = new System.Windows.Forms.Button();
            this.AboutButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lightRecord = new System.Windows.Forms.RadioButton();
            this.lightCall = new System.Windows.Forms.RadioButton();
            this.lightSend = new System.Windows.Forms.RadioButton();
            this.lightCheck = new System.Windows.Forms.RadioButton();
            this.lightStandby = new System.Windows.Forms.RadioButton();
            this.emailChecker = new System.Windows.Forms.Timer(this.components);
            this.statusChecker = new System.Windows.Forms.Timer(this.components);
            this.Resend = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OptionsButton
            // 
            this.OptionsButton.Location = new System.Drawing.Point(12, 12);
            this.OptionsButton.Name = "OptionsButton";
            this.OptionsButton.Size = new System.Drawing.Size(75, 23);
            this.OptionsButton.TabIndex = 0;
            this.OptionsButton.Text = "Options";
            this.OptionsButton.UseVisualStyleBackColor = true;
            this.OptionsButton.Click += new System.EventHandler(this.OptionsButton_Click);
            // 
            // ActivatedBox
            // 
            this.ActivatedBox.AutoSize = true;
            this.ActivatedBox.Location = new System.Drawing.Point(12, 41);
            this.ActivatedBox.Name = "ActivatedBox";
            this.ActivatedBox.Size = new System.Drawing.Size(71, 17);
            this.ActivatedBox.TabIndex = 1;
            this.ActivatedBox.Text = "Activated";
            this.ActivatedBox.UseVisualStyleBackColor = true;
            this.ActivatedBox.CheckedChanged += new System.EventHandler(this.ActivatedBox_CheckedChanged);
            // 
            // QuitButton
            // 
            this.QuitButton.Location = new System.Drawing.Point(12, 64);
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.Size = new System.Drawing.Size(75, 23);
            this.QuitButton.TabIndex = 2;
            this.QuitButton.Text = "Quit";
            this.QuitButton.UseVisualStyleBackColor = true;
            this.QuitButton.Click += new System.EventHandler(this.QuitButton_Click);
            // 
            // AboutButton
            // 
            this.AboutButton.Location = new System.Drawing.Point(12, 93);
            this.AboutButton.Name = "AboutButton";
            this.AboutButton.Size = new System.Drawing.Size(75, 23);
            this.AboutButton.TabIndex = 3;
            this.AboutButton.Text = "About";
            this.AboutButton.UseVisualStyleBackColor = true;
            this.AboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lightRecord);
            this.groupBox1.Controls.Add(this.lightCall);
            this.groupBox1.Controls.Add(this.lightSend);
            this.groupBox1.Controls.Add(this.lightCheck);
            this.groupBox1.Controls.Add(this.lightStandby);
            this.groupBox1.Location = new System.Drawing.Point(93, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(234, 145);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Call Status";
            // 
            // lightRecord
            // 
            this.lightRecord.AutoSize = true;
            this.lightRecord.Location = new System.Drawing.Point(6, 111);
            this.lightRecord.Name = "lightRecord";
            this.lightRecord.Size = new System.Drawing.Size(74, 17);
            this.lightRecord.TabIndex = 4;
            this.lightRecord.TabStop = true;
            this.lightRecord.Text = "Recording";
            this.lightRecord.UseVisualStyleBackColor = true;
            this.lightRecord.CheckedChanged += new System.EventHandler(this.lightRecord_CheckedChanged);
            // 
            // lightCall
            // 
            this.lightCall.AutoSize = true;
            this.lightCall.Location = new System.Drawing.Point(6, 88);
            this.lightCall.Name = "lightCall";
            this.lightCall.Size = new System.Drawing.Size(56, 17);
            this.lightCall.TabIndex = 3;
            this.lightCall.TabStop = true;
            this.lightCall.Text = "Calling";
            this.lightCall.UseVisualStyleBackColor = true;
            this.lightCall.CheckedChanged += new System.EventHandler(this.lightCall_CheckedChanged);
            // 
            // lightSend
            // 
            this.lightSend.AutoSize = true;
            this.lightSend.Location = new System.Drawing.Point(6, 65);
            this.lightSend.Name = "lightSend";
            this.lightSend.Size = new System.Drawing.Size(92, 17);
            this.lightSend.TabIndex = 2;
            this.lightSend.TabStop = true;
            this.lightSend.Text = "Sending Email";
            this.lightSend.UseVisualStyleBackColor = true;
            this.lightSend.CheckedChanged += new System.EventHandler(this.lightSend_CheckedChanged);
            // 
            // lightCheck
            // 
            this.lightCheck.AutoSize = true;
            this.lightCheck.Location = new System.Drawing.Point(6, 42);
            this.lightCheck.Name = "lightCheck";
            this.lightCheck.Size = new System.Drawing.Size(98, 17);
            this.lightCheck.TabIndex = 1;
            this.lightCheck.TabStop = true;
            this.lightCheck.Text = "Checking Email";
            this.lightCheck.UseVisualStyleBackColor = true;
            this.lightCheck.CheckedChanged += new System.EventHandler(this.lightCheck_CheckedChanged);
            // 
            // lightStandby
            // 
            this.lightStandby.AutoSize = true;
            this.lightStandby.Location = new System.Drawing.Point(6, 19);
            this.lightStandby.Name = "lightStandby";
            this.lightStandby.Size = new System.Drawing.Size(64, 17);
            this.lightStandby.TabIndex = 0;
            this.lightStandby.TabStop = true;
            this.lightStandby.Text = "Standby";
            this.lightStandby.UseVisualStyleBackColor = true;
            this.lightStandby.CheckedChanged += new System.EventHandler(this.lightStandby_CheckedChanged);
            // 
            // emailChecker
            // 
            this.emailChecker.Tick += new System.EventHandler(this.emailChecker_Tick);
            // 
            // statusChecker
            // 
            this.statusChecker.Enabled = true;
            this.statusChecker.Interval = 30;
            this.statusChecker.Tick += new System.EventHandler(this.statusChecker_Tick);
            // 
            // Resend
            // 
            this.Resend.Enabled = true;
            this.Resend.Interval = 5000;
            this.Resend.Tick += new System.EventHandler(this.Resend_Tick);
            // 
            // callmeback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 175);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.AboutButton);
            this.Controls.Add(this.QuitButton);
            this.Controls.Add(this.ActivatedBox);
            this.Controls.Add(this.OptionsButton);
            this.Name = "callmeback";
            this.Text = "Call Back";
            this.Load += new System.EventHandler(this.callmeback_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OptionsButton;
        private System.Windows.Forms.CheckBox ActivatedBox;
        private System.Windows.Forms.Button QuitButton;
        private System.Windows.Forms.Button AboutButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton lightSend;
        private System.Windows.Forms.RadioButton lightCheck;
        private System.Windows.Forms.RadioButton lightStandby;
        private System.Windows.Forms.RadioButton lightRecord;
        private System.Windows.Forms.RadioButton lightCall;
        private System.Windows.Forms.Timer emailChecker;
        private System.Windows.Forms.Timer statusChecker;
        private System.Windows.Forms.Timer Resend;
    }
}