using ExceptionLogger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Fingerprints.Converters
{
    class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            Brush result = null;
            try
            {
                if ((bool)value == true)
                {
                    Color temp = (Color)ColorConverter.ConvertFromString("#a8a8a8");
                    result = new SolidColorBrush(temp);
                }
                else
                {
                    Color temp = (Color)ColorConverter.ConvertFromString(Application.Current.Resources["MainColor"].ToString());
                    result = new SolidColorBrush(temp);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
