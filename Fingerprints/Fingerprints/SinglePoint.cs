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

        MouseButtonEventHandler handler = null;
        public override void Draw(Canvas canvas, Image image)
        {
            Point SinglePoint = new Point();
            handler += (ss, ee) =>
            {
                if (ee.RightButton == MouseButtonState.Pressed)
                {
                        SinglePoint = ee.GetPosition(canvas);;
                        EllipseGeometry myEllipseGeometry = new EllipseGeometry();
                        myEllipseGeometry.Center = SinglePoint;
                        myEllipseGeometry.RadiusX = 2;
                        myEllipseGeometry.RadiusY = 2;
                        Path myPath = new Path();
                        myPath.Stroke = Brushes.Red;
                        myPath.StrokeThickness = 0.3;
                        myPath.Data = myEllipseGeometry;
                        canvas.Children.Add(myPath);                   
                }

            };
            image.MouseRightButtonDown += handler;
        }
    }
}
