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
        GeometryGroup oldGroup;
        Point currentPoint;
        bool newLine;
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
            };

            handler += (ss, ee) =>
            {
                if (ee.RightButton == MouseButtonState.Pressed && radioButton1.IsChecked == true)
                {
                    if (currentPoint == new Point(0, 0))
                    {
                        currentPoint = ee.GetPosition(canvas);
                    }

                    LineGeometry lineGeo = new LineGeometry();
                    lineGeo.StartPoint = new Point(Math.Floor(currentPoint.X), Math.Floor(currentPoint.Y));
                    lineGeo.EndPoint = new Point(Math.Floor(ee.GetPosition(canvas).X), Math.Floor(ee.GetPosition(canvas).Y));

                    currentPoint = lineGeo.EndPoint;

                    GeometryGroup newGroup = new GeometryGroup();
                    newGroup.Children.Add(lineGeo);

                    Path path = new Path();
                    path.Stroke = color;
                    path.Tag = Name;
                    path.Fill = color;
                    path.StrokeThickness = thickness;
                    path.SnapsToDevicePixels = true;
                    path.Data = newGroup;

                    if (newLine)
                    {
                        canvas.AddLogicalChild(path);
                        newLine = false;
                    }
                    else
                    {
                        oldGroup = (GeometryGroup)((Path)canvas.Children[canvas.Children.Count - 1]).Data;
                        oldGroup.Children.Add(newGroup.Children.FirstOrDefault());
                        ((Path)canvas.Children[canvas.Children.Count - 1]).Data = oldGroup;
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
            canvas.MouseMove -= handler;
        }
        public override string ToString()
        {
            string points = null;
            if (oldGroup != null && oldGroup.Children.Count > 0)
            {
                foreach (var point in oldGroup.Children)
                {
                    points += ((LineGeometry)point).StartPoint.X + ";" + ((LineGeometry)point).StartPoint.Y + ";";
                }
                points += (((LineGeometry)oldGroup.Children.LastOrDefault()).EndPoint.X) + " " + (((LineGeometry)oldGroup.Children.LastOrDefault()).EndPoint.X);
            }
            return Name + ";" + points;
        }

        public override void DrawFromFile(OverridedCanvas canvas)
        {
            GeometryGroup newGroup = new GeometryGroup();

            for (int i = 1; i < points.Count() - 4; i += 2)
            {
                LineGeometry lineGeo = new LineGeometry();
                lineGeo.StartPoint = new Point(Convert.ToInt32(points[i]), Convert.ToInt32(points[i + 1]));
                lineGeo.EndPoint = new Point(Convert.ToInt32(points[i + 2]), Convert.ToInt32(points[i + 3]));
                newGroup.Children.Add(lineGeo);
            }

            Path path = new Path();
            path.Stroke = color;
            path.Tag = Name;
            path.Fill = color;
            path.StrokeThickness = thickness;
            path.SnapsToDevicePixels = true;
            path.Data = newGroup;
            canvas.AddLogicalChild(path);
        }

    }
}