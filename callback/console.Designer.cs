namespace callback
{
    partial class console
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
            this.waitToConnect = new System.Windows.Forms.Timer(this.components);
            this.TxtPhone = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ChkActivate = new System.Windows.Forms.CheckBox();
            this.OnCall = new System.Windows.Forms.Timer(this.components);
            this.HangUp = new System.Windows.Forms.Timer(this.components);
            this.CheckMail = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ssl = new System.Windows.Forms.CheckBox();
            this.passSave = new System.Windows.Forms.CheckBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // waitToConnect
            // 
            this.waitToConnect.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // TxtPhone
            // 
            this.TxtPhone.Location = new System.Drawing.Point(134, 9);
            this.TxtPhone.Name = "TxtPhone";
            this.TxtPhone.Size = new System.Drawing.Size(100, 20);
            this.TxtPhone.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Email Address to Check";
            // 
            // ChkActivate
            // 
            this.ChkActivate.AutoSize = true;
            this.ChkActivate.Location = new System.Drawing.Point(240, 12);
            this.ChkActivate.Name = "ChkActivate";
            this.ChkActivate.Size = new System.Drawing.Size(71, 17);
            this.ChkActivate.TabIndex = 3;
            this.ChkActivate.Text = "Activated";
            this.ChkActivate.UseVisualStyleBackColor = true;
            this.ChkActivate.CheckedChanged += new System.EventHandler(this.ChkActivate_CheckedChanged);
            // 
            // OnCall
            // 
            this.OnCall.Enabled = true;
            this.OnCall.Tick += new System.EventHandler(this.OnCall_Tick);
            // 
            // HangUp
            // 
            this.HangUp.Interval = 60000;
            this.HangUp.Tick += new System.EventHandler(this.HangUp_Tick);
            // 
            // CheckMail
            // 
            this.CheckMail.Interval = 30000;
            this.CheckMail.Tick += new System.EventHandler(this.CheckMail_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(134, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 5;
            // 
            // ssl
            // 
            this.ssl.AutoSize = true;
            this.ssl.Location = new System.Drawing.Point(240, 38);
            this.ssl.Name = "ssl";
            this.ssl.Size = new System.Drawing.Size(74, 17);
            this.ssl.TabIndex = 6;
            this.ssl.Text = "Use SSL?";
            this.ssl.UseVisualStyleBackColor = true;
            this.ssl.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // passSave
            // 
            this.passSave.AutoSize = true;
            this.passSave.Location = new System.Drawing.Point(101, 61);
            this.passSave.Name = "passSave";
            this.passSave.Size = new System.Drawing.Size(106, 17);
            this.passSave.TabIndex = 7;
            this.passSave.Text = "Save Password?";
            this.passSave.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(134, 84);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(62, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Pop3 Server";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Enabled = false;
            this.radioButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton1.Location = new System.Drawing.Point(213, 61);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(97, 17);
            this.radioButton1.TabIndex = 10;
            this.radioButton1.Text = "Checking Email";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.BackColor = System.Drawing.SystemColors.Control;
            this.radioButton2.Enabled = false;
            this.radioButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton2.Location = new System.Drawing.Point(10, 61);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(73, 17);
            this.radioButton2.TabIndex = 11;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Recording";
            this.radioButton2.UseVisualStyleBackColor = false;
            // 
            // console
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 136);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.passSave);
            this.Controls.Add(this.ssl);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ChkActivate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TxtPhone);
            this.Name = "console";
            this.Text = "console";
            this.Load += new System.EventHandler(this.console_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.onClose);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer waitToConnect;
        private System.Windows.Forms.TextBox TxtPhone;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ChkActivate;
        private System.Windows.Forms.Timer OnCall;
        private System.Windows.Forms.Timer HangUp;
        private System.Windows.Forms.Timer CheckMail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox ssl;
        private System.Windows.Forms.CheckBox passSave;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
    }
}