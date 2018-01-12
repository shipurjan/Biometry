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
        private SelfDefinedMinutiae VectorBifurcationAppearing;
        private SelfDefinedMinutiae VectorBifurcationDisappearing;
        private SelfDefinedMinutiae vectorRidgeEndingAppearing;
        private SelfDefinedMinutiae vectorRidgeEndingDisappearing;

        public MinImporter() : base()
        {
            try
            {
                VectorBifurcationAppearing = GetSelfDefinedMinutiaOrCreate("Rozwidlenie (M)");
                VectorBifurcationDisappearing = GetSelfDefinedMinutiaOrCreate("Złączenie (M)");
                vectorRidgeEndingAppearing = GetSelfDefinedMinutiaOrCreate("Początek (M)");
                vectorRidgeEndingDisappearing = GetSelfDefinedMinutiaOrCreate("Zakończenie (M)");
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

                result.Angle = _row.Direction;
                result.Points.Add(new Point(_row.Mx, _row.My));
                result.Name = GetMinutiaName(_row.MinutiaType, _row.FeatureType);
                result.Quantity = Convert.ToInt32((_row.ReliabilityMeasure * 100));
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
                result = null;
            }
            return result;
        }

        private string GetMinutiaName(MindtctMinutiaTypes minutiaType, MindtctFeatureTypes featureType)
        {
            string result = string.Empty;
            try
            {
                switch (minutiaType)
                {
                    case MindtctMinutiaTypes.Bifurcation:
                        switch (featureType)
                        {
                            case MindtctFeatureTypes.Appearing:
                                result = VectorBifurcationAppearing.Name;
                                break;
                            case MindtctFeatureTypes.Disappearing:
                                result = VectorBifurcationDisappearing.Name;
                                break;
                        }
                        break;
                    case MindtctMinutiaTypes.RidgeEnding:
                        switch (featureType)
                        {
                            case MindtctFeatureTypes.Appearing:
                                result = vectorRidgeEndingAppearing.Name;
                                break;
                            case MindtctFeatureTypes.Disappearing:
                                result = vectorRidgeEndingDisappearing.Name;
                                break;
                        }
                        break;
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
