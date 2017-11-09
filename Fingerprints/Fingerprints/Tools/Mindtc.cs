using ExceptionLogger;
using Fingerprints.Tools.Importers;
using Fingerprints.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Tools
{
    class Mindtc
    {
        private DrawingService DrawingService { get; }
        public Mindtc(DrawingService _drawingService)
        {
            try
            {
                DrawingService = _drawingService;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void Identify(string _sImagePath)
        {
            ImportResult importResult = null;
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
