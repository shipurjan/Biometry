using ExceptionLogger;
using Fingerprints.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Tools.Importers
{
    class ImporterFactory
    {
        public static IDataImporter Create(ImportTypes _type, DrawingService _drawingService)
        {
            IDataImporter result = null;
            try
            {
                switch (_type)
                {
                    case ImportTypes.txt:
                        result = new TxtImporter(_drawingService);
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
