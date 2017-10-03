using ExceptionLogger;
using Fingerprints.MinutiaeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Tools.Importers
{
    public class ImportResult
    {
        public bool Success { get; }
        public List<MinutiaStateBase> ResultData { get; }
        public string Error { get; }

        public ImportResult(bool _result, List<MinutiaStateBase> _resultData, string _error)
        {
            try
            {
                Success = _result;
                ResultData = _resultData;
                Error = _error;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
