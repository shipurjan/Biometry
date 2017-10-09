using ExceptionLogger;
using Fingerprints.MinutiaeTypes;
using Fingerprints.Models;
using Fingerprints.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Fingerprints.Factories
{
    public static class MinutiaStateFactory
    {
        public static MinutiaStateBase Create(SelfDefinedMinutiae _oMinutia, DrawingService _oDrawingService)
        {
            MinutiaStateBase oMinutiaState = null;

            try
            {
                switch (_oMinutia.TypeId)
                {
                    case 1:
                        oMinutiaState = new PointState(_oDrawingService);
                        break;
                    case 2:
                        oMinutiaState = new VectorState(_oDrawingService);
                        break;
                    case 3:
                        oMinutiaState = new CurveLineState(_oDrawingService);
                        break;
                    case 4:
                        oMinutiaState = new TriangleState(_oDrawingService);
                        break;
                    case 5:
                        oMinutiaState = new PeakState(_oDrawingService);
                        break;
                    case 6:
                        oMinutiaState = new SegmentState(_oDrawingService);
                        break;                    
                }
                oMinutiaState.Minutia = _oMinutia;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }

            return oMinutiaState;
        }
    }
}
