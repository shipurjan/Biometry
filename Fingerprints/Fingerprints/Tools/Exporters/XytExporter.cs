using ExceptionLogger;
using Fingerprints.MinutiaeTypes;
using Fingerprints.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fingerprints.Tools.Exporters
{
    class XytExporter : Exporter, DataExporter
    {
        List<string> preparedData;
        public XytExporter(List<MinutiaStateBase> _data) : base(_data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void FormatData()
        {
            List<MinutiaStateBase> vectors = null;
            try
            {
                vectors = data.Where(x => x.Minutia.TypeId == 2).ToList();
                foreach (var item in vectors)
                {
                    preparedData.Add(Parse(item));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void Export(string _path)
        {
            try
            {
                FileTransfer.SaveFile(_path, preparedData);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private string Parse(MinutiaStateBase item)
        {
            string result = string.Empty;
            Point firstPoint;
            try
            {
                firstPoint = item.Points.FirstOrDefault();
                result = string.Format("{0} {1} {2}", firstPoint.X, firstPoint.Y, Utils.angleInDegrees(Convert.ToDouble(item.Angle)));
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }
    }
}
