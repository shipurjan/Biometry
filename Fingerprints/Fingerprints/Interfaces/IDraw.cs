using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Fingerprints
{
    interface IDraw
    {
        void Draw(OverridedCanvas canvas, Image image, RadioButton radioButton1, RadioButton radioButton2, int index = -1);
        void DeleteEvent(Image image, OverridedCanvas canvas);

        void DrawFromFile(OverridedCanvas canvas);
    }
}