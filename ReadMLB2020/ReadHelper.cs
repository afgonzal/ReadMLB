using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReadMLB2020
{
    public class ReadHelper
    {
        public static char[] Separator = new char[] { ',' };
        public static void ReadList(string sourceFileName, string header, Int16 linesToSkip, Int16 firstNameField, Int16 lastNameField, bool numerate, string outputFileName)
        {
            using (TextReader reader = new StreamReader(sourceFileName))
            {
                using (TextWriter writer = new StreamWriter(outputFileName))
                {

                    string line;
                    bool keepReading = true;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string firstLine = line.Split(Separator)[0];
                        //go until you find the header of the section
                        if (firstLine == header)
                        {
                            //tw.WriteLine(line);
                            //for (Int16 i = 0; i < extraTitleLines; i++)
                            //{
                            //    line = tr.ReadLine();
                            //    tw.WriteLine(line);
                            //}
                            //skip certain lines like extra header lines etc
                            for (Int16 i = 0; i < linesToSkip; i++)
                            {
                                line = reader.ReadLine();
                            }

                            Int64 playerNumber = 1;
                            while ((line = reader.ReadLine()) != null && keepReading)
                            {
                                if (numerate)
                                {
                                    line = playerNumber.ToString() + "," + line;
                                    playerNumber++;
                                }
                                string[] fields = line.Split(Separator, StringSplitOptions.None);
                                if ((fields[firstNameField].Replace("\"", "").Trim() != "") || ((fields[lastNameField].Replace("\"", "").Trim() != "")))
                                    writer.WriteLine(line);
                                else
                                    keepReading = false;
                            }
                        }
                    }
                    writer.Flush();
                    writer.Close();
                }
                reader.Close();
            }
        }
    }
}
