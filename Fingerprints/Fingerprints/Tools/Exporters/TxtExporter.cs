using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fingerprints.MinutiaeTypes;
using ExceptionLogger;

namespace Fingerprints.Tools.Exporters
{
    class TxtExporter : Exporter, DataExporter
    {
        private List<string> preparedData;
        public TxtExporter(List<MinutiaStateBase> _data) : base(_data)
        {
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

        public void FormatData()
        {
            try
            {
                foreach (var item in data)
                {
                    Parse(item);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private string Parse(MinutiaStateBase _state)
        {
            string result = string.Empty;
            try
            {
                switch (_state.Minutia.TypeId)
                {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        result = Parse((CurveLineState)_state);
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        result = Parse((SegmentState)_state);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }

            return result;
        }

        private string Parse(SegmentState _state)
        {
            string result = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        private string Parse(CurveLineState _state)
        {
            string result = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }
    }
}
