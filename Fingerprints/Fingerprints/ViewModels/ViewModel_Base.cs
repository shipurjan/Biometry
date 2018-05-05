using ExceptionLogger;
using Fingerprints.MinutiaeTypes;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.ViewModels
{
    class ViewModel_Base : BindableBase
    {
        /// <summary>
        /// Adds DrawingObject with type 'Empty' to DrawingData
        /// </summary>
        /// <param name="_drawingService"></param>
        /// <param name="_oppositeDrawingService"></param>
        protected void AddEmptyObjectOnLastPosition(DrawingService _drawingService, DrawingService _oppositeDrawingService)
        {
            try
            {
                if (CanAddEmptyObjectOnLastPosition(_drawingService, _oppositeDrawingService))
                {
                    _drawingService.DrawingData.Add(new EmptyState());
                }

                if (CanAddEmptyObjectOnLastPosition(_oppositeDrawingService, _drawingService))
                {
                    _oppositeDrawingService.DrawingData.Add(new EmptyState());
                }

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Fils DrawingData with empty objects by count
        /// </summary>
        /// <param name="_drawingService"></param>
        /// <param name="_count"></param>
        protected void FillDrawingDataWithEmptyObjects(DrawingService _drawingService, int _count)
        {
            try
            {
                if (_count <= 0 || _drawingService.BackgroundImage == null)
                {
                    return;
                }

                for (
                    int i = 0; i < _count; i++)
                {
                    _drawingService.AddMinutiaToDrawingData(new EmptyState());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }


        /// <summary>
        /// Checks if EmptyObject can be added to last index
        /// </summary>
        /// <param name="_drawingService"></param>
        /// <param name="_oppositeDrawingService"></param>
        /// <returns></returns>
        private bool CanAddEmptyObjectOnLastPosition(DrawingService _drawingService, DrawingService _oppositeDrawingService)
        {
            bool result = true;
            try
            {
                // if DrawingData is empty, return false
                if (_drawingService.WriteableBitmap == null)
                    return false;

                // if EmptyObject is on last position and DrawingData has more objects that opposite, returns false
                if (_drawingService.DrawingData.LastOrDefault() is EmptyState && _oppositeDrawingService.DrawingData.Count < _drawingService.DrawingData.Count)
                    return false;

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }

            return result;
        }
    }
}
