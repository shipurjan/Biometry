using ExceptionLogger;
using Fingerprints.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Factories
{
    class TypeModelFactory
    {
        public static Type Create(DrawingType _drawingType)
        {
            Type result = null;
            try
            {
                result = new Type()
                {
                    Name = _drawingType.ToString(),
                    DrawingType = _drawingType
                };
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }
    }
}
