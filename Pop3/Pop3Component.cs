using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Pop3
{
    class Pop3Component
    {
        private string m_contentType;
        private string m_name;
        private string m_filename;
        private string m_contentTransferEncoding;
        private string m_contentDescription;
        private string m_contentDisposition;
        private string m_data;
        private string m_filepath;

        public byte[] m_binaryData;

        public string FileExtension
        {
            get
            {
                string extension = null;

                if ((m_filename != null) && Regex.Match(m_filename, @"^.*\..*$").Success)
                {
                    extension = Regex.Replace(m_name, @"^[^\.]*\.([^\.]+)$", "$1");
                }

                return extension;
            }
        }

        public string FileNoExtension
        {
            get
            {
                string extension = null;

                if ((m_filename != null) && Regex.Match(m_filename, @"^.*\..*$").Success)
                {
                    extension = Regex.Replace(m_name, @"^([^\.]*)\.[^\.]+$", "$1");
                }

                return extension;
            }
        }

        public string FilePath
        {
            get { return m_filepath; }
        }

        public string FileName
        {
            get { return m_filename; }
        }

        public string ContentType
        {
            get { return m_contentType; }
        }

        public string Name
        {
            get { return m_name; }
        }

        public string ContentTransferEncoding
        {
            get { return m_contentTransferEncoding; }
        }

        public string ContentDescription
        {
            get { return m_contentDescription; }
        }

        public string ContentDisposition
        {
            get { return m_contentDisposition; }
        }

        public string Data
        {
            get { return m_data; }
        }

        public override string ToString()
        {
            return
                "Content-Type: " + m_contentType + "\r\n" +
                "Name: " + m_name + "\r\n" +
                "Filename: " + m_filename + "\r\n" +
                "Content-Transfer-Encoding: " + m_contentTransferEncoding + "\r\n" +
                "Content-Description: " + m_contentDescription + "\r\n" +
                "Content-Disposition: " + m_contentDisposition + "\r\n" +
                "Data: " + m_data;
        }

        public bool isBody
        {
            get
            {
                return (m_contentDisposition == null) ? true : false;
            }
        }

        public bool isAttachment
        {
            get
            {
                bool ret = false;

                if (m_contentDisposition != null)
                {
                    ret = Regex.Match(m_contentDisposition, "^attachment.*$").Success;
                }

                return ret;
            }
        }

        private void DecodeData()
        {
            if (m_contentDisposition != null)
            {
                if (!Directory.Exists(Pop3Statics.DataFolder))
                {
                    Directory.CreateDirectory(Pop3Statics.DataFolder);
                }

                m_filepath = Pop3Statics.DataFolder + @"\" + m_filename;

                //if base-64 data
                if ((m_contentDisposition.Equals("attachment;")) && (m_contentTransferEncoding.ToUpper().Equals("BASE64")))
                {
                    //convert from base-64
                    m_binaryData = Convert.FromBase64String(m_data.Replace("\n", ""));

                    BinaryWriter bw = new BinaryWriter(new FileStream(m_filepath, FileMode.Create));

                    bw.Write(m_binaryData);
                    bw.Flush();
                    bw.Close();
                }
                else if ((m_contentDisposition.Equals("attachment;")) && (m_contentTransferEncoding.ToUpper().Equals("QUOTED-PRINTABLE")))
                {
                    //if printable
                    using (StreamWriter sw = File.CreateText(m_filepath))
                    {
                        sw.Write(Pop3Statics.FromQuotedPrintable(m_data));
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
        }

        public Pop3Component(string contentType, string data)
        {
            m_contentType = contentType;
            m_data = data;
        }

        public Pop3Component(string contentType, string name, string filename, string contentTransferEncoding, string contentDescription, string contentDisposition, string data)
        {
            m_contentType = contentType;
            m_name = name;
            m_filename = filename;
            m_contentTransferEncoding = contentTransferEncoding;
            m_contentDescription = contentDescription;
            m_contentDisposition = contentDisposition;
            m_data = data;

            DecodeData();
        }
    }
}
