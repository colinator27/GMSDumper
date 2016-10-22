using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMSDumper
{
    class Strings
    {
        private static byte readByte(BinaryReader reader)
        {
            try
            {
                byte b = reader.ReadByte();
                return b;
            }
            catch (IOException e)
            {
                return new byte();
            }
        }
        public static List<byte[]> getStrings()
        {
            List<byte[]> l = new List<byte[]>();
            BinaryReader br;
            try
            {
                br = new BinaryReader(new FileStream("data.win", FileMode.Open));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot open file.");
                return null;
            }
            int length = (int)br.BaseStream.Length;
            int pos = 0;
            int pattern = 0;
            int stage = 0;
            int strlength = 0;
            int count = 0;
            List<byte> pre = new List<byte>();
            bool chunkfound = false;
            while (pos < length)
            {
                byte b = readByte(br);
                pos++;
                if (chunkfound)
                {
                    if (stage == 2)
                    {
                        if (b != 0x00)
                        {
                            strlength--;
                            pre.Add(b);
                        } else
                        {
                            stage = 1;
                            count = 0;
                            l.Add(pre.ToArray());
                            pre = new List<byte>();
                        }
                    }
                    if (stage == 1) //looking for start of string
                    {
                        if (count != 1 && count < 20)
                        {
                            if(b != 0x00)
                            {
                                stage = 2;
                                pre.Add(b);
                            }
                        }
                        if (count >= 20)
                        {
                            Console.WriteLine("Found last string. End of STRG.");
                            pos = length + 1;
                        }
                        count++;
                    }
                    if (stage == 0) //looking for first item: always called "prototype"
                    {
                        //we'll just look for "pro" instead.
                        if (pattern == 2 && b != 0x6F)
                        {
                            pattern = 0;
                        }
                        if (pattern == 2 && b == 0x6F)
                        {
                            pattern = 0;
                            stage = 1;
                            strlength = 6;
                            Console.WriteLine("Found first string. Starting to read strings...");
                            pre.Add(0x70);
                            pre.Add(0x72);
                            pre.Add(0x6F);
                        }
                        if (pattern == 1 && b != 0x72)
                        {
                            pattern = 0;
                        }
                        if (pattern == 1 && b == 0x72)
                        {
                            pattern++;
                        }
                        if (pattern == 0 && b != 0x70)
                        {
                            pattern = 0;
                        }
                        if (pattern == 0 && b == 0x70)
                        {
                            pattern++;
                        }
                    }
                }
                if (!chunkfound)
                {
                    if (pattern <= 4 && chunkfound == false) //searching for chunk
                    {

                        if (pattern == 4 && b != 0x47)
                        {
                            pattern = 0;
                        }
                        if (pattern == 4 && b == 0x47)
                        {
                            pattern = 0;
                            chunkfound = true;
                            stage = 0;
                            Console.WriteLine("Found STRG chunk.");
                        }
                        if (pattern == 3 && b != 0x52)
                        {
                            pattern = 0;
                        }
                        if (pattern == 3 && b == 0x52)
                        {
                            pattern++;
                        }
                        if (pattern == 2 && b != 0x54)
                        {
                            pattern = 0;
                        }
                        if (pattern == 2 && b == 0x54)
                        {
                            pattern++;
                        }
                        if (pattern == 1 && b == 0x53)
                        {
                            pattern++;
                        }
                        if (pattern == 0 && b != 0x00)
                        {
                            pattern = 0;
                        }
                        if (pattern == 0 && b == 0x00)
                        {
                            pattern++;
                        }
                    }
                }
            }
            br.Close();
            return l;
        }
    }
}
