using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Tools
{
    public interface IDataExporter
    {
        /// <summary>
        /// Method should prepare data to proper format to export
        /// </summary>
        void FormatData();

        /// <summary>
        /// Exports data
        /// </summary>
        /// <param name="_path">Path where data will be saved</param>
        void Export(string _path);
    }
}
