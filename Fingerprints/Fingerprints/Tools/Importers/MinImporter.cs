using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fingerprints.Models;
using ExceptionLogger;

namespace Fingerprints.Tools.Importers
{
    class MinImporter : ImporterBase, IDataImporter
    {
        public List<MinutiaFileState> GetformattedData()
        {
            List<MinutiaFileState> result = null;
            try
            {

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

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
