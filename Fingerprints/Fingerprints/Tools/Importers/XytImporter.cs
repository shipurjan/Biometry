using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fingerprints.Models;
using Fingerprints.ViewModels;
using ExceptionLogger;

namespace Fingerprints.Tools.Importers
{
    class XytImporter : ImporterBase, IDataImporter
    {
        public XytImporter() : base()
        {
        }

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
                fileContent = FileTransfer.LoadFile(_path);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
