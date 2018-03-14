using ExceptionLogger;
using Fingerprints.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Fingerprints.Tools
{
    public class DrawingDecorator
    {
        private DrawingService DrawingService { get; }
        public DrawingDecorator(DrawingService _drawingService)
        {
            try
            {
                DrawingService = _drawingService;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Sets opacity to all drawingObjects except at specific index
        /// </summary>
        /// <param name="_index">default null, method will be set alpha to default value (255)</param>
        public void ShowOnlyIndex(int? _index = null)
        {
            Color temp;
            byte alphaValue = 255;
            try
            {
                alphaValue = _index.HasValue ? (byte)150 : (byte)255;

                foreach (var item in DrawingService.DrawingData)
                {
                    temp = item.Color;
                    temp.A = alphaValue;
                    item.Color = temp;

                    item.IsSelected = false;
                }

                if (_index.HasValue)
                {
                    temp = DrawingService.DrawingData[_index.Value].Color;
                    temp.A = 255;
                    DrawingService.DrawingData[_index.Value].Color = temp;

                    DrawingService.DrawingData[_index.Value].IsSelected = true;
                }

                DrawingService.Draw();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
