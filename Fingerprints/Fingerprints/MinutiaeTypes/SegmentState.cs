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
    class SegmentState : MinutiaStateBase, IMouseClickable, IDrawable, IMouseMoveable
    {
        public SegmentState(SelfDefinedMinutiae _minutia, WriteableBitmap _writeableBitmap, int? _atIndex = null) : base(_minutia, _writeableBitmap,_atIndex)
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
                    WriteableBmp.DrawLine(Convert.ToInt16(firstPoint.X), Convert.ToInt16(firstPoint.Y), 
                        Convert.ToInt16(secondPoint.X), 
                        Convert.ToInt16(secondPoint.Y),
                        Color);
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
                    else
                    {
                        InitNewDrawing();
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
