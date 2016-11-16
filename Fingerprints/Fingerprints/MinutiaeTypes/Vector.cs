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
        double size;
        Point firstPointLine;
        Point tmp1, tmp2;
        double angle;
        double thickness;
        int clickCount;
        MouseButtonEventHandler handler = null;
        MouseEventHandler mouseMove = null;
        GeometryGroup group = new GeometryGroup();
        public Vector(string name, string color, double size, double thickness, double x = 0, double y = 0, double angle = 0)
        {
            this.Name = name;
            this.size = size;
            this.thickness = thickness;
            this.color = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString(color);
            firstPointLine = new Point();
            firstPointLine.X = x;
            firstPointLine.Y = y;
            this.angle = angle;
            tmp1 = new Point();
            tmp2 = new Point();
        }

        public override void Draw(OverridedCanvas canvas, Image image, int index = -1)
        {

            handler += (ss, ee) =>
            {

                Path myPath = new Path();
                EllipseGeometry myEllipseGeometry = new EllipseGeometry();
                LineGeometry myPathFigure = new LineGeometry();
                myPathFigure.StartPoint = new Point(0, 0);
                if (true)
                {
                    if (clickCount == 0)
                    {
                        tmp1 = ee.GetPosition(canvas);
                        firstPointLine = tmp1;
                        myPathFigure.StartPoint = tmp1;

                        myEllipseGeometry.Center = tmp1;
                        myEllipseGeometry.RadiusX = 2 * size;
                        myEllipseGeometry.RadiusY = 2 * size;
                        group.Children.Add(myEllipseGeometry);

                        var linetmp = new LineGeometry();
                        group.Children.Add(linetmp);
                        drawCompleteLine(ee, canvas, size);

                        myPath.Stroke = color;
                        myPath.StrokeThickness = thickness;
                        myPath.Data = group;
                        myPath.Tag = Name;
                        canvas.AddLogicalChild(myPath, index);

                        clickCount++;
                    }
                    else
                    {
                        AddElementToSaveList(canvas.Tag.ToString(), index);
                        canvas.Children[canvas.Children.Count - 1].Opacity = 0.5;
                        clickCount = 0;
                        group = null;
                        group = new GeometryGroup();
                    }
                }

            };

            mouseMove += (ss, ee) =>
            {
                if (clickCount == 1)
                {
                    drawCompleteLine(ee, canvas, size);
                }
            };
            image.MouseMove += mouseMove;
            image.MouseRightButtonDown += handler;
            canvas.MouseRightButtonDown += handler;
        }

        private void drawCompleteLine(MouseEventArgs ee, Canvas canvas, double size)
        {
            tmp2 = ee.GetPosition(canvas);
            double deltaX = tmp2.X - tmp1.X;
            double deltaY = tmp2.Y - tmp1.Y;
            angle = (Math.Atan2(deltaY, deltaX));

            tmp2.X = tmp1.X + Math.Cos(angle) * 10;
            tmp2.Y = tmp1.Y + Math.Sin(angle) * 10;

            ((LineGeometry)group.Children[1]).StartPoint = new Point(tmp1.X + (2 * size) * Math.Cos(angle), tmp1.Y + (2 * size) * Math.Sin(angle));
            ((LineGeometry)group.Children[1]).EndPoint = tmp2;
        }

        public override void DeleteEvent(Image image, OverridedCanvas canvas)
        {
            image.MouseRightButtonDown -= handler;
            image.MouseMove -= mouseMove;
            canvas.MouseRightButtonDown -= handler;
        }
        public override string ToString()
        {
            return Name + ";" + Math.Floor(firstPointLine.X).ToString() + ";" + Math.Floor(firstPointLine.Y).ToString() + ";" + angle.ToString();
        }

        public override void DrawFromFile(OverridedCanvas canvas)
        {
            Path myPath = new Path();
            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            LineGeometry myPathFigure = new LineGeometry();

            myEllipseGeometry.Center = firstPointLine;
            myEllipseGeometry.RadiusX = 2 * size;
            myEllipseGeometry.RadiusY = 2 * size;
            group.Children.Add(myEllipseGeometry);
            tmp2.X = firstPointLine.X + Math.Cos(angle) * 10;
            tmp2.Y = firstPointLine.Y + Math.Sin(angle) * 10;
            myPathFigure.StartPoint = new Point(firstPointLine.X + (2 * size) * Math.Cos(angle), firstPointLine.Y + (2 * size) * Math.Sin(angle));
            myPathFigure.EndPoint = tmp2;
            group.Children.Add(myPathFigure);
            myPath.Stroke = color;
            myPath.StrokeThickness = thickness;
            myPath.Data = group;
            myPath.Tag = Name;
            canvas.AddLogicalChild(myPath);
            canvas.Children[canvas.Children.Count - 1].Opacity = 0.5;
        }
    }
}
