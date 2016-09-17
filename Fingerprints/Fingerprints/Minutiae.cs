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
        public virtual void Draw(OverridedCanvas canvas, Image image, RadioButton radioButton1, RadioButton radioButton2) { }
        public virtual void DeleteEvent(Image image, OverridedCanvas canvas) { }

        public virtual void DrawFromFile() { }
    }
}
