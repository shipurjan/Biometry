using ExceptionLogger;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows;
using Fingerprints.Resources;
using Fingerprints.Interfaces;
using Fingerprints.ViewModels;
using Fingerprints.Tools.LinestoPointsConverter;
using System.Linq;

namespace Fingerprints.MinutiaeTypes
{
    class CurveLineState : MinutiaStateBase, IMouseMoveable, IMouseClickable, IDrawable
    {
        public CurveLineState(DrawingService _oDrawingService, SelfDefinedMinutiae _minutia, int? _atIndex = null) : base(_oDrawingService, _minutia, _atIndex)
        {
        }

        public void DrawProcedure()
        {
            try
            {
                if (Points.Count > 0)
                {
                    WriteableBmp.DrawPolyline(IntPoints, Color);
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
                AddPointToList(sender, args);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void AddPointToList(object sender, MouseEventArgs args)
        {
            try
            {
                if (args.MouseDevice.RightButton == MouseButtonState.Pressed)
                {
                    var point = args.GetPosition((IInputElement)sender).ToFloorPoint();
                    Points.Add(point);
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
                AddPointToList(sender, args);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
