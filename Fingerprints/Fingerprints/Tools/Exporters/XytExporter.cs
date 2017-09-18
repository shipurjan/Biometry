using ExceptionLogger;
using Fingerprints.MinutiaeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Tools.Exporters
{
    class XytExporter : Exporter, DataExporter
    {
        public XytExporter(List<MinutiaStateBase> _data) :base(_data)
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
            try
            {

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

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
