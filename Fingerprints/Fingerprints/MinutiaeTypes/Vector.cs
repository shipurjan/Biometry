using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints
{
    class Vector : Minutiae
    {
        Brush color;
        Point firstPointLine;
        Point tmp1, tmp2;
        double angle;
        int clickCount;
        MouseButtonEventHandler handler = null;
        MouseEventHandler mouseMove = null;
        GeometryGroup group = new GeometryGroup();
        public Vector(Brush color)
        {
            this.color = color;
            firstPointLine = new Point();
            tmp1 = new Point();
            tmp2 = new Point();
        }

        public override void Draw(Canvas canvas, Image image)
        {
            handler += (ss, ee) =>
            {

                Path myPath = new Path();
                EllipseGeometry myEllipseGeometry = new EllipseGeometry();
                LineGeometry myPathFigure = new LineGeometry();
                myPathFigure.StartPoint = new Point(0, 0);
                if (clickCount == 0)
                {
                    tmp1 = ee.GetPosition(canvas);
                    firstPointLine = tmp1;
                    myPathFigure.StartPoint = tmp1;

                    myEllipseGeometry.Center = tmp1;
                    myEllipseGeometry.RadiusX = 2;
                    myEllipseGeometry.RadiusY = 2;
                    group.Children.Add(myEllipseGeometry);

                    var linetmp = new LineGeometry();
                    group.Children.Add(linetmp);
                    drawCompleteLine(ee, canvas);

                    myPath.Stroke = color;
                    myPath.StrokeThickness = 0.3;
                    myPath.Data = group;
                    canvas.Children.Add(myPath);

                    clickCount++;
                }
                else
                {
                    canvas.Children[canvas.Children.Count - 1].Opacity = 0.5;
                    clickCount = 0;
                    group = null;
                    group = new GeometryGroup();
                    DeleteEvent(image);
                }
            };

            mouseMove += (ss, ee) =>
            {
                if (clickCount == 1)
                {
                    drawCompleteLine(ee, canvas);
                }
            };
            image.MouseMove += mouseMove;
            image.MouseRightButtonDown += handler;
        }

        private void drawCompleteLine(MouseEventArgs ee, Canvas canvas)
        {
            tmp2 = ee.GetPosition(canvas);
            double deltaX = tmp2.X - tmp1.X;
            double deltaY = tmp2.Y - tmp1.Y;
            angle = (Math.Atan2(deltaY, deltaX));

            tmp2.X = tmp1.X + Math.Cos(angle) * 10;
            tmp2.Y = tmp1.Y + Math.Sin(angle) * 10;

            ((LineGeometry)group.Children[1]).StartPoint = new Point(tmp1.X + 2 * Math.Cos(angle), tmp1.Y + 2 * Math.Sin(angle));
            ((LineGeometry)group.Children[1]).EndPoint = tmp2;
        }

        public void DeleteEvent(Image image)
        {
            image.MouseRightButtonDown -= handler;
            image.MouseMove -= mouseMove;
        }
    }
}
