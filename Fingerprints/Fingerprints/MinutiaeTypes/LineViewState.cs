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
    class LineViewState : MinutiaStateBase, IMouseClickable, IDrawable, IMouseMoveable
    {
        public LineViewState(WriteableBitmap _oWriteableBmp, DrawingService _oDrawingService) : base(_oWriteableBmp, _oDrawingService)
        {
        }

        public void DrawProcedure()
        {
            try
            {
                if (Points.Count > 1)
                {
                    var firstPoint = Points[0];
                    var secondPoint = Points[1];
                    WriteableBmp.DrawLine(Convert.ToInt16(firstPoint.X), Convert.ToInt16(firstPoint.Y), Convert.ToInt16(secondPoint.X), Convert.ToInt16(secondPoint.Y), Colors.Blue);
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
                    DrawingService.AddToList();
                }
                else
                {
                    DrawingService.NewDrawing();
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
                    Points.RemoveAt(1);
                    Points.Add(args.GetPosition((IInputElement)sender).ToFloorPoint());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
