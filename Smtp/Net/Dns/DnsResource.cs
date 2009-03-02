using System;
using System.Collections.Generic;
using System.Text;

namespace Smtp.Net.Dns
{
    public class DnsResource
    {
        public string Name;
        public QueryType QueryType;
        public QueryClass QueryClass;

        public Int32 TimeToLive;
        public UInt16 DataLength;
        public byte[] Data;
        public object Content;
    }
}
