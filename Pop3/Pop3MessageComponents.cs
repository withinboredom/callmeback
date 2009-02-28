using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace Pop3
{
    class Pop3MessageComponents
    {
        private ArrayList m_component = new ArrayList();

        public IEnumerator ComponentEnumerator
        {
            get
            {
                return m_component.GetEnumerator();
            }
        }

        public int NumberOfComponents
        {
            get
            {
                return m_component.Count;
            }
        }

        public Pop3MessageComponents(string[] lines, long startofbody, string multipartboundary, string maincontenttype)
        {
            long stopOfBody = lines.Length;

            if (multipartboundary == null)
            {
                StringBuilder sbText = new StringBuilder();

                for (long i = startofbody; i < stopOfBody; i++)
                {
                    sbText.Append(lines[i].Replace("\n", "").Replace("\r", ""));
                }

                //create a new component
                m_component.Add(new Pop3Component(maincontenttype, sbText.ToString()));
            }
            else
            {
                string boundary = multipartboundary;

                bool firstcomponent = true;

                //loop through email
                for (long i = startofbody; i < stopOfBody; )
                {
                    bool boundaryfound = true;

                    string contentType = null;
                    string name = null;
                    string filename = null;
                    string contenttransfer = null;
                    string description = null;
                    string disposition = null;
                    string data = null;

                    //if first block
                    if (firstcomponent)
                    {
                        boundaryfound = false;
                        firstcomponent = false;

                        while (i < stopOfBody)
                        {
                            string line = lines[i].Replace("\n", "").Replace("\r", "");

                            //if multipart boundary found then exit loop
                            if (Pop3Parse.GetSubHeaderLineType(line, boundary) == Pop3Parse.MultipartBoundaryFound)
                            {
                                boundaryfound = true;
                                ++i;
                                break;
                            }
                            else
                            {
                                ++i;
                            }
                        }
                    }

                    //check to see whether multipart boundary was found
                    if (!boundaryfound)
                    {
                        throw new Pop3MissingBoundaryException("Missing multipart boundary: " + boundary);
                    }

                    bool endofheader = false;

                    //read header info
                    while ((i < stopOfBody))
                    {
                        string line = lines[i].Replace("\n", "").Replace("\r", "");

                        int linetype = Pop3Parse.GetSubHeaderLineType(line, boundary);

                        switch (linetype)
                        {
                            case Pop3Parse.ContentTypeType:
                                contentType = Pop3Parse.ContentType(line);
                                break;
                            case Pop3Parse.ContentTransferEncodingType:
                                contenttransfer = Pop3Parse.ContentTransferEncoding(line);
                                break;
                            case Pop3Parse.ContentDispositionType:
                                disposition = Pop3Parse.ContentDisposition(line);
                                break;
                            case Pop3Parse.ContentDescriptionType:
                                description = Pop3Parse.ContentDescription(line);
                                break;
                            case Pop3Parse.EndOfHeader:
                                endofheader = true;
                                break;
                        }

                        ++i;

                        if (endofheader)
                        {
                            break;
                        }
                        else
                        {
                            while (i < stopOfBody)
                            {
                                //if more lines to read for this line
                                if (line.Substring(line.Length - 1, 1).Equals(";"))
                                {
                                    string nextline = lines[i].Replace("\r", "").Replace("\n", "");

                                    switch (Pop3Parse.GetSubHeaderNextLineType(nextline))
                                    {
                                        case Pop3Parse.NameType:
                                            name = Pop3Parse.Name(nextline);
                                            break;
                                        case Pop3Parse.FilenameType:
                                            filename = Pop3Parse.Filename(nextline);
                                            break;
                                        case Pop3Parse.EndOfHeader:
                                            endofheader = true;
                                            break;
                                    }

                                    if (!endofheader)
                                    {
                                        //point line to current line
                                        line = nextline;
                                        ++i;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    boundaryfound = false;

                    StringBuilder sbText = new StringBuilder();

                    bool emailComposed = false;

                    //store the actual data
                    while (i < stopOfBody)
                    {
                        //get the next line
                        string line = lines[i].Replace("\n", "");

                        //if we've found the boundary
                        if (Pop3Parse.GetSubHeaderLineType(line, boundary) == Pop3Parse.MultipartBoundaryFound)
                        {
                            boundaryfound = true;
                            ++i;
                            break;
                        }
                        else if (Pop3Parse.GetSubHeaderLineType(line, boundary) == Pop3Parse.ComponentsDone)
                        {
                            emailComposed = true;
                            break;
                        }

                        //add this line to data
                        sbText.Append(lines[i]);
                        ++i;
                    }

                    if (sbText.Length > 0)
                    {
                        data = sbText.ToString();
                    }

                    //create a new component
                    m_component.Add(new Pop3Component(contentType, name, filename, contenttransfer, description, disposition, data));

                    //if all multiparts have been composed then exit
                    if (emailComposed)
                    {
                        break;
                    }
                }
            }
        }
    }
}
