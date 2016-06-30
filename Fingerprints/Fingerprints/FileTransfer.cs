﻿using System;
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
            using (StreamWriter writerL = new StreamWriter(LeftImagePath))
            {
                foreach (var item in ListL)
                {
                    writerL.WriteLine(item);
                }
            }
            using (StreamWriter writerR = new StreamWriter(RightImagePath))
            {
                foreach (var item in ListR)
                {
                    writerR.WriteLine(item);
                }
            }
        }
        public static void LoadLeftFile()
        {
            using (StreamReader readerL = new StreamReader(LeftImagePath))
            {
                while (!readerL.EndOfStream)
                {
                    ListL.Add(readerL.ReadLine());
                }
            }
        }

        public static void LoadRightFile()
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
}
