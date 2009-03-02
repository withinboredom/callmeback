using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SKYPE4COMLib;
using ANPOPLib;
using AOSMTPLib;

namespace callback
{
    public partial class console : Form
    {
        public console()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        Skype oSkype = new Skype();
        Conversion converter = new Conversion();
        string phone = "+19313024501";
        List<string> callers = new List<string>();
        Call currentcall;
        List<string> recordings = new List<string>();
        string returnmessage;
        POPMAINClass pop3 = new POPMAINClass();
        POPMSGClass msg = new POPMSGClass();
        MSGSTOREClass store = new MSGSTOREClass();

        private void Connect(string popServer, string user, string password)
        {
            Console.WriteLine("Connecting to email account");
            try
            {
                int ret = 0;
                if (ssl.Checked)
                {
                    ret = pop3.SSL_init();
                    pop3.ServerPort = 995;
                }
                if (ret != 00)
                {
                    throw new Exception("Error with SSL");
                }
                ret = pop3.Connect(textBox2.Text, TxtPhone.Text, textBox1.Text);
                System.Console.WriteLine("Connected");
                if (ret != 00)
                {
                    throw new Exception("Error with Connection");
                }
                int countmail = pop3.GetTotalOfMails();

                if (countmail == -1)
                {
                    throw new Exception("error with emails");
                }
                if (countmail == 0)
                {
                    Console.WriteLine("No Emails");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        void call()
        {
            try
            {
                Console.WriteLine("Placing Call");
                if (callers[0] != "")
                    oSkype.PlaceCall(callers[0], callers[1], callers[2], callers[3]);
                Console.WriteLine("Call Placed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception during call: " + e.Message);
                Console.WriteLine(callers[0] + " " + callers[1] + " " + callers[2] + " " + callers[3]);
                callers.Clear();
                Connect("", "", "");
                pop3.Delete(0);
                pop3.Close();
                CheckMail.Enabled = true;
            }
        }

        private void ChkActivate_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkActivate.Checked)
            {
                oSkype.CallStatus += CallStatus;
                oSkype.Attach(5, true);
                CheckMail.Enabled = true;
            }
            else
            {
                CheckMail.Enabled = false;
            }
        }

        void inProgress()
        {
            MailClass mail = new MailClass();
            if (pop3.Connected)
                pop3.Close();

            mail.Reset();
            mail.FromAddr = "callback@rlanders.com";
            mail.ServerAddr = "";
            mail.Subject = "Call in Progress...";
            mail.BodyText = "";
            //mail.AddRecipient("", returnmessage, 0);
            mail.AddRecipient("", "andrew.shelton0@yahoo.com", 0);
            mail.SendMail();
        }

        int stack = 0;

        void sendmail()
        {
            MailClass mail = new MailClass();

            if (pop3.Connected)
                pop3.Close();

            Console.WriteLine("Sending Mail invitro");

            mail.Reset();
            mail.FromAddr = "callback@rlanders.com";
            mail.ServerAddr = "";
            mail.Subject = "Call Recording";
            mail.BodyText = "";
            foreach (string file in recordings)
            {
                Console.WriteLine("Adding Attachment: " + file);
                mail.AddAttachment(file);
            }

            //mail.AddRecipient("", returnmessage, 0);
            mail.AddRecipient("", "andrew.shelton0@yahoo.com", 0);
            if (mail.SendMail() == 0)
            {
                Console.WriteLine("MessageSent");
            }
            else
            {
                Console.WriteLine("Mail Delivery Failed");
                stack++;
                if (stack < 10)
                    sendmail();
                stack--;
            }
            recordings.Clear();
        }

        private void CallStatus(Call call, TCallStatus status)
        {
            System.Console.WriteLine("Call Status:" + converter.CallStatusToText(status));
            System.Console.WriteLine("Call: " + converter.CallTypeToText(call.Type));
            if (status == TCallStatus.clsRinging && (call.Type == TCallType.cltIncomingPSTN || call.Type == TCallType.cltIncomingP2P))
            {
                System.Console.WriteLine("Incomming Call");
            }
            if ((status == TCallStatus.clsBusy || status == TCallStatus.clsCancelled || status == TCallStatus.clsFailed || status == TCallStatus.clsFinished))
            {
                System.Console.WriteLine("Ending Call");
                try
                {
                    Command hook = new Command();
                    hook.Command = "hook on";
                    oSkype.SendCommand(hook);
                    hook.Command = "hook off";
                    oSkype.SendCommand(hook);
                    radioButton2.Checked = false;
                }
                catch
                { }

                Console.WriteLine("Active Calls: " + oSkype.ActiveCalls.Count.ToString());

                if (oSkype.ActiveCalls.Count == 0)
                {
                    Console.WriteLine("Sending Email...");
                    sendmail();
                    Progress = false;
                    Console.WriteLine("Email Sent");
                    CheckMail.Enabled = true;
                }
            }
            if ((status == TCallStatus.clsInProgress))
            {
                if (Progress == false)
                {
                    Progress = true;
                    inProgress();
                }
                System.Console.WriteLine("In Progress - muting");
                ((ISkype)oSkype).Mute = true;
                Console.WriteLine("Recording");
                radioButton2.Checked = true;
                call.set_OutputDevice(TCallIoDeviceType.callIoDeviceTypeFile, "C:/Users/Robert/skype/" + call.Id.ToString() + ".wav");
                recordings.Add("C:/Users/Robert/skype/" + call.Id.ToString() + ".wav");
            }
        }

        bool Progress = false;

        private void OnCall_Tick(object sender, EventArgs e)
        {
            try
            {
                if (currentcall.Status == TCallStatus.clsInProgress)
                {
                    ((ISkype)oSkype).Mute = true;
                }
            }
            catch
            {
            }
        }

        private void HangUp_Tick(object sender, EventArgs e)
        {
            try
            {
            }
            catch
            {
            }
        }

        private void CheckMail_Tick(object sender, EventArgs e)
        {
            radioButton1.Checked = true;

            if (!pop3.Connected)
            {
                Console.WriteLine("initial connect");
                Connect("pop.gmail.com", "landers.robert@gmail.com", "Certify");
            }

            bool callnow = false;
            int counter = -1;

            while ((pop3.GetTotalOfMails() != 0 || pop3.GetTotalOfMails() != -1) && counter < pop3.GetTotalOfMails())
            {
                Console.WriteLine("there are " + pop3.GetTotalOfMails() + " emails.");
                counter++;
                if (!pop3.Connected)
                {
                    Console.WriteLine("reconnecting...");
                    Connect("pop.gmail.com", "landers.robert@gmail.com", "Certify");
                }
                Console.WriteLine("Downloading Email");
                msg.RawContent = pop3.Retrieve(counter);
                if (msg.GetSubject().ToLower() == "call 2")
                {
                    Console.WriteLine("This is a call...");
                    callers = getCallers(msg.GetBodyText(), msg.GetSubject());
                    callnow = true;
                    returnmessage = msg.GetFromAddress();
                }
                else
                    callnow = false;

                Console.WriteLine("Deleting Email");
                pop3.Delete(counter);

                if (callnow)
                {
                    Console.WriteLine("Calling");
                    CheckMail.Enabled = false;
                    call();
                    return;
                }
            }
            pop3.Close();
            Console.WriteLine("Done Checking mail.");
            radioButton1.Checked = false;
        }

        List<string> getCallers(string body, string subj)
        {
            List<string> ret = new List<string>();
            int counter = 0;

            string numbers = "";
            int callers = int.Parse(subj.Remove(0, 4).Trim());
            System.Console.WriteLine(body);

            if (body.StartsWith("+"))
            {
                while (body.StartsWith("+"))
                {
                    counter++;
                    if (counter > 4)
                        return ret;
                    numbers = body.Substring(0, body.IndexOf(' '));
                    Console.WriteLine("calling number: " + numbers);
                    body = body.Remove(0, body.IndexOf(' ') + 1);
                    Console.WriteLine("Remaining body: " + body);
                    numbers = numbers.Substring(0, 12);
                    ret.Add(numbers);
                }
            }

            if (ret.Count < 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    ret.Add("");
                }
            }

            return ret;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void console_Load(object sender, EventArgs e)
        {
            Settings1 settings = new Settings1();
            settings.Reload();
            if (settings.savepass == true)
            {
                passSave.Checked = true;
                textBox1.Text = settings.password;
            }
            else
                passSave.Checked = false;
            TxtPhone.Text = settings.username;
            textBox2.Text = settings.pop3;
            ssl.Checked = settings.usessl;
        }

        private void onClose(object sender, FormClosingEventArgs e)
        {
            Settings1 settings = new Settings1();
            settings.Reload();
            if (passSave.Checked)
            {
                settings.savepass = true;
                settings.password = textBox1.Text;
            }
            settings.username = TxtPhone.Text;
            settings.pop3 = textBox2.Text;
            settings.usessl = ssl.Checked;
            settings.Save();
        }
    }
}
