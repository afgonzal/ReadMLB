using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReadMLB2020
{
    public class ReadHelper
    {
        public static char[] Separator = new char[] { ',' };
        public static void ReadList(string sourceFileName, string header, Int16 linesToSkip, Int16 firstNameField, Int16 lastNameField, bool numerate, string outputFileName, bool isRoster = false)
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
                                if ((fields[firstNameField].ExtractName() != "") ||
                                    ((fields[lastNameField].ExtractName() != "")))
                                {
                                    if (!isRoster)
                                        writer.WriteLine(line);
                                    else 
                                    {
                                        //for roster only write lines that are ROS
                                        if (fields[1] == "\"ROS\"")
                                        {
                                            if (Convert.ToByte(fields[0]) !=0 && Convert.ToByte(fields[0]) != 8 &&
                                                Convert.ToByte(fields[0]) != 49 && Convert.ToByte(fields[0]) != 54) //skip AL and NL all start teams
                                                writer.WriteLine(line);
                                        }
                                    }
                                }
                                else 
                                {
                                    if (!isRoster)
                                        keepReading = false;
                                    else //except roster that ends in a different way
                                    {
                                        keepReading = Convert.ToByte(fields[0]) < 93;
                                    }
                                }
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
