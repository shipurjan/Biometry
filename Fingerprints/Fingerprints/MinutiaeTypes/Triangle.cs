using Fingerprints.Resources;
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
    class Triangle : Minutiae
    {
        Brush color;
        Point tmp1, tmp2;
        int clickCount;
        Point firstPointLine;
        Point secondPointLine;
        Point thirdPointLine;
        MouseButtonEventHandler handler = null;
        MouseEventHandler mouseMove = null;
        GeometryGroup group = new GeometryGroup();
        double thickness;


        public Triangle(string name, string color, double thickness, double x1 = 0, double y1 = 0, double x2 = 0, double y2 = 0, double x3 = 0, double y3 = 0, long id = 0) : base(id)
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
            thirdPointLine = new Point();
            thirdPointLine.X = x3;
            thirdPointLine.Y = y3;
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
                    id = getIdForMinutiae(canvas.Tag.ToString(), index);
                    tmp1 = ee.GetPosition(canvas);
                    firstPointLine = tmp1;
                    var linetmp = new LineGeometry();

                    DeleteEmptyAtIndex(canvas, index);
                    AddEmptyToOpositeSite(canvas, index);

                    group.Children.Add(linetmp);
                    drawCompleteLine(ee, canvas, clickCount);

                    myPath.Stroke = color;
                    myPath.StrokeThickness = thickness;
                    myPath.Data = group;
                    myPath.Tag = Name;
                    myPath.Uid = id.ToString();

                    canvas.AddLogicalChild(myPath, index);
                    clickCount++;
                }
                else if (clickCount == 1)
                {
                    var linetmp = new LineGeometry();
                    group.Children.Add(linetmp);
                    drawCompleteLine(ee, canvas, clickCount);
                    myPath.Stroke = color;
                    myPath.StrokeThickness = thickness;
                    myPath.Data = group;
                    myPath.Tag = Name;
                    myPath.Uid = id.ToString();

                    deleteAndAdd(canvas, myPath, index);
                    clickCount++;
                }
                else if (clickCount == 2)
                {
                    var linetmp = new LineGeometry();
                    group.Children.Add(linetmp);
                    drawCompleteLine(ee, canvas, clickCount);
                    clickCount++;
                    drawCompleteLine(ee, canvas, clickCount);
                    myPath.Stroke = color;
                    myPath.StrokeThickness = thickness;
                    myPath.Data = group;
                    myPath.Tag = Name;
                    myPath.Uid = id.ToString();

                    deleteAndAdd(canvas, myPath, index);
                    clickCount = 0;
                    AddElementToSaveList(canvas.Tag.ToString(), index);
                    index = -1;
                    group = null;
                    group = new GeometryGroup();
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
        private void drawCompleteLine(MouseEventArgs ee, OverridedCanvas canvas, int clickCount)
        {
            if (clickCount == 1)
            {
                tmp2 = ee.GetPosition(canvas);
                secondPointLine = tmp2;
                ((LineGeometry)group.Children[clickCount - 1]).StartPoint = tmp1;
                ((LineGeometry)group.Children[clickCount - 1]).EndPoint = tmp2;
            }
            else if (clickCount == 2)
            {
                tmp2 = ee.GetPosition(canvas);
                thirdPointLine = tmp2;
                ((LineGeometry)group.Children[clickCount - 1]).StartPoint = ((LineGeometry)group.Children[clickCount - 2]).EndPoint;
                ((LineGeometry)group.Children[clickCount - 1]).EndPoint = tmp2;
            }
            else if (clickCount == 3)
            {
                ((LineGeometry)group.Children[clickCount - 1]).StartPoint = ((LineGeometry)group.Children[clickCount - 2]).EndPoint;
                ((LineGeometry)group.Children[clickCount - 1]).EndPoint = ((LineGeometry)group.Children[0]).StartPoint;
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
            return id + ";" + Name + ";" + Math.Floor(firstPointLine.X).ToString() + ";" + Math.Floor(firstPointLine.Y).ToString() + ";" + Math.Floor(secondPointLine.X).ToString() + ";" + Math.Floor(secondPointLine.Y).ToString() + ";" + Math.Floor(thirdPointLine.X).ToString() + ";" + Math.Floor(thirdPointLine.Y).ToString();
        }

        public override void DrawFromFile(OverridedCanvas canvas)
        {
            Path myPath = new Path();
            LineGeometry myFirstPathFigure = new LineGeometry();
            LineGeometry mySecondPathFigure = new LineGeometry();
            LineGeometry myThirdPathFigure = new LineGeometry();

            myFirstPathFigure.StartPoint = firstPointLine;
            myFirstPathFigure.EndPoint = secondPointLine;
            group.Children.Add(myFirstPathFigure);

            mySecondPathFigure.StartPoint = secondPointLine;
            mySecondPathFigure.EndPoint = thirdPointLine;
            group.Children.Add(mySecondPathFigure);

            myThirdPathFigure.StartPoint = thirdPointLine;
            myThirdPathFigure.EndPoint = firstPointLine;
            group.Children.Add(myThirdPathFigure);

            myPath.Stroke = color;
            myPath.StrokeThickness = thickness;
            myPath.Data = group;
            myPath.Tag = Name;
            myPath.Opacity = 0.5;
            myPath.Uid = id.ToString();

            canvas.AddLogicalChild(myPath);
        }
    }
}
