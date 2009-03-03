using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pop3;
using Smtp;
using Smtp.Net;
using SKYPE4COMLib;

namespace callback
{
    public partial class callmeback : Form
    {
        public callmeback()
        {
            InitializeComponent();
        }

        private void OptionsButton_Click(object sender, EventArgs e)
        {
            ActivatedBox.Checked = false;

            Options optionsbox = new Options();

            optionsbox.ShowDialog();
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.Show();
        }

        private bool activated = false;

        private void ActivatedBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!activated)
            {
                Settings1 settings = new Settings1();
                settings.Reload();
                emailChecker.Interval = settings.interval;
                emailChecker.Enabled = true;
                activated = true;
                pop3client = new Pop3Client(settings.username, settings.password, settings.pop3);
                OptionsButton.Enabled = false;
            }
            else
            {
                //deactivate
                emailChecker.Enabled = false;
                OptionsButton.Enabled = true;
            }
        }

        private Caller skype;

        ArrayList numbers = null;

        private void emailChecker_Tick(object sender, EventArgs e)
        {
            //check email
            if (pop3client != null)
            {
                Settings1 settings = new Settings1();
                settings.Reload();
                lightStandby.Checked = false;
                lightCheck.Checked = true;
                pop3client.OpenInbox();

                while (pop3client.NextEmail())
                {
                    numbers = pop3client.HarvestPhoneNumbers(settings.subject);
                    if (numbers != null)
                    {
                        //call list and exit function
                        skype.callList(numbers);
                        lightStandby.Checked = false;
                        lightCall.Checked = true;
                        lightCheck.Checked = false;
                        emailChecker.Enabled = false;
                        skype.placeBlockedCall(this.sender);
                        this.sender = pop3client.From;
                        pop3client.DeleteEmail();
                        pop3client.CloseConnection();
                        SendStatusMail(this.sender);
                        SendCallReport();
                        return;
                    }
                }
                lightStandby.Checked = true;
                lightCheck.Checked = false;
                pop3client.CloseConnection();
            }
        }

        private void SendCallReport()
        {
            string body = "Call to:\r\n";

            foreach (string number in numbers)
            {
                body += number + "\r\n";
            }

            body += "From Email Address: " + this.sender;

            EmailSender.Send("callback@rlanders.com", "callback@rlanders.com", "Call Report", body);
        }

        private Pop3Client pop3client = null;
        private string sender = "";

        private void callmeback_Load(object sender, EventArgs e)
        {
            skype = new Caller();
            lightStandby.Checked = true;
        }

        public void alldone()
        {
        }

        private void lightStandby_CheckedChanged(object sender, EventArgs e)
        {
            //lightStandby.Checked = false;
        }

        private void lightCheck_CheckedChanged(object sender, EventArgs e)
        {
            //lightCheck.Checked = false;
        }

        private void lightSend_CheckedChanged(object sender, EventArgs e)
        {
            //lightSend.Checked = false;
        }

        private void lightCall_CheckedChanged(object sender, EventArgs e)
        {
            //lightCall.Checked = false;
        }

        private void lightRecord_CheckedChanged(object sender, EventArgs e)
        {
            //lightRecord.Checked = false;
        }

        private void SendAttachment(string to, string attachment)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.From = new System.Net.Mail.MailAddress("callback@rlanders.com");
            message.To.Add(to);
            try
            {
                message.Attachments.Add(new System.Net.Mail.Attachment(attachment));
                EmailSender.Send(message);
            }
            catch (Exception ex)
            {
                resender(to, attachment);
            }
        }

        private void statusChecker_Tick(object sender, EventArgs e)
        {
            if (skype.GoodToGo)
            {
                Settings1 settings = new Settings1();
                settings.Reload();
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.From = new System.Net.Mail.MailAddress("callback@rlanders.com");
                message.To.Add(this.sender);
                //message.Attachments.Add(new System.Net.Mail.Attachment(skype.
                foreach (string attachment in skype.Recordings)
                {
                    try
                    {
                        message.Attachments.Add(new System.Net.Mail.Attachment(attachment));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error with adding of attachment: " + ex.ToString());
                        message.Body += "There was an error uploading the attached recordings.\r\nThe following files will be resent: \r\n";
                        foreach (string attach in skype.Recordings)
                        {
                            message.Body += attach + "\r\n";
                        }
                        continue;
                    }
                }
                try
                {
                    if (skype.Recordings.Count == 0)
                    {
                        message.Body += "For some reason the call did not go through, please resend your email\r\n";
                        message.Body += "\r\n\tTried Calling the following numbers: \r\n";
                        foreach (string number in numbers)
                        {
                            message.Body += number + "\r\n";
                        }
                    }
                    EmailSender.Send(message);
                }
                catch
                {
                    //more than likely skype was just used for a legit reason... so silently fail
                }
                numbers.Clear();
                emailChecker.Enabled = true;
                skype.unblockCall();
            }
        }

        private void SendStatusMail(string sender)
        {
            Settings1 settings = new Settings1();
            settings.Reload();
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.From = new System.Net.Mail.MailAddress("callback@rlanders.com");
            message.To.Add(sender);
            this.sender = sender;
            message.Body = "Call 2 in motion - standby";
            EmailSender.Send(message);
        }

        string resendList = "";

        private void resender(string to, string attachment)
        {
            resendList = "~" + to + "=" + attachment + resendList;
            Console.WriteLine(resendList);
        }

        private void Resend_Tick(object sender, EventArgs e)
        {
            while (resendList.Length > 0)
            {
                string attach = resendList.Substring(resendList.LastIndexOf("=") + 1);
                resendList = resendList.Remove(resendList.LastIndexOf("="));
                Console.WriteLine("attach: " + attach);
                string to = resendList.Substring(resendList.LastIndexOf("~") + 1);
                resendList = resendList.Remove(resendList.LastIndexOf("~"));
                Console.WriteLine("send above to: " + to);

                SendAttachment(to, attach);
            }
        }
    }
}
