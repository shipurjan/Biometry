using ExceptionLogger;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows;
using Fingerprints.Resources;
using Fingerprints.Interfaces;
using Fingerprints.ViewModels;

namespace Fingerprints.MinutiaeTypes
{
    class CurveLineState : MinutiaStateBase, IMouseMoveable, IMouseClickable, IDrawable
    {
        public CurveLineState(WriteableBitmap _oWriteableBmp, MainWindowViewModel _oMainWindowViewModel) : base(_oWriteableBmp, _oMainWindowViewModel)
        {
        }

        public void DrawProcedure()
        {
            try
            {
                if (Points.Count > 0)
                {
                    oWriteableBmp.DrawPolyline(IntPoints, Colors.Red);
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
