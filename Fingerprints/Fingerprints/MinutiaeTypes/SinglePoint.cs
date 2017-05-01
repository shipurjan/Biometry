using Fingerprints.Models;
using Fingerprints.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    class SinglePoint : Minutiae, IDraw
    {

        MouseButtonEventHandler handler = null;

        public SinglePoint(MinutiaState state)
        {
            this.state = state;
            ConvertStateColorToBrush();
        }


        public void Draw(OverridedCanvas canvas, Image image, int index = -1)
        {
            AddHandler(canvas, image, index);
            image.MouseRightButtonDown += handler;
        }

        public void DeleteEvent(Image image, OverridedCanvas canvas)
        {
            image.MouseRightButtonDown -= handler;
        }

        public override string ToString()
        {
            return state.Id + ";" + state.Minutia.Name + ";" + Math.Floor(state.Points[0].X).ToString() + ";" + Math.Floor(state.Points[0].Y).ToString();
        }

        public void DrawFromFile(OverridedCanvas canvas)
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

        public string ToJson()
        {
            JObject minutiaeJson = new JObject();
            minutiaeJson["id"] = state.Id;
            minutiaeJson["name"] = state.Minutia.Name;
            minutiaeJson["points"] = new JArray()
            {
                state.Points[0].ToJObject()
            };

            return minutiaeJson.ToString();
        }
    }
}
