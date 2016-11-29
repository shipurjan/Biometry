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
    class Segment : Minutiae
    {
        Brush color;
        Point tmp1, tmp2;
        int clickCount;
        Point firstPointLine;
        Point secondPointLine;
        MouseButtonEventHandler handler = null;
        MouseEventHandler mouseMove = null;
        GeometryGroup group = new GeometryGroup();
        double thickness;

        public Segment(string name, string color, double thickness, double x1 = 0, double y1 = 0, double x2 = 0, double y2 = 0)
        {
            this.thickness = thickness;
            this.Name = name;
            this.color = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString(color);
            firstPointLine = new Point();
            firstPointLine.X = x1;
            firstPointLine.Y = y1;
            secondPointLine = new Point();
            secondPointLine.X = x2;
            secondPointLine.Y = y2;

            tmp1 = new Point();
            tmp2 = new Point();
        }

        public override void Draw(OverridedCanvas canvas, Image image, int index = -1)
        {
            handler += (ss, ee) =>
            {
                Path myPath = new Path();

                if (clickCount == 0)
                {
                    tmp1 = ee.GetPosition(canvas);
                    firstPointLine = tmp1;
                    var linetmp = new LineGeometry();
                    group.Children.Add(linetmp);
                    drawCompleteLine(ee, canvas, clickCount);

                    myPath.Stroke = color;
                    myPath.StrokeThickness = thickness;
                    myPath.Data = group;
                    myPath.Tag = Name;
                    deleteChildWithGivenIndex(canvas.Tag.ToString(), index);
                    canvas.AddLogicalChild(myPath, index);
                    clickCount++;
                }
                else
                {
                    var linetmp = new LineGeometry();
                    group.Children.Add(linetmp);
                    drawCompleteLine(ee, canvas, clickCount);
                    myPath.Stroke = color;
                    myPath.StrokeThickness = thickness;
                    myPath.Data = group;
                    myPath.Tag = Name;
                    //canvas.Children.RemoveAt(canvas.Children.Count - 1);
                    //canvas.AddLogicalChild(myPath, index);
                    deleteAndAdd(canvas, myPath, index);
                    clickCount = 0;
                    AddElementToSaveList(canvas.Tag.ToString(), index);
                    group = null;
                    group = new GeometryGroup();
                    index = -1;
                }

            };

            mouseMove += (ss, ee) =>
            {
                if (clickCount > 0 && clickCount < 3)
                {
                    drawCompleteLine(ee, canvas, clickCount);
                }
            };
            image.MouseMove += mouseMove;
            image.MouseRightButtonDown += handler;
            canvas.MouseRightButtonDown += handler;
        }
        private void drawCompleteLine(MouseEventArgs ee, OverridedCanvas canvas, int clickCount)
        {
            if (clickCount == 1)
            {
                tmp2 = ee.GetPosition(canvas);
                secondPointLine = tmp2;
                ((LineGeometry)group.Children[clickCount - 1]).StartPoint = tmp1;
                ((LineGeometry)group.Children[clickCount - 1]).EndPoint = tmp2;
            }
        }

        public override void DeleteEvent(Image image, OverridedCanvas canvas)
        {
            image.MouseRightButtonDown -= handler;
            image.MouseMove -= mouseMove;
            canvas.MouseRightButtonDown -= handler;
        }
        public override string ToString()
        {
            return Name + ";" + Math.Floor(firstPointLine.X).ToString() + ";" + Math.Floor(firstPointLine.Y).ToString() + ";" + Math.Floor(secondPointLine.X).ToString() + ";" + Math.Floor(secondPointLine.Y).ToString();
        }
        public void deleteAndAdd(OverridedCanvas canvas, Path myPath, int index = -1)
        {
            if (index == -1)
            {
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
            }
            else
            {
                canvas.Children.RemoveAt(index);
            }

            canvas.AddLogicalChild(myPath, index);
        }

        public override void DrawFromFile(OverridedCanvas canvas)
        {
            Path myPath = new Path();
            LineGeometry myFirstPathFigure = new LineGeometry();

            myFirstPathFigure.StartPoint = firstPointLine;
            myFirstPathFigure.EndPoint = secondPointLine;
            group.Children.Add(myFirstPathFigure);

            myPath.Stroke = color;
            myPath.StrokeThickness = thickness;
            myPath.Data = group;
            myPath.Tag = Name;
            canvas.AddLogicalChild(myPath);
        }

    }
}
