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
    /// <summary>
    /// Object represents status of the importer
    /// </summary>
    public class ImportResult
    {
        /// <summary>
        /// true if importer successfully get data
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Represent data prepared by importer
        /// </summary>
        public List<MinutiaFileState> ResultData { get; set; }

        /// <summary>
        /// if Success return fail, this property will contain error message
        /// </summary>
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
