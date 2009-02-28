using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace callback
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            Settings1 settings = new Settings1();
            settings.Reload();

            textBoxInterval.Text = settings.interval.ToString();
            textBoxName.Text = settings.username;
            textBoxPassword.Text = settings.password;
            textBoxServer.Text = settings.pop3;
            textBoxSubject.Text = settings.subject;

            checkBoxChain.Checked = settings.chain;
            checkBoxRecord.Checked = settings.record;
            checkBoxSave.Checked = !settings.deleteAfterSending;
            checkBoxSavePass.Checked = settings.savepass;
            checkBoxSend.Checked = settings.sendRecordingToSender;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //save the settings
            Settings1 settings = new Settings1();
            settings.interval = int.Parse(textBoxInterval.Text);
            settings.username = textBoxName.Text;
            settings.password = textBoxPassword.Text;
            settings.pop3 = textBoxServer.Text;
            settings.subject = textBoxSubject.Text;

            settings.chain = checkBoxChain.Checked;
            settings.record = checkBoxRecord.Checked;
            settings.deleteAfterSending = !checkBoxSave.Checked;
            settings.savepass = checkBoxSavePass.Checked;
            settings.sendRecordingToSender = checkBoxSend.Checked;

            settings.Save();

            this.Close();
        }
    }
}
