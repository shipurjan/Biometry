using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Tools
{
    public interface DataExporter
    {
        void FormatData();
        void Export(string _path);
    }
}
