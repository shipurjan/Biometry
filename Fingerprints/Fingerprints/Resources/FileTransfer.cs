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
                using (StreamWriter writerL = new StreamWriter(LeftImagePath))
                {
                    foreach (var item in getListWithoutEmptyObjects(ListL))
                    {
                        writerL.WriteLine(item);
                    }
                }
            }

            if (RightImagePath != null)
            {
                using (StreamWriter writerR = new StreamWriter(RightImagePath))
                {
                    foreach (var item in getListWithoutEmptyObjects(ListR))
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

        public static void ConvertToXytAndSave(string path)
        {
            saveVectorToXYT(ListL, getPath(path, LeftImagePath));
            saveVectorToXYT(ListR, getPath(path, RightImagePath));
        }

        private static void saveVectorToXYT(List <string> list, string path)
        {
            using (StreamWriter writerL = new StreamWriter(path))
            {
                foreach (var item in getVectorList(list))
                {
                    writerL.WriteLine(getStringToXYT(item));
                }
            }
        }

        private static string getStringToXYT(string item)
        {
            string[] array = item.Split(';');
            int angle = (int)Math.Round(Convert.ToDouble(array[4]) * 180 / 3.14);
            if (angle < 0)
            {
                angle *= -1;
            }
            else
            {
                angle = 360 - angle;
            }
            return array[2] + " " + array[3] + " " + angle;
        }

        private static string getPath(string path, string choosedFile)
        {
            string[] pathSegments = path.Split('.');
            string[] choosedFileSegments = choosedFile.Split('.');
            string[] tmpSegments = choosedFileSegments[choosedFileSegments.Count() - 2].Split('\\');
            pathSegments[pathSegments.Count() - 2] += "_" + tmpSegments.LastOrDefault() + ".";

            string tmp = "";

            foreach (string segment in pathSegments)
            {
                tmp += segment;
            }

            return tmp;
        }

        private static List<string> getListWithoutEmptyObjects(List<string> list)
        {
            List<string> temp = new List<string>();
            foreach (var item in list)
            {
                if (getNameFromListElement(item) != "Puste")
                {
                    temp.Add(item);
                }
            }

            return temp;
        }

        private static string getNameFromListElement(string listElement)
        {
            return listElement.Split(';')[1];
        }

        private static List<string> getVectorList(List<string> list)
        {
            List<SelfDefinedMinutiae> minutiaeList = new MinutiaeTypeController().GetAllMinutiaeTypes();
            List<string> vectorsList = new List<string>();
            foreach (var item in list)
            {
                string[] tmp = item.Split(';');
                var type = minutiaeList.Where(x => x.Name == tmp[1]).FirstOrDefault();

                if (type != null && type.TypeId == 2)
                {
                    vectorsList.Add(item);
                }
            }

            return vectorsList;
        }
    }
}
