using Fingerprints.MinutiaeTypes;
using Fingerprints.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Tools.Importers
{
    public abstract class ImporterBase
    {
        /// <summary>
        /// DrawingService in which Minutia will be located
        /// </summary>
        protected DrawingService DrawingService;

        public ImporterBase(DrawingService _drawingService)
        {
            DrawingService = _drawingService;
        }
    }
}
