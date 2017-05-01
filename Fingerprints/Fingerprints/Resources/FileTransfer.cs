using Fingerprints.Models;
using Fingerprints.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public static List<MinutiaState> ListL = new List<MinutiaState>();
        public static List<MinutiaState> ListR = new List<MinutiaState>();
        public static string LeftImagePath;
        public static string RightImagePath;
        public static void Save()
        {
            if (LeftImagePath != null)
            {
                using (StreamWriter writerL = new StreamWriter(LeftImagePath))
                {
                    List<MinutiaState> states = getListWithoutEmptyObjects(ListL);
                    writerL.Write(JsonConvert.SerializeObject(states.Select(x => x.ToMinutiaFileState()), Formatting.Indented));
                }
            }

            if (RightImagePath != null)
            {
                using (StreamWriter writerR = new StreamWriter(RightImagePath))
                {
                    List<MinutiaState> states = getListWithoutEmptyObjects(ListR);
                    writerR.Write(JsonConvert.SerializeObject(states.Select(x => x.ToMinutiaFileState()), Formatting.Indented));
                }
            }
        }

        public static void SaveFile(string path, List<string> list)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach (var item in list)
                {
                    writer.WriteLine(item);
                }
            }
        }
        public static void LoadLeftFile()
        {
            if (File.Exists(LeftImagePath))
            {
                using (StreamReader readerL = new StreamReader(LeftImagePath))
                {
                    string file = readerL.ReadToEnd();
                    List<MinutiaFileState> minutiaeList = JsonConvert.DeserializeObject<List<MinutiaFileState>>(file);
                    ListL = minutiaeList.Select(x => x.ToMinutiaState()).ToList();
                }
            }
        }

        public static void LoadRightFile()
        {
            if (File.Exists(RightImagePath))
            {
                using (StreamReader readerR = new StreamReader(RightImagePath))
                {
                    string file = readerR.ReadToEnd();
                    List<MinutiaFileState> minutiaeList = JsonConvert.DeserializeObject<List<MinutiaFileState>>(file);
                    ListR = minutiaeList.Select(x => x.ToMinutiaState()).ToList();
                    //while (!readerR.EndOfStream)
                    //{
                    //    ListR.Add(readerR.ReadLine());
                    //}
                }
            }
        }

        public static void ConvertToXytAndSave(string path)
        {
            //Transformer transformer = new Transformer();
            //if (ListL.Count() != 0)
            //    SaveFile(getPath(path, LeftImagePath), transformer.getBozorthFormat(ListL));
            //if (ListR.Count() != 0)
            //    SaveFile(getPath(path, RightImagePath), transformer.getBozorthFormat(ListR));
        }

        public static string getPath(string path, string choosedFile)
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

        private static List<MinutiaState> getListWithoutEmptyObjects(List<MinutiaState> list)
        {
            return list.Where(x => x.Minutia.Name != "Puste").ToList();
        }

        private static string getNameFromListElement(string listElement)
        {
            JObject minutia = JsonConvert.DeserializeObject<JObject>(listElement);
            return minutia["name"].ToString();
        }
    }
}
