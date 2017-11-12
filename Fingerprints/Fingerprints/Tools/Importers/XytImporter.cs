using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fingerprints.Models;
using Fingerprints.ViewModels;
using ExceptionLogger;
using System.IO;
using Fingerprints.MinutiaeTypes;
using System.Windows;

namespace Fingerprints.Tools.Importers
{
    class XytImporter : ImporterBase, IDataImporter
    {
        private SelfDefinedMinutiae vectorMinutia;
        public XytImporter() : base()
        {
            using (var db = new FingerContext())
            {
                vectorMinutia = db.SelfDefinedMinutiaes.Where(x => x.DrawingType == DrawingType.Vector).FirstOrDefault();
            }

        }

        public List<MinutiaFileState> GetformattedData()
        {
            List<MinutiaFileState> result = new List<MinutiaFileState>();
            try
            {
                using (StringReader reader = new StringReader(fileContent))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var columns = XytColumns.GetXytRow(line);

                        result.Add(NewFileState(columns));
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

        private MinutiaFileState NewFileState(XytRow _row)
        {
            MinutiaFileState result = new MinutiaFileState();
            try
            {
                result.Angle = _row.Angle;
                result.Points.Add(new Point(_row.X, _row.Y));
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
