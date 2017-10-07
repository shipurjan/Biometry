using ExceptionLogger;
using Fingerprints.MinutiaeTypes;
using Fingerprints.Models;
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
        public List<MinutiaFileState> ResultData { get; }
        public string Error { get; }

        public ImportResult(bool _result, List<MinutiaFileState> _resultData, string _error)
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
