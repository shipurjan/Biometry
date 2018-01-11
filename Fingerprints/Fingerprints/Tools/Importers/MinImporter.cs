using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fingerprints.Models;
using ExceptionLogger;
using System.IO;
using System.Windows;

namespace Fingerprints.Tools.Importers
{
    class MinImporter : ImporterBase, IDataImporter
    {
        private SelfDefinedMinutiae vectorMinutia;

        public MinImporter() : base()
        {
            try
            {
                vectorMinutia = GetSelfDefinedMinutiaOrCreate("mindtct(.min)");
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public List<MinutiaFileState> GetformattedData()
        {
            List<MinutiaFileState> result = null;
            try
            {
                result = new List<MinutiaFileState>();

                using (StringReader reader = new StringReader(fileContent))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var columns = MinColumns.GetMinRow(line);

                        if (columns != null)
                        {
                            result.Add(NewFileState(columns));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        public void Import(string _path)
        {
            try
            {
                fileContent = FileTransfer.LoadFile(_path);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
        private MinutiaFileState NewFileState(MinRow _row)
        {
            MinutiaFileState result = null;
            try
            {
                result = new MinutiaFileState();

                result.Angle = _row.Dir;
                result.Points.Add(new Point(_row.Mx, _row.My));
                result.Name = vectorMinutia.Name;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
                result = null;
            }
            return result;
        }
    }
}
