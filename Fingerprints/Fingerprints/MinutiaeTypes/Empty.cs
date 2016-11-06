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
    class Empty : Minutiae
    {
        public override void Draw(OverridedCanvas canvas, Image image, RadioButton radioButton1, RadioButton radioButton2, int index = 0)
        {
            Point singlePoint = new Point(1, 1);
            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = singlePoint;
            myEllipseGeometry.RadiusX = 0;
            myEllipseGeometry.RadiusY = 0;
            Path myPath = new Path();
            myPath.StrokeThickness = 0.3;
            myPath.Data = myEllipseGeometry;
            myPath.Opacity = 1;
            myPath.Name = "Puste";
            myPath.Tag = "Puste";
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
            //canvas.AddLogicalChild(myPath);
        }

        public override void DrawFromFile(OverridedCanvas canvas)
        {
            Point singlePoint = new Point(1, 1);
            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = singlePoint;
            myEllipseGeometry.RadiusX = 0;
            myEllipseGeometry.RadiusY = 0;
            Path myPath = new Path();
            myPath.StrokeThickness = 0.3;
            myPath.Data = myEllipseGeometry;
            myPath.Opacity = 1;
            myPath.Name = "Puste";
            myPath.Tag = "Puste";
            //if (canvas.Tag.ToString() == "Left")
            //{
            //    FileTransfer.ListL.Add(ToString());
            //}
            //if (canvas.Tag.ToString() == "Right")
            //{
            //    FileTransfer.ListR.Add(ToString());
            //}
            canvas.AddLogicalChild(myPath);
        }

        public override string ToString()
        {
            return "Puste";
        }
    }
}
