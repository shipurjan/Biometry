using Emgu.CV;
using Emgu.CV.Structure;
using ExceptionLogger;
using Fingerprints.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Fingerprints.Tools.ImageFilters
{
    public class FilterImageFluentInterface
    {
        private readonly FilterImage _filterImage;

        public FilterImageFluentInterface(FilterImage filterImage)
        {
            _filterImage = filterImage;
        }

        /// <summary>
        /// Set filter image using given filter
        /// </summary>
        /// <param name="_filterType"></param>
        /// <returns></returns>
        public FilterImageFluentInterface Filter(FilterImageType _filterType, params object[] _parameters)
        {
            try
            {
                switch (_filterType)
                {
                    case FilterImageType.None:
                        SetFilteredToOryginal();
                        break;
                    case FilterImageType.Sobel:
                        FilterBySobel();
                        break;
                    case FilterImageType.Gauss:
                        FilterByGauss(_parameters);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }            
            return this;
        }

        /// <summary>
        /// Filter using sobel
        /// </summary>
        private void FilterBySobel()
        {
            Image<Gray, Byte> sobel = null;
            try
            {
                sobel = new Image<Gray, Byte>(_filterImage.FilteredImage);
                _filterImage.FilteredImage = sobel.Sobel(0, 1, 3)
                                            .Add(sobel.Sobel(1, 0, 3))
                                            .AbsDiff(new Gray(0.0)).ToBitmap();                
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Filter using gauss
        /// </summary>
        private void FilterByGauss(params object[] _parameters)
        {
            Image<Gray, Byte> gauss = null;
            try
            {
                gauss = new Image<Gray, Byte>(_filterImage.FilteredImage);
                _filterImage.FilteredImage = gauss.SmoothGaussian(Convert.ToInt32(_parameters[0]))
                                            .ToBitmap();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Set back to oryginal image
        /// </summary>
        private void SetFilteredToOryginal()
        {
            try
            {
                _filterImage.FilteredImage = _filterImage.OryginalImage;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Return filtered image
        /// </summary>
        /// <returns></returns>
        public Bitmap Get()
        {
            return _filterImage.FilteredImage;
        }
    }
}
