using System;

namespace Pop3
{
    public class Pop3ReceiveException : Exception
    {
        private string m_exceptionString;

        public Pop3ReceiveException()
            : base()
        {
            m_exceptionString = null;
        }

        public Pop3ReceiveException(string exceptionstring)
            : base()
        {
            m_exceptionString = exceptionstring;
        }

        public Pop3ReceiveException(string exceptionstring, Exception ex)
            : base(exceptionstring, ex)
        {
        }

        public override string ToString()
        {
            return m_exceptionString;
        }
    }
}
