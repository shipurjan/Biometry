using ExceptionLogger;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Fingerprints.Tools.ImageFilters
{
    public class FilterImage
    {
        private readonly FilterImageFluentInterface _set;

        public Bitmap OryginalImage { get; set; }

        public Bitmap FilteredImage { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public FilterImage(Bitmap _image, string _path)
        {
            try
            {
                FilePath = _path;
                FileName = Path.GetFileName(FilePath);
                OryginalImage = _image;
                FilteredImage = _image;
                _set = new FilterImageFluentInterface(this);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public FilterImageFluentInterface Filter(FilterImageType type, params object[] _parameters)
        {
            _set.Filter(type, _parameters);
            return _set;
        }
    }
}
