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
        public static string LeftImagePath;
        public static string RightImagePath;
        public static void Save()
        {
            if (LeftImagePath != null)
            {
                deleteAllEmptyTypes(ListL);
                using (StreamWriter writerL = new StreamWriter(LeftImagePath))
                {
                    foreach (var item in ListL)
                    {
                        writerL.WriteLine(item);
                    }
                }
            }

            if (RightImagePath != null)
            {
                deleteAllEmptyTypes(ListR);
                using (StreamWriter writerR = new StreamWriter(RightImagePath))
                {
                    foreach (var item in ListR)
                    {
                        writerR.WriteLine(item);
                    }
                }
            }
        }
        public static void LoadLeftFile()
        {
            if (File.Exists(LeftImagePath))
            {
                using (StreamReader readerL = new StreamReader(LeftImagePath))
                {
                    while (!readerL.EndOfStream)
                    {
                        ListL.Add(readerL.ReadLine());
                    }
                }
            }
        }

        public static void LoadRightFile()
        {
            if (File.Exists(RightImagePath))
            {
                using (StreamReader readerR = new StreamReader(RightImagePath))
                {
                    while (!readerR.EndOfStream)
                    {
                        ListR.Add(readerR.ReadLine());
                    }
                }
            }
        }

        private static void deleteAllEmptyTypes(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (getNameFromListElement(list[i]) == "Puste")
                {
                    list.Remove(list[i]);
                    i -= 1;
                }
            }
        }

        private static string getNameFromListElement(string listElement)
        {
            return listElement.Split(';')[1];
        }
    }
}
