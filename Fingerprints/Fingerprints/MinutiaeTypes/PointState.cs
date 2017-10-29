using Fingerprints.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Fingerprints.ViewModels;
using ExceptionLogger;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using Fingerprints.Resources;

namespace Fingerprints.MinutiaeTypes
{
    class PointState : MinutiaStateBase, IMouseClickable, IDrawable
    {
        public PointState(DrawingService _oDrawingService, SelfDefinedMinutiae _minutia, int? _atIndex = null) : base(_oDrawingService, _minutia, _atIndex)
        {
        }

        public void DrawProcedure()
        {
            try
            {
                if (Points.Count == 1)
                {
                    // Get beging and end of drawing
                    var firstPoint = Points[0];

                    // Draw elipse starting in firstPoint location and fixed radius size
                    WriteableBmp.DrawEllipseCentered(Convert.ToInt16(firstPoint.X), Convert.ToInt16(firstPoint.Y), 5, 5, Colors.Green);                    
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void MouseClick(object sender, MouseButtonEventArgs args)
        {
            if (args.RightButton == MouseButtonState.Pressed)
            {
                try
                {
                    if (Points.Count == 0)
                    {
                        Points.Add(args.GetPosition((IInputElement)sender).ToFloorPoint());
                        DrawingService.InitiateNewDrawing();
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteExceptionLog(ex);
                }
                
            }
        }
    }
}
