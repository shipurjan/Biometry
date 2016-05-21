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
        Point firstPointLine;
        Point tmp1, tmp2;
        double angle;
        int clickCount;
        MouseButtonEventHandler handler = null;
        MouseEventHandler mouseMove = null;
        public Vector()
        {
            firstPointLine = new Point();
            tmp1 = new Point();
            tmp2 = new Point();
        }
        public override void Draw(Canvas canvas, Image image)
        {
            
           
            handler += (ss, ee) =>
            {
                if (clickCount == 0)
                {
                    tmp1 = ee.GetPosition(canvas);
                    firstPointLine = tmp1;
                    EllipseGeometry myEllipseGeometry = new EllipseGeometry();
                    myEllipseGeometry.Center = tmp1;
                    myEllipseGeometry.RadiusX = 2;
                    myEllipseGeometry.RadiusY = 2;
                    Path myPath = new Path();
                    myPath.Stroke = Brushes.Red;
                    myPath.StrokeThickness = 0.3;
                    myPath.Data = myEllipseGeometry;
                    canvas.Children.Add(myPath);
                    clickCount++;

                }
                else
                {
                    tmp2 = ee.GetPosition(canvas);
                    clickCount = 0;
                    var linetmp = new Line();
                        

                    double deltaX = tmp2.X - tmp1.X;
                    double deltaY = tmp2.Y - tmp1.Y;
                    linetmp.Stroke = Brushes.Red;
                    double angle = (Math.Atan2(deltaY, deltaX));
                    linetmp.X1 = tmp1.X + 2 * Math.Cos(angle);
                    linetmp.Y1 = tmp1.Y + 2 * Math.Sin(angle);
                    tmp2.X = tmp1.X + Math.Cos(angle) * 10;
                    tmp2.Y = tmp1.Y + Math.Sin(angle) * 10;
                    linetmp.X2 = tmp2.X;
                    linetmp.Y2 = tmp2.Y;
                    linetmp.StrokeThickness = 0.3;
                    canvas.Children.Add(linetmp);
                    //image.MouseRightButtonDown -= handler;
                }
            };




            image.MouseRightButtonDown += handler;
        }

        public void DeleteEvent(Canvas canvas, Image image, Button button)
        {
            image.MouseRightButtonDown -= handler;

        }
    }
}
