using System;
using System.Text.RegularExpressions;

namespace Pop3
{
    public class Pop3Statics
    {
        public const string DataFolder = @"Pop3Temp";

        public static string FromQuotedPrintable(string inString)
        {
            string outputString = null;
            string inputString = inString.Replace("=\n", "").Replace("=20\n", " ");

            if (inputString.Length > 3)
            {
                outputString = "";

                for (int x = 0; x < inputString.Length; )
                {
                    string s1 = inputString.Substring(x, 1);

                    if ((s1.Equals("=")) && ((x + 2) < inputString.Length))
                    {
                        string hexstring = inputString.Substring(x + 1, 2);

                        if (Regex.Match(hexstring.ToUpper(), @"^[A-F|0-9]+[A-F|0-9]+$").Success)
                        {
                            outputString += System.Text.Encoding.ASCII.GetString(new byte[] { System.Convert.ToByte(hexstring, 16) });
                            x += 3;
                        }
                        else
                        {
                            outputString += s1;
                            ++x;
                        }
                    }
                    else
                    {
                        outputString += s1;
                        ++x;
                    }
                }
            }
            else
            {
                outputString = inputString;
            }

            return outputString.Replace("\n", "\r\n");
        }
    }
}
