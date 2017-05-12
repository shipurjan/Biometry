using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fingerprints.Models;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Fingerprints.Resources;
using System.Windows;

namespace Fingerprints.MinutiaeTypes.Vector
{
    class UserVector : Vector, IDraw
    {
        MouseButtonEventHandler handler = null;
        MouseEventHandler mouseMove = null;
        int clickCount;
        public UserVector(MinutiaState state) : base(state)
        {
            state.Points = new List<Point>();
        }

        public void Draw(OverridedCanvas canvas, Image image, int index = -1)
        {

            handler += (ss, ee) =>
            {

                Path myPath = new Path();
                EllipseGeometry myEllipseGeometry = new EllipseGeometry();
                LineGeometry myPathFigure = new LineGeometry();
                myPathFigure.StartPoint = new Point(0, 0);

                if (clickCount == 0)
                {
                    state.Id = getIdForMinutiae(canvas.Tag.ToString(), index);
                    tmp1 = ee.GetPosition(canvas);
                    state.Points.Insert(0, tmp1.ToFloorPoint());
                    myPathFigure.StartPoint = tmp1;

                    myEllipseGeometry.Center = tmp1;
                    myEllipseGeometry.RadiusX = 2 * state.Minutia.Size;
                    myEllipseGeometry.RadiusY = 2 * state.Minutia.Size;
                    group.Children.Add(myEllipseGeometry);

                    var linetmp = new LineGeometry();
                    group.Children.Add(linetmp);
                    drawCompleteLine(ee, canvas, state.Minutia.Size);

                    myPath.Stroke = color;
                    myPath.StrokeThickness = state.Minutia.Thickness;
                    myPath.Data = group;
                    myPath.Tag = state.Minutia.Name;
                    myPath.Uid = state.Id.ToString();
                    DeleteEmptyAtIndex(canvas, index);
                    AddEmptyToOpositeSite(canvas, index);
                    canvas.AddLogicalChild(myPath, index);

                    clickCount++;
                }
                else
                {
                    AddElementToSaveList(canvas.Tag.ToString(), index);
                    canvas.Children[canvas.Children.Count - 1].Opacity = 0.5;
                    clickCount = 0;
                    index = -1;
                    group = null;
                    group = new GeometryGroup();
                }


            };

            mouseMove += (ss, ee) =>
            {
                if (clickCount == 1)
                {
                    drawCompleteLine(ee, canvas, state.Minutia.Size);
                }
            };
            image.MouseMove += mouseMove;
            image.MouseRightButtonDown += handler;
            canvas.MouseRightButtonDown += handler;
        }

        public void Stop(Image image, OverridedCanvas canvas)
        {
            image.MouseRightButtonDown -= handler;
            image.MouseMove -= mouseMove;
            canvas.MouseRightButtonDown -= handler;
        }

        private void drawCompleteLine(MouseEventArgs ee, Canvas canvas, double size)
        {
            tmp2 = ee.GetPosition(canvas);
            double deltaX = tmp2.X - tmp1.X;
            double deltaY = tmp2.Y - tmp1.Y;
            state.Angle = (Math.Atan2(deltaY, deltaX));

            tmp2.X = tmp1.X + Math.Cos(state.Angle) * 10;
            tmp2.Y = tmp1.Y + Math.Sin(state.Angle) * 10;

            ((LineGeometry)group.Children[1]).StartPoint = new Point(tmp1.X + (2 * size) * Math.Cos(state.Angle), tmp1.Y + (2 * size) * Math.Sin(state.Angle));
            ((LineGeometry)group.Children[1]).EndPoint = tmp2;
        }
    }
}
