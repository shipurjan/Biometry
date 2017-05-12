using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Fingerprints.Models;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints.MinutiaeTypes.SinglePoint
{
    class FileSinglePoint : SinglePoint, IDraw
    {
        public FileSinglePoint(MinutiaState state) : base(state)
        {
        }

        public void Draw(OverridedCanvas canvas, Image image, int index = -1)
        {
            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = state.Points[0];
            myEllipseGeometry.Center = state.Points[0];
            myEllipseGeometry.RadiusX = 2 * state.Minutia.Size;
            myEllipseGeometry.RadiusY = 2 * state.Minutia.Size;
            Path myPath = new Path();
            myPath.Stroke = color;
            myPath.StrokeThickness = 0.3;
            myPath.Data = myEllipseGeometry;
            myPath.Opacity = 0.5;
            myPath.Tag = state.Minutia.Name;
            myPath.Uid = state.Id.ToString();
            canvas.AddLogicalChild(myPath);
        }
        public void Stop(Image image, OverridedCanvas canvas)
        {
        }
    }
}
