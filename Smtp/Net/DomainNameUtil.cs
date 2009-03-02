using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Smtp.Net.Dns;
using Microsoft.Win32;

namespace Smtp.Net
{
    public class DomainNameUtil
    {
        public static string[] FindDnsServers()
        {
            RegistryKey start = Registry.LocalMachine;
            string DNSservers = @"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters";

            RegistryKey DNSserverKey = start.OpenSubKey(DNSservers);
            if (DNSserverKey == null)
            {
                return null;
            }
            string serverlist = (string)DNSserverKey.GetValue("NameServer");
            if (String.IsNullOrEmpty(serverlist))
            {
                serverlist = (string)DNSserverKey.GetValue("DhcpNameServer");
            }
            DNSserverKey.Close();
            start.Close();
            if (String.IsNullOrEmpty(serverlist))
            {
                return null;
            }
            string[] servers = serverlist.Split(' ');
            return servers;
        }

        static List<IPAddress> GetRootDnsServers()
        {
            List<IPAddress> rootServers = new List<IPAddress>();
            rootServers.Add(IPAddress.Parse("128.8.10.90"));
            rootServers.Add(IPAddress.Parse("192.203.230.10"));
            rootServers.Add(IPAddress.Parse("192.36.148.17"));
            rootServers.Add(IPAddress.Parse("192.58.128.30"));
            rootServers.Add(IPAddress.Parse("193.0.14.129"));
            rootServers.Add(IPAddress.Parse("202.12.27.33"));
            return rootServers;
        }

        static List<IPAddress> RootDnsServers = GetRootDnsServers();

        public static IPHostEntry GetIPHostEntry(string domainName)
        {
            return GetIPHostEntry(domainName, QueryType.Address, FindDnsServers());
        }

        public static IPHostEntry GetIPHostEntryForMX(string domainName)
        {
            return GetIPHostEntry(domainName, QueryType.MailExchange, FindDnsServers());
        }

        static List<IPAddress> DnsServers = new List<IPAddress>();

        public static IPHostEntry GetIPHostEntry(string domainName, QueryType queryType, string[] dnsServers)
        {
            if (String.IsNullOrEmpty(domainName))
            {
                throw new ArgumentException("domain is empty", "domainName");
            }
            DnsServers.Clear();
            if (dnsServers != null)
            {
                foreach (string dnsServer in dnsServers)
                {
                    DnsServers.Add(IPAddress.Parse(dnsServer));
                }
            }
            else
            {
                DnsServers = new List<IPAddress>();
                DnsServers.AddRange(RootDnsServers);
            }

            int retry = 0;

            while (retry < 10)
            {
                foreach (IPAddress dns in DnsServers)
                {
                    IPHostEntry ip = GetIPHostEntry(domainName, queryType, dns);
                    if (ip != null)
                    {
                        return ip;
                    }
                }
                retry++;
            }

            return null;
        }

        public static IPHostEntry GetIPHostEntry(string domainName, QueryType queryType, IPAddress dnsServer)
        {
            DnsMessage message = DnsMessage.StandardQuery();
            DnsQuery query = new DnsQuery(domainName, queryType);

            message.Querys.Add(query);
            byte[] msgData = DnsMessageCoder.EncodeDnsMessage(message);
            try
            {
                byte[] reply = QueryServer(msgData, dnsServer);
                if (reply != null)
                {
                    DnsMessage answer = DnsMessageCoder.DecodeDnsMessage(reply);
                    if (answer.ID == message.ID)
                    {
                        IPHostEntry host = new IPHostEntry();
                        host.HostName = domainName;
                        if (queryType == QueryType.Address)
                        {
                            host.AddressList = GetAddressList(domainName, answer);
                        }
                        else if (queryType == QueryType.MailExchange)
                        {
                            host.Aliases = GetMailExchangeAliases(domainName, answer);
                            host.AddressList = GetAddressList(answer, new List<string>(host.Aliases));
                        }
                        return host;
                    }
                    else if (answer.AuthoritativeRecords.Count > 0)
                    {
                        IPAddress[] serverAddresses = GetAuthorityServers(answer);
                        foreach (IPAddress serverIP in serverAddresses)
                        {
                            IPHostEntry host = GetIPHostEntry(domainName, queryType, serverIP);
                            if (host != null)
                            {
                                return host;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        private static IPAddress[] GetAddressList(string domainName, DnsMessage answer)
        {
            List<IPAddress> addressList = new List<IPAddress>();
            foreach (DnsResource resource in answer.Answers)
            {
                if (resource.QueryType == QueryType.Address && resource.Name == domainName)
                {
                    IPAddress ip = new IPAddress((byte[])resource.Content);
                    addressList.Add(ip);
                }
            }
            return addressList.ToArray();
        }

        private static string[] GetMailExchangeAliases(string domainName, DnsMessage answer)
        {
            List<string> aliases = new List<string>();
            foreach (DnsResource resource in answer.Answers)
            {
                MailExchange mx = (MailExchange)resource.Content;
                aliases.Add(mx.HostName);
            }
            return aliases.ToArray();
        }

        private static IPAddress[] GetAuthorityServers(DnsMessage answer)
        {
            List<string> authorities = new List<string>();
            foreach (DnsResource resource in answer.AuthoritativeRecords)
            {
                if (resource.QueryType == QueryType.NameServer)
                {
                    string nameServer = (string)resource.Content;
                    authorities.Add(nameServer);
                }
            }
            if (answer.AdditionalRecords.Count > 0)
            {
                return GetAddressList(answer, authorities);
            }
            else
            {
                List<IPAddress> serverAddresses = new List<IPAddress>();
                foreach (string authority in authorities)
                {
                    serverAddresses.AddRange(System.Net.Dns.GetHostAddresses(authority));
                }
                return serverAddresses.ToArray();
            }
        }

        private static IPAddress[] GetAddressList(DnsMessage answer, List<string> authorities)
        {
            List<IPAddress> serverAddresses = new List<IPAddress>();
            foreach (DnsResource resource in answer.AdditionalRecords)
            {
                if (resource.QueryType == QueryType.Address)
                {
                    if (authorities.Contains(resource.Name))
                    {
                        IPAddress serverIP = new IPAddress((byte[])resource.Content);
                        serverAddresses.Add(serverIP);
                    }
                }
            }
            return serverAddresses.ToArray();
        }

        private static byte[] QueryServer(byte[] query, IPAddress serverIP)
        {
            byte[] retVal = null;

            try
            {
                IPEndPoint ipend = new IPEndPoint(serverIP, 53);
                Socket udpclient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                IPEndPoint iplocalend = new IPEndPoint(IPAddress.Any, 0);
                EndPoint localpoint = (EndPoint)iplocalend;
                udpclient.Bind(localpoint);

                udpclient.Connect(ipend);

                //send query
                udpclient.Send(query);

                //wait for reply
                if (udpclient.Poll(5 * 1000000, SelectMode.SelectRead))
                {
                    retVal = new byte[512];
                    udpclient.Receive(retVal);
                }

                udpclient.Close();
            }
            catch
            {
            }

            return retVal;
        }
    }
}
