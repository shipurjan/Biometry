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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Fingerprints.Tools
{
    class Mindtc : IDisposable
    {
        public event DetectionComplatedDelegate DetectionCompleted;
        public delegate void DetectionComplatedDelegate(ImportResult _result);

        private Process mindtcProcess;

        private Task mindtcTask;

        private string PreparedImagePath { set; get; }

        private string MindtcPath { get; }
        private string AppDirectoryPath { get; }
        private string tempDirectoryPath { get; }
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

        public void DetectImage(string _ImagePath)
        {
            mindtcTask = Task.Run(() =>
            {
                try
                {
                    Directory.CreateDirectory(tempDirectoryPath);

                    PreparedImagePath = PrepareImageAndReturnPath(_ImagePath);

                    StartProcess();
                }
                catch (Exception ex)
                {
                    Logger.WriteExceptionLog(ex);
                }
            });
        }

        public void StartProcess()
        {
            try
            {
                mindtcProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = MindtcPath,
                        Arguments = string.Format("-m1 {0} {1}", PreparedImagePath, Path.Combine(tempDirectoryPath, Path.GetFileNameWithoutExtension(PreparedImagePath))),
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


                mindtcProcess.ErrorDataReceived += Process_ErrorDataReceived;
                mindtcProcess.OutputDataReceived += Process_OutputDataReceived;

                mindtcProcess.Exited += Process_Exited;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            try
            {
                ImportResult importResult = ImporterService.Import(Path.Combine(tempDirectoryPath, Path.GetFileNameWithoutExtension(PreparedImagePath) + ".xyt"));

                Application.Current.Dispatcher.Invoke(() =>
                {
                    DetectionCompleted(importResult);
                });
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            finally
            {
                Directory.Delete(tempDirectoryPath, recursive: true);
                Dispose();
            }
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                if (e.Data != null)
                {
                    Debug.WriteLine(e.Data);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                if (e.Data != null)
                {
                    Debug.WriteLine(e.Data);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        mindtcProcess?.Dispose();
                        mindtcProcess = null;

                        mindtcTask?.Dispose();
                        mindtcTask = null;
                    }

                    disposedValue = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void Dispose()
        {
            try
            {
                Dispose(true);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
        #endregion
    }
}
