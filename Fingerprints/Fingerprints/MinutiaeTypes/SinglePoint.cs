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
    class SinglePoint : Minutiae
    {
        Brush color;
        double size;
        Point singlePoint = new Point();
        MouseButtonEventHandler handler = null;

        public SinglePoint(string name, Brush color, double size)
        {
            this.Name = name;
            this.size = size;
            this.color = color;
        }
        public override void Draw(Canvas canvas, Image image, Border border1, Border border2)
        {            
            handler += (ss, ee) =>
            {
                if (ee.RightButton == MouseButtonState.Pressed && border1.BorderBrush == Brushes.DeepSkyBlue)
                {
                    singlePoint = ee.GetPosition(canvas);;
                    EllipseGeometry myEllipseGeometry = new EllipseGeometry();
                    myEllipseGeometry.Center = singlePoint;
                    myEllipseGeometry.RadiusX = 2 * size;
                    myEllipseGeometry.RadiusY = 2 * size;
                    Path myPath = new Path();
                    myPath.Stroke = color;
                    myPath.StrokeThickness = 0.3;
                    myPath.Data = myEllipseGeometry;
                    myPath.Opacity = 0.5;
                    canvas.Children.Add(myPath);
                    border1.BorderBrush = Brushes.Black;
                    border2.BorderBrush = Brushes.DeepSkyBlue;
                    if (border1.Tag.ToString() == "Left")
                    {
                        FileTransfer.ListL.Add(ToString());
                    }
                    else
                    {
                        FileTransfer.ListR.Add(ToString());
                    }
                }

            };
            image.MouseRightButtonDown += handler;
        }

        public override void DeleteEvent(Image image)
        {
            image.MouseRightButtonDown -= handler;
        }

        public override string ToString()
        {
            return Name + ";" + singlePoint.X.ToString() + "," + singlePoint.Y.ToString();
        }
    }
}
