using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fingerprints.MinutiaeTypes;
using ExceptionLogger;
using Fingerprints.Models;
using Fingerprints.Factories;
using Fingerprints.ViewModels;
using Newtonsoft.Json;

namespace Fingerprints.Tools.Importers
{
    public class TxtImporter : ImporterBase, IDataImporter
    {
        public TxtImporter(DrawingService _drawingService) : base(_drawingService)
        {
        }

        public List<MinutiaFileState> GetformattedData()
        {
            List<MinutiaFileState> result = null;
            try
            {
                result = JsonConvert.DeserializeObject<List<MinutiaFileState>>(fileContent);
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
