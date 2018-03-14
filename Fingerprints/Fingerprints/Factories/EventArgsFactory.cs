using ExceptionLogger;
using Fingerprints.EventArgsObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Factories
{
    public static class EventArgsFactory
    {
        public static DrawingObjectAddedEventArgs CreateDrawingObjectAdded(int _position)
        {
            DrawingObjectAddedEventArgs result = null;
            try
            {
                result = new DrawingObjectAddedEventArgs();
                result.PositionIndex = _position;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }
    }
}
