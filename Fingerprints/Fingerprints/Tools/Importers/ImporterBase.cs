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
        /// Data that need to be prepared
        /// </summary>
        protected DrawingService DrawingService;

        public ImporterBase(DrawingService _drawingService)
        {
            DrawingService = _drawingService;
        }
    }
}
