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
        public static MinutiaStateBase Create(MinutiaState _oState, WriteableBitmap _oWriteableBmp, DrawingService _oDrawingService)
        {
            MinutiaStateBase oMinutiaState = null;

            try
            {
                switch (_oState.Minutia.TypeId)
                {
                    case 3:
                        oMinutiaState = new CurveLineState(_oWriteableBmp, _oDrawingService);
                        oMinutiaState.Minutia = _oState.Minutia;
                        break;
                    case 6:
                        oMinutiaState = new SegmentState(_oWriteableBmp, _oDrawingService);
                        oMinutiaState.Minutia = _oState.Minutia;
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }

            return oMinutiaState;
        }
    }
}
