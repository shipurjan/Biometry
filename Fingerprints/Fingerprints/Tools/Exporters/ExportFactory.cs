using ExceptionLogger;
using Fingerprints.MinutiaeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Tools.Exporters
{
    public static class ExportFactory
    {
        public static DataExporter Create(ExportTypes _type, List<MinutiaStateBase> _data)
        {
            DataExporter result = null;
            try
            {
                switch (_type)
                {
                    case ExportTypes.Xyt:
                        result = new XytExporter(_data);
                        break;
                    case ExportTypes.Txt:
                        result = new TxtExporter(_data);
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
