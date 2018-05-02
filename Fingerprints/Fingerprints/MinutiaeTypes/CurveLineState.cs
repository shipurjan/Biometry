using ExceptionLogger;
using System;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows;
using Fingerprints.Resources;
using Fingerprints.Interfaces;
using System.Collections.ObjectModel;
using Fingerprints.Tools.Converters;
using System.Linq;
using Prism.Commands;

namespace Fingerprints.MinutiaeTypes
{
    class CurveLineState : MinutiaStateBase, IMouseMoveable, IMouseClickable, IDrawable
    {
        public CurveLineState(SelfDefinedMinutiae _minutia, WriteableBitmap _writeableBitmap, int? _atIndex = null) : base(_minutia, _writeableBitmap, _atIndex)
        {
            AcceptButtonVisibility = true;
            AcceptButtonCommand = new DelegateCommand(AcceptButtonClick);
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

                if (Points.Count == 1)
                {
                    Points.Add(args.GetPosition((IInputElement)sender).ToFloorPoint());
                }
                else if (Points.Count > 1)
                {
                    Points[Points.Count - 1] = args.GetPosition((IInputElement)sender).ToFloorPoint();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Action on AcceptButtonClick
        /// </summary>
        private void AcceptButtonClick()
        {
            try
            {
                Points.RemoveAt(Points.Count - 1);
                InitNewDrawing();
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
                if (args.MouseDevice.LeftButton == MouseButtonState.Pressed)
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
