using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Fingerprints.ViewModels
{
    class LineViewModel : MinutiaeStateViewModel
    {
        public LineViewModel(WriteableBitmap _oWriteableBmp) : base(_oWriteableBmp)
        {
        }
    }
}
