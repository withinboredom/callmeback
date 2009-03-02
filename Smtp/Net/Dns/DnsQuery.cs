using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smtp.Net.Dns
{
    public enum QueryType
    {
        Address = 1,
        NameServer = 2,
        CanonicalName = 5,
        StartOfAuthorityZone = 6,
        WellKnownService = 11,
        Pointer = 12,
        HostInfo = 13,
        MailInfo = 14,
        MailExchange = 15,
        Text = 16,
        Unknown = 9999,
    }

    public enum QueryClass
    {
        Internet = 1,
        Chaos = 3,
        Hesiod = 4,
    }

    public class DnsQuery
    {
        public string DomainName;
        public QueryType QueryType;
        public QueryClass QueryClass;

        public DnsQuery(string domainName, QueryType qType)
        {
            DomainName = domainName;
            QueryType = qType;
            QueryClass = QueryClass.Internet;
        }

        public DnsQuery(string domainName, QueryType qtype, QueryClass qclass)
        {
            DomainName = domainName;
            QueryType = qtype;
            QueryClass = qclass;
        }
    }
}
