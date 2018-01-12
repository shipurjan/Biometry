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
        /// <summary>
        /// Returns Import object based on type
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_drawingService"></param>
        /// <returns></returns>
        public static IDataImporter Create(ImportTypes _type)
        {
            IDataImporter result = null;
            try
            {
                switch (_type)
                {
                    case ImportTypes.txt:
                        result = new TxtImporter();
                        break;
                    case ImportTypes.xyt:
                        result = new XytImporter();
                        break;
                    case ImportTypes.min:
                        result = new MinImporter();
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
