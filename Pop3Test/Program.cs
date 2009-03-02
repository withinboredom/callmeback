using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Pop3;
using Smtp.Net;

namespace Pop3Test
{
    class Program
    {
        static void Main(string[] args)
        {
            runner program = new runner();

            program.run();
        }

        class runner
        {
            public string stripHTML(string html)
            {
                return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
            }

            public void emailme()
            {
                EmailSender.Send("callback@rlanders.com", "landers.robert@gmail.com", "testing uno dos tres", "body body sexy sexy");
                EmailSender.Send("callback@rlanders.com", "callback@rlanders.com", "call 2", "+19313024501");
            }

            public ArrayList getPhoneNumbers(string body)
            {
                ArrayList hh = new ArrayList();

                string [] lis = body.Split(new char[] { '\r', '\n', ' ' });

                foreach (string i in lis)
                {
                    if((Regex.IsMatch(i, @"^[+][0-9]\d{3}-\d{3}-\d{4}$")) || (Regex.IsMatch(i, @"^[+][0-9]\d{3}\d{3}\d{4}$")))
                    {
                        hh.Add(i);
                    }
                }

                return hh;
            }

            private string removeSpaceDashes(string body)
            {
                return Regex.Replace(body, @"[ -]+", "");
            }

            public void run()
            {
                try
                {
                    Pop3Client client = new Pop3Client("callback", "certify588", "proxy.rlanders.com");
                    client.OpenInbox();

                    while (client.NextEmail())
                    {
                        Console.WriteLine("Original Body: " + client.Body);

                        string htmlless = stripHTML(client.Body);

                        Console.WriteLine("HTMLless Body: " + client.Body);

                        ArrayList numbers = getPhoneNumbers(htmlless);

                        if (numbers.Count > 0)
                        {
                            Console.WriteLine("Phone Numbers Harvested: ");
                            for (int i = 0; i < numbers.Count; i++)
                            {
                                Console.WriteLine("\tNo. " + i.ToString() + ": " + numbers[i]);
                            }
                        }
                    }

                    client.CloseConnection();
                }
                catch (Pop3LoginException ex)
                {
                    Console.WriteLine("Error logging in");
                }

                emailme();
            }
        }
    }
}
