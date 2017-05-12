using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Fingerprints.Models;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints.MinutiaeTypes.Empty
{
    class UserEmpty : Empty, IDraw
    {
        public UserEmpty(MinutiaState state) : base(state)
        {
        }

        public void Draw(OverridedCanvas canvas, Image image, int index = -1)
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
            canvas.AddLogicalChild(myPath, index);
            AddElementToSaveList(canvas.Tag.ToString(), index);
        }

        public void Stop(Image image, OverridedCanvas canvas)
        {
        }
    }
}
