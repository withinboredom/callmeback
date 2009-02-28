using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Pop3
{
    class Pop3StateObject
    {
        //client socket
        public Socket workSocket = null;

        //size of receive buffer
        public const int BufferSize = 256;

        //receive buffer
        public byte[] buffer = new byte[BufferSize];

        //received data string
        public StringBuilder sb = new StringBuilder();
    }
}
