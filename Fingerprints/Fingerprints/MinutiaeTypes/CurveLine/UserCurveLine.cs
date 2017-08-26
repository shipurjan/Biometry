using Fingerprints.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints.MinutiaeTypes.CurveLine
{
    class UserCurveLine : CurveLine, IDraw
    {
        MouseEventHandler handler = null;
        bool newLine;
        Point currentPoint;

        public UserCurveLine(MinutiaState state) : base(state)
        {
            state.Points = new List<Point>();
            newLine = true;
        }
        public void Draw(OverridedCanvas canvas, Image image, int index = -1)
        {
            mainWindow.acceptLeftCurveButton.Visibility = Visibility.Visible;
            mainWindow.acceptRightCurveButton.Visibility = Visibility.Visible;

            acceptButtonsClickEvents(canvas, index);

            handler += (ss, ee) =>
            {
                if (ee.RightButton == MouseButtonState.Pressed)
                {
                    if (newLine)
                    {
                        baseLine = new Polyline
                        {
                            Stroke = color,
                            StrokeThickness = state.Minutia.Thickness,
                            StrokeLineJoin = PenLineJoin.Miter,
                            StrokeMiterLimit = 0,
                            Tag = state.Minutia.Name,
                        };
                        state.Id = getIdForMinutiae(canvas.Tag.ToString(), index);
                        baseLine.Uid = state.Id.ToString();
                        DeleteEmptyAtIndex(canvas, index);
                        AddEmptyToOpositeSite(canvas, index);
                        canvas.AddLogicalChild(baseLine, index);
                        newLine = false;
                        index = -1;
                    }
                    currentPoint.X = Math.Floor(ee.GetPosition(canvas).X);
                    currentPoint.Y = Math.Floor(ee.GetPosition(canvas).Y);

                    if (baseLine.Points.LastOrDefault() != currentPoint)
                    {
                        baseLine.Points.Add(currentPoint);
                    }
                }
            };
            image.MouseMove += handler;
            canvas.MouseMove += handler;
        }

        public void Stop(Image image, OverridedCanvas canvas)
        {
            mainWindow.acceptLeftCurveButton.Visibility = Visibility.Hidden;
            mainWindow.acceptRightCurveButton.Visibility = Visibility.Hidden;
            image.MouseMove -= handler;
            canvas.MouseMove -= handler;
        }

        public void acceptButtonsClickEvents(OverridedCanvas canvas, int index = -1)
        {
            if (canvas.Tag.ToString() == "Left")
            {
                mainWindow.acceptLeftCurveButton.Click += (ss, ee) =>
                {
                    newLine = true;
                    this.AddElementToSaveList(canvas.Tag.ToString(), index);
                    this.baseLine = null;
                };
            }
            else
            {
                mainWindow.acceptRightCurveButton.Click += (ss, ee) =>
                {
                    newLine = true;
                    this.AddElementToSaveList(canvas.Tag.ToString(), index);
                    this.baseLine = null;
                };
            }
        }
    }
}
