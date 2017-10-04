using ExceptionLogger;
using Fingerprints.MinutiaeTypes;
using Fingerprints.Models;
using Fingerprints.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                    case 2:
                        oMinutiaState = new VectorState(_oDrawingService);
                        oMinutiaState.Minutia = _oMinutia;
                        break;
                    case 3:
                        oMinutiaState = new CurveLineState(_oDrawingService);
                        oMinutiaState.Minutia = _oMinutia;
                        break;
                    case 6:
                        oMinutiaState = new SegmentState(_oDrawingService);
                        oMinutiaState.Minutia = _oMinutia;
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }

            return oMinutiaState;
        }

        /// <summary>
        /// Creates MinutiaStateBase
        /// </summary>
        /// <param name="_minutiaFileState"></param>
        /// <param name="_minutia"></param>
        /// <param name="_drawingService"></param>
        /// <returns></returns>
        public static MinutiaStateBase Create(MinutiaFileState _minutiaFileState, SelfDefinedMinutiae _minutia, DrawingService _drawingService)
        {
            MinutiaStateBase result = null;
            try
            {
                result = Create(_minutia, _drawingService);
                result.Id = _minutiaFileState.Id;
                result.Points.AddRange(_minutiaFileState.Points);
                result.Angle = _minutiaFileState.Angle;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }


    }
}
