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

namespace Fingerprints.Tools.Importers
{
    public class TxtImporter : ImporterBase, IDataImporter
    {
        private List<MinutiaFileState> dataToPrepare;

        public TxtImporter(DrawingService _drawingService) : base(_drawingService)
        {
        }

        public List<MinutiaStateBase> GetformattedData()
        {
            List<MinutiaStateBase> result = null;
            List<SelfDefinedMinutiae> definedMinutiaes = null;
            try
            {
                using (var db = new FingerContext())
                {
                    definedMinutiaes = db.SelfDefinedMinutiaes.ToList();
                }

                foreach (var item in dataToPrepare)
                {
                    var tempMinutia = definedMinutiaes.Where(x => x.Name == item.Name).FirstOrDefault();

                    result.Add(MinutiaStateFactory.Create(item, tempMinutia, DrawingService));
                }
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
                dataToPrepare = FileTransfer.LoadFile(_path);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
