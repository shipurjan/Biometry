using Fingerprints.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints.MinutiaeTypes.Peak
{
    class FilePeak : Peak, IDraw
    {
        public FilePeak(MinutiaState state) : base(state)
        {
        }

        public void Draw(OverridedCanvas canvas, Image image, int index = -1)
        {
            Path myPath = new Path();
            LineGeometry myFirstPathFigure = new LineGeometry();
            LineGeometry mySecondPathFigure = new LineGeometry();

            myFirstPathFigure.StartPoint = state.Points[0];
            myFirstPathFigure.EndPoint = state.Points[1];
            group.Children.Add(myFirstPathFigure);
            mySecondPathFigure.StartPoint = state.Points[1];
            mySecondPathFigure.EndPoint = state.Points[2];
            group.Children.Add(mySecondPathFigure);

            myPath.Stroke = color;
            myPath.StrokeThickness = state.Minutia.Thickness;
            myPath.Data = group;
            myPath.Tag = state.Minutia.Name;
            myPath.Uid = state.Id.ToString();
            canvas.AddLogicalChild(myPath);
            canvas.Children[canvas.Children.Count - 1].Opacity = 0.5;
        }

        public void Stop(Image image, OverridedCanvas canvas)
        {
        }
    }
}
