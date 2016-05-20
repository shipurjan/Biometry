using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Fingerprints
{
    abstract class Minutiae
    {
        public string Name;
        public virtual void Draw(Canvas canvas, Image image) { }
    }
}
