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
        /// <summary>
        /// Creates MinutiaStateBase object assigned to drawing service
        /// </summary>
        /// <param name="_oMinutia">SelfDefinedMinutiae</param>
        /// <param name="_oDrawingService">DrawingService</param>
        /// <param name="_atIndex">Optional index where Minutia must be added</param>
        /// <returns></returns>
        public static MinutiaStateBase Create(SelfDefinedMinutiae _oMinutia, DrawingService _oDrawingService, int? _atIndex = null)
        {
            MinutiaStateBase oMinutiaState = null;

            try
            {
                switch (_oMinutia.TypeId)
                {
                    case 1:
                        oMinutiaState = new PointState(_oDrawingService, _atIndex);
                        break;
                    case 2:
                        oMinutiaState = new VectorState(_oDrawingService, _atIndex);
                        break;
                    case 3:
                        oMinutiaState = new CurveLineState(_oDrawingService, _atIndex);
                        break;
                    case 4:
                        oMinutiaState = new TriangleState(_oDrawingService);
                        break;
                    case 5:
                        oMinutiaState = new PeakState(_oDrawingService, _atIndex);
                        break;
                    case 6:
                        oMinutiaState = new SegmentState(_oDrawingService, _atIndex);
                        break;
                    case 7:
                        oMinutiaState = new EmptyState(_oDrawingService, _atIndex);
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

        /// <summary>
        /// Creates MinutieStatebase object based to file state which is assign to drawing service
        /// </summary>
        /// <param name="_fileStates"></param>
        /// <param name="_drawingService"></param>
        public static void AddMinutiaeFileToDrawingService(List<MinutiaFileState> _fileStates, DrawingService _drawingService)
        {
            List<SelfDefinedMinutiae> definedMinutiaes = null;

            try
            {
                using (var db = new FingerContext())
                {
                    //get SelfDefinedMinutiaes from db
                    definedMinutiaes = db.SelfDefinedMinutiaes.ToList();

                    //creates MinutiaeStateBase and adds to list
                    foreach (var item in _fileStates)
                    {
                        //get SelfDefinedMinutia by name
                        var tempMinutia = definedMinutiaes.Where(x => x.Name == item.Name).FirstOrDefault();

                        //Creates object of Minutia which is automatic assigned to drawing service
                        MinutiaStateFactory.Create(item, tempMinutia, _drawingService);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

    }
}
