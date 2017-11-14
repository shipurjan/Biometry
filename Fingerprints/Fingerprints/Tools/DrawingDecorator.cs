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
    class DrawingDecorator
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

        public void ShowOnlyIndex(int _index)
        {
            Color temp;
            try
            {
                foreach (var item in DrawingService.DrawingData)
                {
                    temp = item.Color;
                    temp.A = 200;
                    item.Color = temp;
                }

                temp = DrawingService.DrawingData[_index].Color;
                temp.A = 255;
                DrawingService.DrawingData[_index].Color = temp;

                DrawingService.Draw();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
