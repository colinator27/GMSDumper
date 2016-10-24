using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMSDumper
{
    class Program
    {
        static void Main(string[] args)
        {
            List<byte[]> list3 = Audio.getAudio();
            List<byte[]> list2 = Strings.getStrings();
            List<byte[]> list = Textures.getTextures();
            Console.WriteLine("Exporting images...");
            if (list != null)
            for (int i = 0; i < list.Count; i++){
                try {
                    File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/texture" + i.ToString() + ".png", list[i]);
                } catch (IOException e)
                {
                    Console.WriteLine(e.Message + "\n Failed to write file.");
                }
                Console.WriteLine("Exporting image "+i.ToString());
            }
            Console.WriteLine("Exporting audio...");
            if (list3 != null)
            for (int i = 0; i < list3.Count; i++)
            {
                try {
                    File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/sound_" + i.ToString() + ".wav", list3[i]);
                } catch (IOException e)
                {
                    Console.WriteLine(e.Message + "\n Failed to write file.");
                }
                Console.WriteLine("Exporting audio " + i.ToString());
            }
            Console.WriteLine("Exporting strings...");
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/strings.txt", String.Empty);
            if (list2 != null)
            for (int i = 0; i < list2.Count; i++)
            {
                try
                {
                    if ((i + 1) < list2.Count)
                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "/strings.txt", System.Text.Encoding.Default.GetString(list2[i]) + Environment.NewLine);
                    if ((i + 1) >= list2.Count)
                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "/strings.txt", System.Text.Encoding.Default.GetString(list2[i]));
                    } catch (IOException e)
                {
                    Console.WriteLine(e.Message + "\n Can't write to strings.txt.");
                }
            }
            Console.WriteLine("Done. Any key to continue...");
            Console.ReadKey();
        }
    }
}
