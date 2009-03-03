using System;
using System.Collections;
using System.Text;
using SKYPE4COMLib;

/* Caller Class
 * Description: Connects to Skype instance.  Handles outgoing calls and recording.
 *  One Instance per runtime.
 */

namespace callback
{
    class Caller
    {
        Skype m_skype = new Skype();
        ArrayList m_callers = new ArrayList();
        ArrayList m_recordings = new ArrayList();

        public ArrayList Recordings
        {
            get
            {
                return m_recordings;
            }
        }

        class caller
        {
            public int id;
            public TCallStatus status;

            public override string ToString()
            {
                return "Caller " + id + ": " + status.ToString();
            }
        }

        public bool callList(ArrayList numbers)
        {
            if (m_callers == null)
                return false;

            m_callers = numbers;

            Console.WriteLine("Adding list of numbers: ");

            foreach (string number in numbers)
            {
                Console.WriteLine("\t" + number);
            }

            return true;
        }

        public bool addCaller(string number)
        {
            m_callers.Add(number);

            Console.WriteLine("Adding Caller: " + number);

            return true;
        }

        public Caller()
        {
            m_skype.CallStatus += CallStatus;
            m_skype.Attach(5, true);
            Console.WriteLine("Attachment successfull");
        }

        private bool emailsent = true;

        public bool GoodToGo
        {
            get
            {
                return (!emailsent) && (!incall);
            }
        }

        public void placeBlockedCall(string Sender)
        {
            Console.WriteLine("Placing Blocked Call");
            Settings1 settings = new Settings1();
            settings.Reload();

            if (calls == null) calls = new ArrayList();
            else calls.Clear();

            Console.WriteLine("Created call array");

            if (settings.chain)
            {
                if (m_callers.Count > 0)
                {
                    Console.WriteLine("Calling first caller in chain");
                    Command command = new Command();
                    command.Command = "call " + m_callers[0].ToString();
                    m_skype.SendCommand(command);
                }
            }
            else
            {
                Console.WriteLine("Calling list of callers at once");
                if (m_callers.Count < 4)
                {
                    m_callers.Add("");
                    m_callers.Add("");
                    m_callers.Add("");
                    m_callers.Add("");
                }
                try
                {
                    Call call = m_skype.PlaceCall(m_callers[0].ToString(), m_callers[1].ToString(), m_callers[2].ToString(), m_callers[3].ToString());
                    string filename = "C:/users/Robert/skype/" + call.Id.ToString() + ".wav";
                    call.set_OutputDevice(TCallIoDeviceType.callIoDeviceTypeFile, filename);
                    incall = true;
                    m_recordings.Add(filename);
                }
                catch (Exception ex)
                {
                    incall = false;
                    emailsent = false;
                    Console.WriteLine("Error with call");
                    string body = "Error with call to the following numbers: \r\n";
                    foreach (string number in m_callers)
                    {
                        body += number + "\r\n";
                    }
                    body += "From email address: " + Sender;
                    Smtp.Net.EmailSender.Send("callback@rlanders.com", "callback@rlanders.com", "call error report", body);
                }
            }

            ((ISkype)m_skype).Mute = true;
        }

        private bool incall = false;

        bool checkCallExists(caller call)
        {
            bool exists = false;

            foreach (caller c in calls)
            {
                if (c.id == call.id)
                    exists = true;
            }

            return exists;
        }

        private ArrayList calls;

        Settings1 settings = new Settings1();

        private caller createCaller(int id, TCallStatus status)
        {
            caller call = new caller();
            call.id = id;
            call.status = status;

            Console.WriteLine("Creating caller: " + call);

            if (!checkCallExists(call))
            {
                calls.Add(call);
            }
            else
            {
                ((caller)calls[findID(id)]).status = status;
            }

            return call;
        }

        private int findID(int id)
        {
            for (int i = 0; i < calls.Count; i++)
            {
                if (((caller)calls[i]).id == id)
                    return i;
            }
            return -1;
        }

        public void unblockCall()
        {
            this.calls.Clear();
            this.emailsent = true;
            
            this.incall = false;
            
            this.m_callers.Clear();
            this.m_recordings.Clear();
            
        }

        private void CallStatus(Call call, TCallStatus status)
        {
            createCaller(call.Id, status);

            if ((status == TCallStatus.clsBusy || status == TCallStatus.clsCancelled || status == TCallStatus.clsFailed || status == TCallStatus.clsFinished))
            {
                //end the call
                Command command = new Command();
                command.Command = "hook on";
                m_skype.SendCommand(command);
                incall = false;
                emailsent = false;
            }
        }
    }
}
