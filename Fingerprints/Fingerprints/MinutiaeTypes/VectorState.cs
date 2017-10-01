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
    class VectorState : MinutiaStateBase, IMouseClickable, IDrawable, IMouseMoveable
    {
        public VectorState(DrawingService _oDrawingService) : base(_oDrawingService)
        {
        }

        public void DrawProcedure()
        {
            try
            {
                if (Points.Count > 1)
                {
                    // Get beging and end of drawing
                    var firstPoint = Points[0];
                    var secondPoint = Points[1];
                    
                    // Get delta X and Y
                    double deltaX = secondPoint.X - firstPoint.X;
                    double deltaY = secondPoint.Y - firstPoint.Y;

                    // Compute Angle using deltas
                    Angle = (Math.Atan2(deltaY, deltaX));

                    // Draw elipse starting in firstPoint location and fixed radius size
                    WriteableBmp.DrawEllipseCentered(Convert.ToInt16(firstPoint.X), Convert.ToInt16(firstPoint.Y), 5, 5, Colors.Red);

                    // Draw line starting circle border and ending in pointer location
                    WriteableBmp.DrawLine(Convert.ToInt16(firstPoint.X + 5 * Math.Cos(Angle)),
                        Convert.ToInt16(firstPoint.Y + 5 * Math.Sin(Angle)),
                        Convert.ToInt16(secondPoint.X),
                        Convert.ToInt16(secondPoint.Y),
                        Colors.Red);
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
                if (Points.Count == 0)
                {                   
                    Points.Add(args.GetPosition((IInputElement)sender).ToFloorPoint());
                }
                else
                {
                    DrawingService.InitiateNewDrawing();
                }
            }
        }

        public void MouseMove(object sender, MouseEventArgs args)
        {
            try
            {
                if (Points.Count == 1)
                {
                    Points.Add(args.GetPosition((IInputElement)sender).ToFloorPoint());
                }
                else if (Points.Count != 0)
                {
                    Points[1] = (args.GetPosition((IInputElement)sender).ToFloorPoint());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
