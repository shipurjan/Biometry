using ExceptionLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Fingerprints.Tools
{
    public static class HexColor
    {
        /// <summary>
        /// Creates brush color from hex string
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static Brush ToBrush(string _value)
        {
            Brush result = null;
            try
            {
                result = (Brush)new BrushConverter().ConvertFromString(_value);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }
    }
}
