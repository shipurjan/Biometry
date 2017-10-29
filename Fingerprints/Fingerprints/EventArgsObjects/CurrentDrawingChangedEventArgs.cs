using Fingerprints.MinutiaeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.EventArgsObjects
{
    class CurrentDrawingChangedEventArgs : EventArgs
    {
        public MinutiaStateBase CurrentDrawing { get; set; }
    }
}
