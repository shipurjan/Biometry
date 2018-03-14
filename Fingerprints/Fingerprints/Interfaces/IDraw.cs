using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Fingerprints
{
    public interface IDraw
    {
        void Draw(OverridedCanvas canvas, Image image, int index = -1);
        void Stop(Image image, OverridedCanvas canvas);
    }
}