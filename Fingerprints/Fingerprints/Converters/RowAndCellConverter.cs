using ExceptionLogger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Fingerprints.Converters
{
    class RowAndCellConverter : IMultiValueConverter
    {
        public object Convert(object[] values, System.Type targetType, object parameter, CultureInfo culture)
        {
            GridClickedItemPosition result = null;
            try
            {
                result = new GridClickedItemPosition()
                {
                    RowIndex = (int)values[0],
                    CellIndex = (int)values[1]
                };
            }

            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
