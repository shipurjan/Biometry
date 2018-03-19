using ExceptionLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using System.IO;
using System.Configuration;

namespace Fingerprints.Tools.SqlLocalDB
{
    public class SqlLocalDB_Utils
    {
        private static string localDBName = "myinstance";

        private List<string> outputDataBuffer;

        public SqlLocalDB_Utils()
        {
            try
            {
                outputDataBuffer = new List<string>();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public bool CheckIfInstanceExists()
        {
            bool result = false;
            ProcessExecutor process;
            try
            {
                outputDataBuffer.Clear();

                process = new ProcessExecutor(InstanceCheck_Process_Exit, InstanceCheck_Process_OutputData, InstanceCheck_Process_ErrorData);
                process.StartProcess_WithWait("sqllocaldb.exe", "info");

                if (!outputDataBuffer.Exists(x => x.Equals("sqllocaldb.exe", StringComparison.OrdinalIgnoreCase)))
                {
                    FingerContext.LocalDB_Name = outputDataBuffer.FirstOrDefault();
                    result = true;
                }

                process.Dispose();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        private void InstanceCheck_Process_ErrorData(object sender, DataReceivedEventArgs e)
        {
            try
            {
                if (e.Data != null)
                {
                    MessageBox.Show(e.Data);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void InstanceCheck_Process_OutputData(object sender, DataReceivedEventArgs e)
        {
            try
            {
                if (e.Data != null)
                {
                    outputDataBuffer.Add(e.Data);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void InstanceCheck_Process_Exit(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public static bool IsLocalDBInstalled()
        {
            bool result = false;
            try
            {
                result = ExistsOnPath("SqlLocalDB.exe");
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        public static bool ExistsOnPath(string fileName)
        {
            bool result = false;
            try
            {
                result =  GetFullPath(fileName) != null;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        public static string GetFullPath(string fileName)
        {
            string result = string.Empty;
            try
            {
                if (File.Exists(fileName))
                    return Path.GetFullPath(fileName);

                var values = Environment.GetEnvironmentVariable("PATH");
                foreach (var path in values.Split(';'))
                {
                    var fullPath = Path.Combine(path, fileName);
                    if (File.Exists(fullPath))
                        return fullPath;
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
