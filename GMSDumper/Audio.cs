using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMSDumper
{
    class Audio
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
                Console.WriteLine(e.Message + "\n Failed to read byte.");
                return new byte();
            }
        }
        public static List<byte[]> getAudio()
        {
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
            List<byte[]> l = new List<byte[]>();
            List<byte> pre = new List<byte>();
            int length = (int)br.BaseStream.Length;
            int pos = 0;
            int pattern = 0;
            int stage = 0;
            bool chunkfound = false;
            while (pos < length)
            {
                byte b = readByte(br);
                pos++;
                if (chunkfound)
                {
                    if (stage == 1) //reading WAV and adding to list
                    {
                        pre.Add(b);
                        if (pattern == 3)
                        {
                            if (b == 0x46)
                            {
                                pattern = 0;
                                for(int i = 0; i < 4; i++)
                                    pre.RemoveAt(pre.Count-1);
                                l.Add(pre.ToArray());

                                pre = new List<byte>();
                                pre.Add(0x52);
                                pre.Add(0x49);
                                pre.Add(0x46);
                                pre.Add(0x46);
                            }
                            else
                            {
                                pattern = 0;
                            }
                        }
                        if (pattern == 2)
                        {
                            if (b == 0x46)
                            {
                                pattern++;
                            }
                            else
                            {
                                pattern = 0;
                            }
                        }
                        if (pattern == 1)
                        {
                            if (b == 0x49)
                            {
                                pattern++;
                            }
                            else
                            {
                                pattern = 0;
                            }
                        }
                        if (pattern == 0)
                        {
                            if (b == 0x52)
                            {
                                pattern++;
                            }
                            else
                            {
                                pattern = 0;
                            }
                        }
                    }
                    if (stage == 0) //looking for WAV header. (note: only used once I think)
                    {
                        if (pattern == 3)
                        {
                            if (b == 0x46)
                            {
                                pattern = 0;
                                stage = 1;
                                pre = new List<byte>();
                                pre.Add(0x52);
                                pre.Add(0x49);
                                pre.Add(0x46);
                                pre.Add(0x46);

                                Console.WriteLine("Found first WAV.");
                            }
                            else
                            {
                                pattern = 0;
                            }
                        }
                        if (pattern == 2)
                        {
                            if (b == 0x46)
                            {
                                pattern++;
                            }
                            else
                            {
                                pattern = 0;
                            }
                        }
                        if (pattern == 1)
                        {
                            if (b == 0x49)
                            {
                                pattern++;
                            }
                            else
                            {
                                pattern = 0;
                            }
                        }
                        if (pattern == 0)
                        {
                            if (b == 0x52)
                            {
                                pattern++;
                            } else
                            {
                                pattern = 0;
                            }
                        }
                    }
                }
                if (!chunkfound)
                {
                    if (pattern == 3)
                    {
                        if (b == 0x4F)
                        {
                            pattern = 0;
                            chunkfound = true;
                            stage = 0;
                            Console.WriteLine("Found AUDO chunk.");
                        }
                        else
                        {
                            pattern = 0;
                        }
                    }
                    if (pattern == 2)
                    {
                        if (b == 0x44)
                        {
                            pattern++;
                        }
                        else
                        {
                            pattern = 0;
                        }
                    }
                    if (pattern == 1)
                    {
                        if (b == 0x55)
                        {
                            pattern++;
                        }
                        else
                        {
                            pattern = 0;
                        }
                    }
                    if (pattern == 0)
                    {
                        if (b == 0x41)
                        {
                            pattern++;
                        } else
                        {
                            pattern = 0;
                        }
                    }
                }
            }
            br.Close();
            return l;
        }
    }
}
