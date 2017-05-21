using Fingerprints.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints.MinutiaeTypes.Triangle
{
    class FileTriangle : Triangle, IDraw
    {
        public FileTriangle(MinutiaState state) : base(state)
        {
            this.state = state;
        }
        public void Draw(OverridedCanvas canvas, Image image, int index = -1)
        {
            Path myPath = new Path();
            LineGeometry myFirstPathFigure = new LineGeometry();
            LineGeometry mySecondPathFigure = new LineGeometry();
            LineGeometry myThirdPathFigure = new LineGeometry();

            myFirstPathFigure.StartPoint = state.Points[0];
            myFirstPathFigure.EndPoint = state.Points[1];
            group.Children.Add(myFirstPathFigure);

            mySecondPathFigure.StartPoint = state.Points[1];
            mySecondPathFigure.EndPoint = state.Points[2];
            group.Children.Add(mySecondPathFigure);

            myThirdPathFigure.StartPoint = state.Points[2];
            myThirdPathFigure.EndPoint = state.Points[0];
            group.Children.Add(myThirdPathFigure);

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
