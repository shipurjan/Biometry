using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Fingerprints
{
    abstract class Minutiae : IDraw
    {
        public string Name;
        public virtual void Draw(OverridedCanvas canvas, Image image, Border border1, Border border2) { }
        public virtual void DeleteEvent(Image image, OverridedCanvas canvas) { }

    }
}
