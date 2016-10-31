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
        GeometryGroup oldGroup;
        Point currentPoint;
        bool newLine;
        Polyline baseLine;
        bool clickCount = true;
        string[] points;
        double thickness;
        Button closeEventButton;
        
        MouseEventHandler handler = null;

        public CurveLine(string name, string color, double thickness, string[] points = null, Button button = null)
        {
            this.thickness = thickness;
            closeEventButton = button;
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
        public override void Draw(OverridedCanvas canvas, Image image, RadioButton radioButton1, RadioButton radioButton2)
        {
            closeEventButton.Click += (ss, ee) =>
            {
                clickCount = false;
                newLine = true;
            };

            radioButton1.Unchecked += (ss, ee) =>
            {
                clickCount = false;
                newLine = true;

                if (radioButton1.Name == "activeCanvasL")
                {
                    if (FileTransfer.ListL.Count > 0)
                    {
                        if (FileTransfer.ListL.Last().ToString() != ToString())
                        {
                            FileTransfer.ListL.Add(ToString());
                        }
                    }
                    else
                    {
                        FileTransfer.ListL.Add(ToString());
                    }

                }
                else
                {
                    if (FileTransfer.ListR.Count > 0)
                    {
                        if (FileTransfer.ListR.Last().ToString() != ToString())
                        {
                            FileTransfer.ListR.Add(ToString());
                        }
                    }
                    else
                    {
                        FileTransfer.ListR.Add(ToString());
                    }
                }
            };

            handler += (ss, ee) =>
            {
                if (ee.RightButton == MouseButtonState.Pressed && radioButton1.IsChecked == true)
                {
                    if (newLine)
                    {
                        baseLine = new Polyline
                        {
                            Stroke = color,
                            StrokeThickness = thickness,
                            SnapsToDevicePixels = true,
                            StrokeLineJoin = PenLineJoin.Miter,
                            StrokeMiterLimit = 2,
                            Tag = Name,
                        };
                        canvas.AddLogicalChild(baseLine);
                        newLine = false;
                    }
                    currentPoint.X = Math.Floor(ee.GetPosition(canvas).X + 0.5);
                    currentPoint.Y = Math.Floor(ee.GetPosition(canvas).Y + 0.5);


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
                    //points += point.X + ";" + point.Y + ";";
                }
            }
            return Name + ";" + points;
        }

        public override void DrawFromFile(OverridedCanvas canvas)
        {
            List<Point> curvePoint = new List<Point>();
            PointCollection curvePoints = new PointCollection();
            for (int i = 1; i < points.Count() - 2; i += 2)
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
            };
            polyLine.Points = curvePoints;
            canvas.AddLogicalChild(polyLine);
        }

        public PointCollection convertLinesToPoints(PointCollection points)
        {
            double a, b, c = 0;
            PointCollection all = new PointCollection();

            for (int i = 0; i < points.Count - 1; i++)
            {
                a = calculateA(points[i], points[i + 1]);
                b = calculateB(points[i], points[i + 1]);
                c = calculateC(points[i], points[i + 1]);
                Point p2 = points[i + 1]; // important assignment becouse is a bug when "points[i + 1]" is in for loop
                for (double j = points[i].X; j <= p2.X; j++)
                {
                    Point p = new Point() { X = j, Y = ((-a * j) - c) / b };
                    all.Add(p);
                }
            }
            return all;
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

    }
}