using ExceptionLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Fingerprints.Tools.ImageFilters
{
    class FilterImage
    {
        private readonly FilterImageFluentInterface _set;

        public BitmapImage OryginalImage { get; set; }

        public BitmapImage FilteredImage { get; set; }

        public FilterImage(BitmapImage _image)
        {
            try
            {
                OryginalImage = _image;
                FilteredImage = _image;
                _set = new FilterImageFluentInterface(this);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public FilterImageFluentInterface Filter(FilterImageType type)
        {
            _set.Filter(type);
            return _set;
        }

        internal byte[,,] ToBitmap()
        {
            throw new NotImplementedException();
        }
    }
}
