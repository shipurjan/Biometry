using Fingerprints.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints.MinutiaeTypes.CurveLine
{
    class FileCurveLine : CurveLine, IDraw
    {
        public FileCurveLine(MinutiaState state) : base(state)
        {
        }
        public void Draw(OverridedCanvas canvas, Image image, int index = -1)
        {
            List<Point> curvePoint = new List<Point>();
            PointCollection curvePoints = new PointCollection();
            for (int i = 2; i < state.Points.Count() - 2; i += 2)
            {
                curvePoints.Add(new Point(Convert.ToInt32(state.Points[i]), Convert.ToInt32(state.Points[i + 1])));
            }

            Polyline polyLine = new Polyline()
            {
                Stroke = color,
                StrokeThickness = state.Minutia.Thickness,
                SnapsToDevicePixels = true,
                StrokeLineJoin = PenLineJoin.Miter,
                StrokeMiterLimit = 2,
                Tag = state.Minutia.Name,
                Uid = state.Id.ToString(),
            };
            polyLine.Points = curvePoints;
            canvas.AddLogicalChild(polyLine);
            canvas.Children[canvas.Children.Count - 1].Opacity = 0.5;
        }

        public void Stop(Image image, OverridedCanvas canvas)
        {
        }
    }
}
