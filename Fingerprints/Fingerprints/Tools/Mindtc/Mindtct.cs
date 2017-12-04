using Emgu.CV;
using Emgu.CV.Structure;
using ExceptionLogger;
using Fingerprints.Tools.Importers;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Fingerprints.Tools.Mindtc
{
    class Mindtct : IDisposable
    {
        /// <summary>
        /// Event informing that detection is complete
        /// </summary>
        public event DetectionComplatedDelegate DetectionCompleted;

        /// <summary>
        /// Delegate used in event
        /// </summary>
        /// <param name="_result">Result of detection</param>
        public delegate void DetectionComplatedDelegate(ImportResult _result);

        /// <summary>
        /// mindtc console process
        /// </summary>
        private Process mindtcProcess;

        /// <summary>
        /// mindtc Task for asynchronous operation
        /// </summary>
        private Task mindtcTask;

        /// <summary>
        /// Path to temporary image
        /// </summary>
        private string PreparedImagePath { set; get; }

        /// <summary>
        /// Path to mindtc app
        /// </summary>
        private string MindtcPath { get; }

        /// <summary>
        /// Path to main directory of application
        /// </summary>
        private string AppDirectoryPath { get; }

        /// <summary>
        /// path to temp directory used in detection
        /// </summary>
        private string tempDirectoryPath { get; }


        public Mindtct()
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

        /// <summary>
        /// Starts asynchronous image detection
        /// </summary>
        /// <param name="_ImagePath"></param>
        public void DetectImage(string _ImagePath)
        {
            mindtcTask = Task.Run(() =>
            {
                try
                {
                    //Create temporary directory
                    Directory.CreateDirectory(tempDirectoryPath);

                    //Prepares image, saves in 8bits depth
                    PreparedImagePath = PrepareImageAndReturnPath(_ImagePath);

                    //Starts mindtc process
                    StartProcess();
                }
                catch (Exception ex)
                {
                    Logger.WriteExceptionLog(ex);
                }
            });
        }

        /// <summary>
        /// Starts mindtc process, initializes events 
        /// </summary>
        public void StartProcess()
        {
            try
            {
                mindtcProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = MindtcPath,
                        Arguments = string.Format("{0} {1}", PreparedImagePath, Path.Combine(tempDirectoryPath, Path.GetFileNameWithoutExtension(PreparedImagePath))),
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

        /// <summary>
        /// mindtcProcess event, occurs when process ended
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Process_Exited(object sender, EventArgs e)
        {
            try
            {
                //import data from temporary file
                ImportResult importResult = ImporterService.Import(Path.Combine(tempDirectoryPath, Path.GetFileNameWithoutExtension(PreparedImagePath) + ".xyt"));

                //fire event
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
                //delete temporary directory
                //Directory.Delete(tempDirectoryPath, recursive: true);
            }
        }

        /// <summary>
        /// mindtc Process event, occurs when console output received data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// mindtc Process event, occurs when console output received error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Prepares Image for mindtc Detection, saves images in 8bits depth
        /// </summary>
        /// <param name="_ImagePath"></param>
        /// <returns></returns>
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
                        mindtcProcess.ErrorDataReceived -= Process_ErrorDataReceived;
                        mindtcProcess.OutputDataReceived -= Process_OutputDataReceived;
                        mindtcProcess.Exited -= Process_Exited;

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
