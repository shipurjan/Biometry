using Fingerprints.Interfaces;
using System;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using Fingerprints.Resources;
using ExceptionLogger;
using Fingerprints.ViewModels;

namespace Fingerprints.MinutiaeTypes
{
    class TriangleState : MinutiaStateBase, IMouseClickable, IDrawable, IMouseMoveable
    {
        public TriangleState(DrawingService _oDrawingService) : base(_oDrawingService)
        {
        }

        public void DrawProcedure()
        {
            try
            {
                if (Points.Count < 4)
                {
                    // Draw polyline based on current point array
                    WriteableBmp.DrawPolyline(IntPoints, Colors.OrangeRed);
                }
                else
                {
                     
                    WriteableBmp.DrawPolyline(IntPoints, Colors.OrangeRed);
                    //Points.RemoveAt(3);
                }
                
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void MouseClick(object sender, MouseButtonEventArgs args)
        {
            try
            {
                if (args.RightButton == MouseButtonState.Pressed)
                {
                    if (Points.Count == 0)
                    {
                        Points.Add(args.GetPosition((IInputElement)sender).ToFloorPoint());
                    }
                    else if (Points.Count == 2)
                    {
                        Points.Add(args.GetPosition((IInputElement)sender).ToFloorPoint());
                    }
                    else if (Points.Count == 3)
                    {
                        Points.Add(args.GetPosition((IInputElement)sender).ToFloorPoint());
                        DrawingService.InitiateNewDrawing();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
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
                else if (Points.Count == 2)
                {
                    Points[1] = (args.GetPosition((IInputElement)sender).ToFloorPoint());
                }
                else if (Points.Count == 3)
                {
                    Points[2] = (args.GetPosition((IInputElement)sender).ToFloorPoint());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
