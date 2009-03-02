using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Pop3
{
    public class Pop3MessageFilter : Pop3Message
    {
        public Pop3MessageFilter(long position, long size, Socket client)
            : base(position, size, client)
        {
        }

        public string BodyAsText
        {
            get
            {
                return Regex.Replace(base.Body, @"<(.|\n)*?>", string.Empty);
            }
        }
    }
}
