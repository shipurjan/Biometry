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
        double thickness;
        Point singlePoint = new Point();
        MouseButtonEventHandler handler = null;

        public SinglePoint(string name, string color, double size, double thickness, double x = 0, double y = 0)
        {
            this.thickness = thickness;
            this.Name = name;
            this.size = size;
            this.color = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString(color);
            singlePoint.X = x;
            singlePoint.Y = y;
        }
        public override void Draw(OverridedCanvas canvas, Image image, RadioButton radioButton1, RadioButton radioButton2)
        {            
            handler += (ss, ee) =>
            {
                if (ee.RightButton == MouseButtonState.Pressed && radioButton1.IsChecked == true)
                {
                    singlePoint = ee.GetPosition(canvas);
                    EllipseGeometry myEllipseGeometry = new EllipseGeometry();
                    myEllipseGeometry.Center = singlePoint;
                    myEllipseGeometry.RadiusX = 2 * size;
                    myEllipseGeometry.RadiusY = 2 * size;
                    Path myPath = new Path();
                    myPath.Stroke = color;
                    myPath.StrokeThickness = thickness;
                    myPath.Data = myEllipseGeometry;
                    //myPath.Opacity = 0.5;
                    myPath.Tag = Name;
                    canvas.AddLogicalChild(myPath);
                    radioButton1.IsChecked = false;
                    radioButton2.IsChecked = true;
                    if (radioButton1.Name == "activeCanvasL")
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

        public override void DeleteEvent(Image image, OverridedCanvas canvas)
        {
            image.MouseRightButtonDown -= handler;
        }

        public override string ToString()
        {
            return Name + ";" + Math.Floor(singlePoint.X).ToString() + ";" + Math.Floor(singlePoint.Y).ToString();
        }

        public void DrawFromFile(OverridedCanvas canvas)
        {
            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = singlePoint;
            myEllipseGeometry.RadiusX = 2 * size;
            myEllipseGeometry.RadiusY = 2 * size;
            Path myPath = new Path();
            myPath.Stroke = color;
            myPath.StrokeThickness = 0.3;
            myPath.Data = myEllipseGeometry;
            myPath.Opacity = 0.5;
            myPath.Tag = Name;
            canvas.AddLogicalChild(myPath);
        }
    }
}
