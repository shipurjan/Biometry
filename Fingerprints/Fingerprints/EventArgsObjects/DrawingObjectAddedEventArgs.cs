using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.EventArgsObjects
{
    public class DrawingObjectAddedEventArgs : EventArgs
    {
        public int PositionIndex { get; set; }
    }
}
