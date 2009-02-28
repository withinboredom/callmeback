using System;

namespace Pop3
{
    class Pop3ConnectException : Exception
    {
        private string m_exceptionstring;

        public Pop3ConnectException()
            : base()
        {
            m_exceptionstring = null;
        }

        public Pop3ConnectException(string exceptionstring)
            : base()
        {
            m_exceptionstring = exceptionstring;
        }

        public Pop3ConnectException(string exceptionstring, Exception ex)
            : base(exceptionstring, ex)
        {
        }

        public override string ToString()
        {
            return m_exceptionstring;
        }
    }
}
