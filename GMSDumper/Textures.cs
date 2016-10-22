using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMSDumper
{
    class Textures
    {
        private static byte readByte(BinaryReader reader)
        {
            try {
                byte b = reader.ReadByte();
                return b;
            }
            catch (IOException e)
            {
                return new byte();
            }
        }
        public static List<byte[]> getTextures()
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
            int length = (int)br.BaseStream.Length;
            int pos = 0;
            int pattern = 0;
            int pattern2 = 0;
            bool chunkfound = false;
            int stage = 0;
            int imgstage = 0;
            List<byte[]> l = new List<byte[]>();
            List<byte> image = new List<byte>();
            while (pos < length)
            {
                byte b = readByte(br);
                pos++;
                if (chunkfound)
                {
                    if (stage == 2) //reading png and adding to list
                    {
                        if (imgstage == 1)
                        {
                            image.Add(b);
                            if (pattern == 0 && b == 0x49)
                            {
                                pattern++;
                            } else
                            if (pattern == 1 && b == 0x45)
                            {
                                pattern++;
                            } else
                            if (pattern == 2 && b == 0x4E)
                            {
                                pattern++;
                            } else
                            if (pattern == 3 && b == 0x44)
                            {
                                pattern++;
                            } else
                            if (pattern == 4 && b == 0xAE)
                            {
                                pattern++;
                            } else
                            if (pattern == 5 && b == 0x42)
                            {
                                pattern++;
                            } else
                            if (pattern == 6 && b == 0x60)
                            {
                                pattern++;
                            } else
                            if (pattern == 7 && b == 0x82)
                            {
                                pattern = 0;
                                l.Add(image.ToArray());
                                Console.WriteLine("Reading image " + (l.Count-1).ToString() + ".");
                                stage = 1;
                            } else
                            if (pattern > 0)
                            {
                                pattern = 0;
                            }
                        }
                        if (imgstage == 0)
                        {
                            image.Add(0x89);
                            image.Add(0x50);
                            image.Add(0x4E);
                            image.Add(0x47);
                            image.Add(0x0D);
                            pattern = 0;
                            imgstage = 1;
                        }
                    }
                    if (stage == 1) //looking for header of png
                    {
                        if (pattern == 0 && b == 0x89)
                        {
                            pattern++;
                        } else
                        if (pattern == 1 && b == 0x50)
                        {
                            pattern++;
                        } else
                        if (pattern == 2 && b == 0x4E)
                        {
                            pattern++;
                        } else
                        if (pattern == 3 && b == 0x47)
                        {
                            pattern = 0;
                            stage = 2;
                            image = new List<byte>();
                            imgstage = 0;
                        } else
                        if (pattern > 0)
                        {
                            pattern = 0;
                        }
                        if (pattern2 == 0 && b == 0x41) //finding AUDO chunk
                        {
                            pattern2++;
                        } else
                        if (pattern2 == 1 && b == 0x55)
                        {
                            pattern2++;
                        } else
                        if (pattern2 == 2 && b == 0x44)
                        {
                            pattern2++;
                        } else
                        if (pattern2 == 3 && b == 0x4F)
                        {
                            pattern2 = 0;
                            pos = length + 1;
                            Console.WriteLine("Found end of TXTR chunk.");
                        } else
                        if (pattern2 > 0)
                        {
                            pattern2 = 0;
                        }
                    }
                    if (stage == 0) //finding where to start
                    {
                        if (pattern < 30 && b == 0x00)
                        {
                            pattern++;
                        } else
                        if (pattern == 30 && b == 0x00)
                        {
                            pattern = 0;
                            stage = 1;
                            pattern2 = 0;
                            Console.WriteLine("Reading textures...");
                        } else
                        if (pattern > 0)
                        {
                            pattern = 0;
                        }
                    }
                }
                if (pattern <= 24 && chunkfound == false) //searching for chunk
                {
                    
                    if (pattern == 24 && b != 0x52)
                    {
                        pattern = 0;
                    }
                    if (pattern == 24 && b == 0x52)
                    {
                        pattern = 0;
                        chunkfound = true;
                        Console.WriteLine("Found TXTR chunk.");
                    }
                    if (pattern == 23 && b != 0x54)
                    {
                        pattern = 0;
                    }
                    if (pattern == 23 && b == 0x54)
                    {
                        pattern++;
                    }
                    if (pattern == 22 && b != 0x58)
                    {
                        pattern = 0;
                    }
                    if (pattern == 22 && b == 0x58)
                    {
                        pattern++;
                    }
                    if (pattern == 21 && b == 0x54)
                    {
                        pattern++;
                    }
                    if (pattern <= 20 && b != 0x00)
                    {
                        pattern = 0;
                    }
                    if (pattern <= 20 && b == 0x00)
                    {
                        pattern++;
                    }
                }
            }
            br.Close();
            return l;
        }
    }
}
