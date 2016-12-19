using Fingerprints.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints
{
    public class CurveLine : Minutiae
    {
        Brush color;
        Point currentPoint;
        bool newLine;
        Polyline baseLine;
        string[] points;
        double thickness;

        MouseEventHandler handler = null;

        public CurveLine(string name, string color, double thickness, string[] points = null, long id = 0) : base(id)
        {
            this.thickness = thickness;
            newLine = true;
            this.points = points;
            this.Name = name;
            this.color = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString(color);
        }
        /// <summary>
        /// Dodaje handlery do myszy, rysuje linie ciagla, zapisuje jako liste puktow
        /// </summary>
        /// <param name="canvas">Atkualny canvas</param>
        /// <param name="image">Aktualny obrazek</param>
        /// <param name="border1">Ramka 1</param>
        /// <param name="border2">Ramka 2</param>
        public override void Draw(OverridedCanvas canvas, Image image, int index = -1)
        {
            window.acceptLeftCurveButton.Visibility = Visibility.Visible;
            window.acceptRightCurveButton.Visibility = Visibility.Visible;

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
                            StrokeThickness = thickness,
                            StrokeLineJoin = PenLineJoin.Miter,
                            StrokeMiterLimit = 0,
                            Tag = Name,
                        };
                        id = getIdForMinutiae(canvas.Tag.ToString(), index);
                        baseLine.Uid = id.ToString();
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

        public override void DeleteEvent(Image image, OverridedCanvas canvas)
        {
            window.acceptLeftCurveButton.Visibility = Visibility.Hidden;
            window.acceptRightCurveButton.Visibility = Visibility.Hidden;
            image.MouseMove -= handler;
            canvas.MouseMove -= handler;
        }
        public override string ToString()
        {
            string points = null;
            if (baseLine != null && baseLine.Points.Count > 0)
            {
                foreach (var point in convertLinesToPoints(baseLine.Points))
                {
                    points += point.X + ";" + point.Y + ";";
                }
                return id + ";" + Name + ";" + points;
            }
            return "";
        }

        public void acceptButtonsClickEvents(OverridedCanvas canvas, int index = -1)
        {
            if (canvas.Tag.ToString() == "Left")
            {
                window.acceptLeftCurveButton.Click += (ss, ee) =>
                {
                    newLine = true;
                    this.AddElementToSaveList(canvas.Tag.ToString(), index);
                    this.baseLine = null;
                };
            }
            else
            {
                window.acceptRightCurveButton.Click += (ss, ee) =>
                {
                    newLine = true;
                    this.AddElementToSaveList(canvas.Tag.ToString(), index);
                    this.baseLine = null;
                };
            }
        }
        public override void DrawFromFile(OverridedCanvas canvas)
        {
            List<Point> curvePoint = new List<Point>();
            PointCollection curvePoints = new PointCollection();
            for (int i = 2; i < points.Count() - 2; i += 2)
            {
                curvePoints.Add(new Point(Convert.ToInt32(points[i]), Convert.ToInt32(points[i + 1])));
            }

            Polyline polyLine = new Polyline()
            {
                Stroke = color,
                StrokeThickness = thickness,
                SnapsToDevicePixels = true,
                StrokeLineJoin = PenLineJoin.Miter,
                StrokeMiterLimit = 2,
                Tag = Name,
                Uid = id.ToString(),
            };
            polyLine.Points = curvePoints;
            canvas.AddLogicalChild(polyLine);
            canvas.Children[canvas.Children.Count - 1].Opacity = 0.5;
        }

        // Converter
        public PointCollection convertLinesToPoints(PointCollection points)
        {
            double a, b, c = 0;
            PointCollection convertedPoints = new PointCollection();

            for (int i = 0; i < points.Count - 1; i++)
            {
                Point p1, p2;
                p1 = points[i];
                p2 = points[i + 1];

                a = calculateA(p1, p2);
                b = calculateB(p1, p2);
                c = calculateC(p1, p2);

                if (p1.X < p2.X && b != 0)
                {

                    for (double x = p1.X; x <= p2.X; x++)
                    {
                        Point createdPoint = createPoint(a, b, c, x);
                        convertedPoints = fillSpaceBetweenPointsAndAddPoint(convertedPoints, createdPoint);
                    }
                }
                else if (b != 0)
                {
                    for (double x = p1.X; x >= p2.X; x--)
                    {
                        Point createdPoint = createPoint(a, b, c, x);
                        convertedPoints = fillSpaceBetweenPointsAndAddPoint(convertedPoints, createdPoint);
                    }
                }

            }
            return convertedPoints;
        }

        public PointCollection fillSpaceBetweenPointsAndAddPoint(PointCollection points, Point p1)
        {
            if (points.LastOrDefault() != new Point() { X = 0, Y = 0 })
            {
                while (lengthOfLine(p1, points.LastOrDefault()) > 1.5)
                {
                    Point lastPoint = points.LastOrDefault();
                    if (p1.Y < lastPoint.Y)
                    {
                        lastPoint.Y -= 1;
                    }
                    else
                    {
                        lastPoint.Y += 1;
                    }
                    points.Add(lastPoint);
                }
            }
            points.Add(p1);

            return points;
        }

        public double calculateA(Point point1, Point point2)
        {
            return point1.Y - point2.Y;
        }

        public double calculateB(Point point1, Point point2)
        {
            return point2.X - point1.X;
        }

        public double calculateC(Point point1, Point point2)
        {
            return (point1.X - point2.X) * point1.Y + (point2.Y - point1.Y) * point1.X;
        }

        public Point createPoint(double a, double b, double c, double x)
        {
            return new Point() { X = x, Y = Math.Floor(((-a * x) - c) / b) };
        }

        public double lengthOfLine(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
        }

    }
}