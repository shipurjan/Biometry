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
        /// <summary>
        /// Combine Full Path with file name to create name,
        /// for example: 'system32\notepad.exe' and 'image1.jpeg' will produce 'system32\notepad_image1.exe'
        /// </summary>
        /// <param name="path">Full Path to file</param>
        /// <param name="choosedFile">name of file for example image.jpg</param>
        /// <returns></returns>
        public static string CombainePathWithName(string path, string choosedFile)
        {
            string result = string.Empty;
            try
            {
                string fileNameFromPath = Path.GetFileNameWithoutExtension(path);
                string fileExtensionFromPath = Path.GetExtension(path);
                string choosedFileName = Path.GetFileNameWithoutExtension(choosedFile);

                //Combine file names from path and choosedFile
                string fullName = String.Format("{0}_{1}{2}", fileNameFromPath, choosedFileName, fileExtensionFromPath);

                result = Path.Combine(Path.GetDirectoryName(path), fullName);

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        /// <summary>
        /// Returns ImportType from path
        /// If file is not supporter, returns null
        /// </summary>
        /// <param name="_path"></param>
        /// <returns></returns>
        public static ImportTypes? GetImportTypeFromPath(string _path)
        {
            ImportTypes? result = null;
            ImportTypes tempType;
            string extension = string.Empty;
            try
            {
                extension = Path.GetExtension(_path).Trim('.');

                //try parse extension from path to ImportType
                Enum.TryParse<ImportTypes>(extension, out tempType);

                if (tempType != 0)
                {
                    result = tempType;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }
    }
}
