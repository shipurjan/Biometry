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
        void Draw(Canvas canvas, Image image, Border border);
    }
}