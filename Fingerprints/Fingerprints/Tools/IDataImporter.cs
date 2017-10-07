using Fingerprints.MinutiaeTypes;
using Fingerprints.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Tools
{
    interface IDataImporter
    {
        /// <summary>
        /// Method should prepare data to proper format
        /// </summary>
        List<MinutiaFileState> GetformattedData();

        /// <summary>
        /// Imports data
        /// </summary>
        /// <param name="_path">Path from data will be imported</param>
        void Import(string _path);
    }
}
