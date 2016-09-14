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
    class CurveLine : Minutiae
    {
        Brush color;
        Polyline baseLine;
        Point currentPoint;
        bool newLine;
        bool clickCount = true;
        string[] points;
        double thickness;
        Button closeEventButton;
        
        MouseButtonEventHandler handlerMouseDown = null;
        MouseEventHandler handler = null;

        public CurveLine(string color, double thickness, string name = "Krzywa", string[] points = null, Button button = null)
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
                            SnapsToDevicePixels = true
                        };
                        baseLine.Tag = Name;
                        baseLine.SnapsToDevicePixels = true;
                        canvas.AddLogicalChild(baseLine);
                        newLine = false; 
                    }
                    currentPoint.X = Math.Floor(ee.GetPosition(canvas).X +0.5);
                    currentPoint.Y = Math.Floor(ee.GetPosition(canvas).Y +0.5);
                    

                    if (baseLine.Points.LastOrDefault() != currentPoint)
                    {
                        Console.WriteLine(currentPoint.X + " " + currentPoint.Y);
                        baseLine.Points.Add(currentPoint);
                    }

                    //clickCount = false;
                }
                if (ee.RightButton == MouseButtonState.Released && clickCount == false)
                {
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

                    if (radioButton1.IsChecked == false)
                    {
                        radioButton2.IsChecked = false;
                        radioButton1.IsChecked = true;
                        clickCount = true;
                    }
                    else
                    {
                        radioButton1.IsChecked = false;
                        radioButton2.IsChecked = true;
                        clickCount = true;
                    }
                }
            };
            image.MouseMove += handler;
            canvas.MouseMove += handler;
        }

        public override void DeleteEvent(Image image, OverridedCanvas canvas)
        {
            image.MouseMove -= handler;
            canvas.MouseMove += handler;
        }
        public override string ToString()
        {
            string points = null;
            if (baseLine != null && baseLine.Points.Count > 0)
            {
                foreach (var point in baseLine.Points)
                {
                    points += point.X + ";" + point.Y + ";";
                }
            }
            return Name + ";" + points;
        }

        public void DrawFromFile(OverridedCanvas canvas)
        {
            List<Point> curvePoint = new List<Point>();
            PointCollection curvePoints = new PointCollection();
            for (int i = 1; i < points.Count()-2; i+=2)
            {
                curvePoints.Add(new Point(Convert.ToInt32(points[i]), Convert.ToInt32(points[i + 1])));
            }

            Polyline polyLine = new Polyline()
            {
                Stroke = color,
                StrokeThickness = thickness,
                SnapsToDevicePixels = true,
            };
            polyLine.Points = curvePoints;
            polyLine.Tag = Name;
            canvas.AddLogicalChild(polyLine);
        }

    }
}

