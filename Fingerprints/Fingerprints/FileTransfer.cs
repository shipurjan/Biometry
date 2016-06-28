using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints
{
    public static class FileTransfer
    {
        public static List<string> ListL = new List<string>();
        public static List<string> ListR = new List<string>();
        public static void Save()
        {
            //streamWriter(ListL, "cordsLeft.txt");
            //streamWriter(ListR, "cordsRight.txt");
            using (StreamWriter writerL = new StreamWriter("cordsLeft.txt"))
            {
                foreach (var item in ListL)
                {
                    writerL.WriteLine(item);
                }
            }
            using (StreamWriter writerR = new StreamWriter("cordsRight.txt"))
            {
                foreach (var item in ListR)
                {
                    writerR.WriteLine(item);
                }
            }
        }

        //static private void streamWriter(List<string> list, string path)
        //{
        //    using (StreamWriter writer = new StreamWriter(path))
        //    {
        //        foreach (var item in list)
        //        {
        //            writer.WriteLine(item);
        //        }
        //    }
        //}
        public static void Load()
        {

        }
    }
}
