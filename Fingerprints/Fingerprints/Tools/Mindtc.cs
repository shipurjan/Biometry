using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using ExceptionLogger;
using Fingerprints.Tools.Importers;
using Fingerprints.ViewModels;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Fingerprints.Tools
{
    class Mindtc
    {
        private string MindtcPath { get; }
        private string AppDirectoryPath { get; }
        private string tempDirectoryPath { get; }
        private DrawingService DrawingService { get; }
        public Mindtc()
        {
            try
            {
                string appLocation = System.Reflection.Assembly.GetEntryAssembly().Location;
                AppDirectoryPath = Path.GetDirectoryName(appLocation);
                MindtcPath = Path.Combine(AppDirectoryPath, "Additionals", "mindtct.exe");
                tempDirectoryPath = Path.Combine(AppDirectoryPath, "Additionals", "Temp");
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void Identify(string _ImagePath)
        {
            ImportResult importResult = null;
            string imagePath = "";
            try
            {
                Directory.CreateDirectory(tempDirectoryPath);

                imagePath = PrepareImageAndReturnPath(_ImagePath);

                var mindtcProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = MindtcPath,
                        Arguments = string.Format("{0} {1}", imagePath, Path.Combine(tempDirectoryPath, Path.GetFileNameWithoutExtension(imagePath))),
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        RedirectStandardError = true,
                    }
                };

                mindtcProcess.Start();
                mindtcProcess.EnableRaisingEvents = true;

                mindtcProcess.BeginErrorReadLine();
                mindtcProcess.BeginOutputReadLine();
                mindtcProcess.ErrorDataReceived += (s, e) =>
                {
                    if (e.Data != null)
                    {
                        Debug.WriteLine(e.Data);
                    }
                };
                mindtcProcess.OutputDataReceived += (s, e) =>
                {
                    if (e.Data != null)
                    {
                        Debug.WriteLine(e.Data);
                    }
                };
                mindtcProcess.Disposed += (s, e) =>
                {

                };

                mindtcProcess.Exited += (s, e) =>
                {
                    importResult = ImporterService.Import(Path.Combine(tempDirectoryPath, Path.GetFileNameWithoutExtension(imagePath) + ".xyt"));
                };
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            finally
            {
                //Directory.Delete(tempDirectoryPath, recursive:true);
            }
        }

        private string PrepareImageAndReturnPath(string _ImagePath)
        {
            string result = "";

            string imageName = Path.GetFileName(_ImagePath);
            try
            {
                result = Path.Combine(tempDirectoryPath, imageName);
                Mat imageMat = new Mat(_ImagePath);

                imageMat.ToImage<Gray, Byte>().Save(result);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
                result = "";
            }
            return result;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
}
