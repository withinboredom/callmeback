using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace Smtp.Net
{
    public class EmailSender
    {
        public static int SmtpPort = 25;

        public static bool Send(string from, string to, string subject, string body)
        {
            string domainName = GetDomainName(to);
            IPAddress[] servers = GetMailExchangeServer(domainName);
            //if (servers == null) return false;
            //foreach (IPAddress server in servers)
            {
                try
                {
                    SmtpClient client = new SmtpClient(@"proxy.rlanders.com", SmtpPort);
                    NetworkCredential creds = new NetworkCredential("callback", "certify588");
                    client.Credentials = creds;
                    client.Send(from, to, subject, body);
                    return true;
                }
                catch (Exception ex)
                {
                    //continue;
                    Console.WriteLine("error with send: " + ex.ToString());
                }
            }
            return false;
        }

        public static bool Send(MailMessage mailMessage)
        {
            string domainName = GetDomainName(mailMessage.To[0].Address);
            IPAddress[] servers = GetMailExchangeServer(domainName);
            if (servers == null) return false;
            foreach (IPAddress server in servers)
            {
                try
                {
                    SmtpClient client = new SmtpClient(server.ToString(), SmtpPort);
                    client.Send(mailMessage);
                    return true;
                }
                catch
                {
                    continue;
                }
            }
            return false;
        }

        public static string GetDomainName(string emailAddress)
        {
            int atidx = emailAddress.IndexOf('@');
            if (atidx == -1)
            {
                throw new ArgumentException("not valid email addr", "emailaddress");
            }
            if (emailAddress.IndexOf('<') > -1 && emailAddress.IndexOf('>') > -1)
            {
                return emailAddress.Substring(atidx + 1, emailAddress.IndexOf('>') - atidx);
            }
            else
            {
                return emailAddress.Substring(atidx + 1);
            }
        }

        public static IPAddress[] GetMailExchangeServer(string domainName)
        {
            IPHostEntry hostentry = DomainNameUtil.GetIPHostEntryForMX(domainName);
            if (hostentry == null) return null;
            if (hostentry.AddressList.Length > 0)
            {
                return hostentry.AddressList;
            }
            else if (hostentry.Aliases.Length > 0)
            {
                return System.Net.Dns.GetHostAddresses(hostentry.Aliases[0]);
            }
            else
            {
                return null;
            }
        }
    }
}
