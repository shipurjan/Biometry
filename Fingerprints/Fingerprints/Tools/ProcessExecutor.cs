using ExceptionLogger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fingerprints.Tools
{
    public delegate void Process_Exited(object sender, EventArgs e);
    public delegate void Process_OutputDataReceived(object sender, DataReceivedEventArgs e);
    public delegate void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e);

    public class ProcessExecutor : IDisposable
    {
        private Process_Exited onProcess_Exit;
        private Process_OutputDataReceived onProcess_OutputDataReceive;
        private Process_ErrorDataReceived onProcess_ErrorDataReceive;

        private Process applicationProcess;

        /// <summary>
        /// constructor of Process executor class
        /// </summary>
        /// <param name="_onProcessExit">this delegate will be called on process exit</param>
        /// <param name="_onProcess_OutputDataReceive">this delegate will be called when process returns some data (can be called multiple times on process execution)</param>
        /// <param name="_onProcess_ErrorDataReceive">this delegate will be called when process returns errors (can be called multiple times on process execution)</param>
        public ProcessExecutor(Process_Exited _onProcessExit, Process_OutputDataReceived _onProcess_OutputDataReceive, Process_ErrorDataReceived _onProcess_ErrorDataReceive)
        {
            try
            {
                onProcess_Exit = _onProcessExit;
                onProcess_OutputDataReceive = _onProcess_OutputDataReceive;
                onProcess_ErrorDataReceive = _onProcess_ErrorDataReceive;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Starts process, needs exe name and optional parameters
        /// </summary>
        /// <param name="_fileName">name of exe e.g sqllocaldb.exe</param>
        /// <param name="_parameters">parameters e.g info</param>
        public void StartProcess_WithWait(string _fileName, string _parameters = "")
        {
            try
            {
                InitializeAndStartProcess(_fileName, _parameters);
                applicationProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Initializes process object and starts its
        /// </summary>
        /// <param name="_fileName"></param>
        /// <param name="_parameters"></param>
        private void InitializeAndStartProcess(string _fileName, string _parameters)
        {
            try
            {
                applicationProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = _fileName,
                        Arguments = _parameters,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        RedirectStandardError = true,
                    }
                };

                applicationProcess.Start();
                applicationProcess.EnableRaisingEvents = true;
                applicationProcess.BeginErrorReadLine();
                applicationProcess.BeginOutputReadLine();

                applicationProcess.ErrorDataReceived += ErrorDataReceved;
                applicationProcess.OutputDataReceived += OutputDataReceived;
                applicationProcess.Exited += Exited;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// launches delegate on process exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exited(object sender, EventArgs e)
        {
            try
            {
                onProcess_Exit(sender, e);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// launches delegate when process returns data (can be called multiple times on process execution)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                onProcess_OutputDataReceive(sender, e);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// launches delegate when process returns errors (can be called mutliple times on process execution)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ErrorDataReceved(object sender, DataReceivedEventArgs e)
        {
            try
            {
                onProcess_ErrorDataReceive(sender, e);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
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
                        applicationProcess?.Dispose();
                    }

                    disposedValue = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        // This code added to correctly implement the disposable pattern.
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
