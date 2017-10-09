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

        /// <summary>
        /// File content in string
        /// </summary>
        protected string fileContent;

        public ImporterBase(DrawingService _drawingService)
        {
            DrawingService = _drawingService;
        }
    }
}
