using ExceptionLogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Tools
{
    class PathTool
    {
        public static string CombainePathWithName(string path, string choosedFile)
        {
            string result = string.Empty;
            try
            {
                string fileNameFromPath = Path.GetFileNameWithoutExtension(path);
                string fileExtensionFromPath = Path.GetExtension(path);
                string choosedFileName = Path.GetFileNameWithoutExtension(choosedFile);

                string fullName = String.Format("{0}_{1}{2}", fileNameFromPath, choosedFileName, fileExtensionFromPath);

                result = Path.Combine(Path.GetDirectoryName(path), fullName);

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }
    }
}
