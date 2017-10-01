using Fingerprints.MinutiaeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Tools.Exporters
{
    public abstract class Exporter
    {
        /// <summary>
        /// Data that need to be prepared
        /// </summary>
        protected List<MinutiaStateBase> data;
        public Exporter(List<MinutiaStateBase> _data)
        {
            data = _data;
        }
    }
}
