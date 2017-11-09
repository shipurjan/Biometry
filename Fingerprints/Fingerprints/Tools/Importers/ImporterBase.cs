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
        /// File content in string
        /// </summary>
        protected string fileContent;

        public ImporterBase()
        {
        }
    }
}
