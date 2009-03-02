using System;
using System.Collections.Generic;
using System.IO;

namespace Smtp.Net.Dns
{
    public enum MessageType { Query = 0, Response = 1 }

    public enum QueryKind
    {
        StandardQuery = 0,
        InverseQuery = 1,
        ServerStatus = 2,
    }

    public enum ResponseCode
    {
        NoError = 0,
        FormatError = 1,
        ServerFailure = 2,
        NameError = 3,
        NotImplemented = 4,
        Refused = 5,
    }

    public class DnsMessage
    {
        public UInt16 ID;

        public MessageType Type;

        public QueryKind QueryKind;

        public bool IsAuthoritativeAnswer;

        public bool IsTruncated;

        public bool IsRecursionDesired;

        public bool IsRecursionAvailable;

        public ResponseCode ResponseCode;

        public List<DnsQuery> Querys = new List<DnsQuery>();
        public List<DnsResource> Answers = new List<DnsResource>();

        public List<DnsResource> AuthoritativeRecords = new List<DnsResource>();
        public List<DnsResource> AdditionalRecords = new List<DnsResource>();

        static UInt16 m_ID = 100;
        internal static UInt16 GenerateID()
        {
            if (m_ID >= 65535)
            {
                m_ID = 100;
            }
            return m_ID++;
        }

        public static DnsMessage StandardQuery()
        {
            DnsMessage message = new DnsMessage();
            message.ID = GenerateID();
            message.Type = MessageType.Query;
            message.QueryKind = QueryKind.StandardQuery;
            return message;
        }

        public static DnsMessage InverseQuery()
        {
            DnsMessage message = new DnsMessage();
            message.ID = GenerateID();
            message.Type = MessageType.Query;
            message.QueryKind = QueryKind.InverseQuery;
            return message;
        }
    }
}
