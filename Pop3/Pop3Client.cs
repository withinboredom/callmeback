using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

//taken from codeproject.com's Desmond McCarter's Pop3 Client

namespace Pop3
{
    class Pop3Client
    {
        private Pop3Credential m_credential;

        private const int m_pop3port = 110;
        private const int MAX_BUFFER_READ_SIZE = 256;

        private long m_inboxPosition = 0;
        private long m_directPosition = -1;

        private Socket m_socket = null;

        private Pop3Message m_pop3Message = null;

        public Pop3Credential userDetails
        {
            get
            {
                return m_credential;
            }

            set
            {
                m_credential = value;
            }
        }

        public string From
        {
            get { return m_pop3Message.From; }
        }

        public string To
        {
            get { return m_pop3Message.To; }
        }

        public string Subject
        {
            get { return m_pop3Message.Subject; }
        }

        public string Body
        {
            get { return m_pop3Message.Body; }
        }

        public IEnumerator MultipartEnumerator
        {
            get { return m_pop3Message.MultipartEnumerator; }
        }

        public bool IsMultipart
        {
            get { return m_pop3Message.IsMultipart; }
        }

        public Pop3Client(string user, string password, string server)
        {
            m_credential = new Pop3Credential(user, password, server);
        }

        private Socket GetClientSocket()
        {
            Socket s = null;

            try
            {
                IPHostEntry hostentry = null;

                //get host related info
                //hostentry = Dns.Resolve(m_credential.Server);
                hostentry = Dns.GetHostEntry(m_credential.Server);

                //loop through the address list to obtain the supported address family.
                //this is to avoid an exception that occurs when the host ip address
                //is not compat with the address family (ex: ipv6)
                foreach (IPAddress address in hostentry.AddressList)
                {
                    IPEndPoint ipe = new IPEndPoint(address, m_pop3port);

                    Socket tempsock = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    tempsock.Connect(ipe);

                    if (tempsock.Connected)
                    {
                        //we have a connection return this socket
                        s = tempsock;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Pop3ConnectException(e.ToString());
            }

            //throw an exception we can't detect
            if (s == null)
            {
                throw new Pop3ConnectException("Error : connecting to " + m_credential.Server);
            }

            return s;
        }

        private void Send(string data)
        {
            if (m_socket == null)
            {
                throw new Pop3MessageException("pop3 connection is lost");
            }

            try
            {
                //convert the data to byte data using ascii encoding
                byte[] byteData = Encoding.ASCII.GetBytes(data + "\r\n");

                //begin sending the data to the remote device
                m_socket.Send(byteData);
            }
            catch (Exception e)
            {
                throw new Pop3SendException(e.ToString());
            }
        }

        private string GetPop3String()
        {
            if (m_socket == null)
            {
                throw new Pop3MessageException("Connection to pop3 is lost");
            }

            byte[] buffer = new byte[MAX_BUFFER_READ_SIZE];
            string line = null;

            try
            {
                int bytecount = m_socket.Receive(buffer, buffer.Length, 0);

                line = Encoding.ASCII.GetString(buffer, 0, bytecount);
            }
            catch (Exception e)
            {
                throw new Pop3ReceiveException(e.ToString());
            }

            return line;
        }

        private void LoginToInbox()
        {
            string returned;

            //send username
            Send("user " + m_credential.User);

            //get response
            returned = GetPop3String();

            if (!returned.Substring(0, 3).Equals("+OK"))
            {
                throw new Pop3LoginException("login not accepted");
            }

            Send("pass " + m_credential.Pass);

            returned = GetPop3String();

            if (!returned.Substring(0, 3).Equals("+OK"))
            {
                throw new Pop3LoginException("login/password not accepted");
            }
        }

        public long MessageCount
        {
            get
            {
                long count = 0;

                if (m_socket == null)
                {
                    throw new Pop3MessageException("pop3 server not detected");
                }

                Send("stat");

                string returned = GetPop3String();

                // if values returned ...
                if (Regex.Match(returned,
                    @"^.*\+OK[ |	]+([0-9]+)[ |	]+.*$").Success)
                {
                    // get number of emails ...
                    count = long.Parse(Regex
                    .Replace(returned.Replace("\r\n", "")
                    , @"^.*\+OK[ |	]+([0-9]+)[ |	]+.*$", "$1"));
                }

                return (count);
            }
        }

        public void CloseConnection()
        {
            Send("quit");
            m_socket = null;
            m_pop3Message = null;
        }

        public bool DeleteEmail()
        {
            bool ret = false;

            Send("dele " + m_inboxPosition);

            string returned = GetPop3String();

            if (Regex.Match(returned,
                @"^.*\+OK.*$").Success)
            {
                ret = true;
            }

            return ret;
        }

        public bool NextEmail(long directPosition)
        {
            bool ret;

            if (directPosition >= 0)
            {
                m_directPosition = directPosition;
                ret = NextEmail();
            }
            else
            {
                throw new Pop3MessageException("position less than 0");
            }

            return ret;
        }

        public bool NextEmail()
        {
            string returned;

            long pos;

            if (m_directPosition == -1)
            {
                if (m_inboxPosition == 0)
                {
                    pos = 1;
                }
                else
                {
                    pos = m_inboxPosition + 1;
                }
            }
            else
            {
                pos = m_directPosition + 1;
                m_directPosition = -1;
            }

            //send username
            Send("list " + pos.ToString());

            //get response
            returned = GetPop3String();

            //if email does not exist then return false
            if (returned.Substring(0, 4).Equals("-ERR"))
            {
                return false;
            }

            m_inboxPosition = pos;

            //strip clrf
            string[] nocr = returned.Split(new char[] { '\r' });

            //get size
            string[] elements = nocr[0].Split(new char[] { ' ' });

            long size = long.Parse(elements[2]);

            //else read data
            m_pop3Message = new Pop3Message(m_inboxPosition, size, m_socket);

            return true;
        }

        public void OpenInbox()
        {
            //get a socket
            m_socket = GetClientSocket();

            //get initial header from pop3 server
            string header = GetPop3String();

            if (!header.Substring(0, 3).Equals("+OK"))
            {
                throw new Exception("Invalid POP3 Header");
            }

            //send login details
            LoginToInbox();
        }
    }
}
