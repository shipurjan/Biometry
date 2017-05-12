using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Fingerprints.Models;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace Fingerprints.MinutiaeTypes.Vector
{
    class FileVector : Vector, IDraw
    {
        public FileVector(MinutiaState state) : base(state)
        {
        }

        public void Draw(OverridedCanvas canvas, Image image, int index = -1)
        {
            Path myPath = new Path();
            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            LineGeometry myPathFigure = new LineGeometry();

            myEllipseGeometry.Center = state.Points[0];
            myEllipseGeometry.RadiusX = 2 * state.Minutia.Size;
            myEllipseGeometry.RadiusY = 2 * state.Minutia.Size;
            group.Children.Add(myEllipseGeometry);
            tmp2.X = state.Points[0].X + Math.Cos(state.Angle) * 10;
            tmp2.Y = state.Points[0].Y + Math.Sin(state.Angle) * 10;
            myPathFigure.StartPoint = new Point(state.Points[0].X + (2 * state.Minutia.Size) * Math.Cos(state.Angle), state.Points[0].Y + (2 * state.Minutia.Size) * Math.Sin(state.Angle));
            myPathFigure.EndPoint = tmp2;
            group.Children.Add(myPathFigure);
            myPath.Stroke = color;
            myPath.StrokeThickness = state.Minutia.Thickness;
            myPath.Data = group;
            myPath.Tag = state.Minutia.Name;
            myPath.Opacity = 0.5;
            myPath.Uid = state.Id.ToString();
            canvas.AddLogicalChild(myPath);
        }

        public void Stop(Image image, OverridedCanvas canvas)
        {

        }
    }
}
