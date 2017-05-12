using System.Windows.Controls;
using Fingerprints.Models;
using System.Windows.Input;
using System.Windows.Media;
using Fingerprints.Resources;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows;

namespace Fingerprints.MinutiaeTypes.SinglePoint
{
    class UserSinglePoint : SinglePoint, IDraw
    {
        MouseButtonEventHandler handler = null;

        public UserSinglePoint(MinutiaState state) : base(state)
        {
            state.Points = new List<Point>();
        }
        public void Draw(OverridedCanvas canvas, Image image, int index = -1)
        {
            AddHandler(canvas, image, index);
            image.MouseRightButtonDown += handler;
        }

        public void Stop(Image image, OverridedCanvas canvas)
        {
            image.MouseRightButtonDown -= handler;
        }

        private void AddHandler(OverridedCanvas canvas, Image image, int index)
        {
            handler += (ss, ee) =>
            {
                if (ee.RightButton == MouseButtonState.Pressed)
                {
                    AddToCanvas(ss, ee, canvas, image, index);
                    index = -1;
                }
            };
        }

        private void AddToCanvas(object sender, MouseButtonEventArgs ee, OverridedCanvas canvas, Image image, int index)
        {
            state.Points.Insert(0, ee.GetPosition(canvas).ToFloorPoint());
            state.Id = getIdForMinutiae(canvas.Tag.ToString(), index);

            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = state.Points[0];
            myEllipseGeometry.RadiusX = 2 * state.Minutia.Size; ;
            myEllipseGeometry.RadiusY = 2 * state.Minutia.Size; ;
            Path myPath = new Path();

            myPath.Stroke = color;
            myPath.StrokeThickness = state.Minutia.Thickness;
            myPath.Data = myEllipseGeometry;
            myPath.Tag = state.Minutia.Name;
            myPath.Uid = state.Id.ToString();

            DeleteEmptyAtIndex(canvas, index);
            AddEmptyToOpositeSite(canvas, index);
            canvas.AddLogicalChild(myPath, index);
            AddElementToSaveList(canvas.Tag.ToString(), index);
        }
    }
}
