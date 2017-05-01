using Fingerprints.Models;
using Fingerprints.Resources;
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
    class Segment : Minutiae, IDraw
    {
        Point tmp1, tmp2;
        int clickCount;
        MouseButtonEventHandler handler = null;
        MouseEventHandler mouseMove = null;
        GeometryGroup group = new GeometryGroup();

        public Segment(MinutiaState state)
        {
            this.state = state;
            ConvertStateColorToBrush();

            tmp1 = new Point();
            tmp2 = new Point();
        }

        public void Draw(OverridedCanvas canvas, Image image, int index = -1)
        {
            handler += (ss, ee) =>
            {
                Path myPath = new Path();
                if (clickCount == 0)
                {
                    state.Id = getIdForMinutiae(canvas.Tag.ToString(), index);
                    tmp1 = ee.GetPosition(canvas);
                    state.Points[0] = tmp1;
                    var linetmp = new LineGeometry();
                    group.Children.Add(linetmp);
                    drawCompleteLine(ee, canvas, clickCount);

                    myPath.Stroke = color;
                    myPath.StrokeThickness = state.Minutia.Thickness;
                    myPath.Data = group;
                    myPath.Tag = state.Minutia.Name;
                    myPath.Uid = state.Id.ToString();

                    DeleteEmptyAtIndex(canvas, index);
                    AddEmptyToOpositeSite(canvas, index);

                    canvas.AddLogicalChild(myPath, index);
                    clickCount++;
                }
                else
                {
                    var linetmp = new LineGeometry();
                    group.Children.Add(linetmp);
                    drawCompleteLine(ee, canvas, clickCount);
                    myPath.Stroke = color;
                    myPath.StrokeThickness = state.Minutia.Thickness;
                    myPath.Data = group;
                    myPath.Tag = state.Minutia.Name;
                    myPath.Uid = state.Id.ToString();

                    deleteAndAdd(canvas, myPath, index);
                    clickCount = 0;
                    AddElementToSaveList(canvas.Tag.ToString(), index);
                    group = null;
                    group = new GeometryGroup();
                    index = -1;
                }

            };

            mouseMove += (ss, ee) =>
            {
                if (clickCount > 0 && clickCount < 3)
                {
                    drawCompleteLine(ee, canvas, clickCount);
                }
            };
            image.MouseMove += mouseMove;
            image.MouseRightButtonDown += handler;
            canvas.MouseRightButtonDown += handler;
        }
        private void drawCompleteLine(MouseEventArgs ee, OverridedCanvas canvas, int clickCount)
        {
            if (clickCount == 1)
            {
                tmp2 = ee.GetPosition(canvas);
                state.Points[1] = tmp2;
                ((LineGeometry)group.Children[clickCount - 1]).StartPoint = tmp1;
                ((LineGeometry)group.Children[clickCount - 1]).EndPoint = tmp2;
            }
        }

        public void DeleteEvent(Image image, OverridedCanvas canvas)
        {
            image.MouseRightButtonDown -= handler;
            image.MouseMove -= mouseMove;
            canvas.MouseRightButtonDown -= handler;
        }
        public override string ToString()
        {
            return state.Id + ";" + state.Minutia.Name + ";" + Math.Floor(state.Points[0].X).ToString() + ";" + Math.Floor(state.Points[0].Y).ToString() + ";" + Math.Floor(state.Points[1].X).ToString() + ";" + Math.Floor(state.Points[1].Y).ToString();
        }
        public void deleteAndAdd(OverridedCanvas canvas, Path myPath, int index = -1)
        {
            if (index == -1)
            {
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
            }
            else
            {
                canvas.Children.RemoveAt(index);
            }

            canvas.AddLogicalChild(myPath, index);
        }

        public void DrawFromFile(OverridedCanvas canvas)
        {
            Path myPath = new Path();
            LineGeometry myFirstPathFigure = new LineGeometry();

            myFirstPathFigure.StartPoint = state.Points[0];
            myFirstPathFigure.EndPoint = state.Points[1];
            group.Children.Add(myFirstPathFigure);

            myPath.Stroke = color;
            myPath.StrokeThickness = state.Minutia.Thickness;
            myPath.Data = group;
            myPath.Tag = state.Minutia.Name;
            myPath.Uid = state.Id.ToString();
            canvas.AddLogicalChild(myPath);
            canvas.Children[canvas.Children.Count - 1].Opacity = 0.5;
        }

        public string ToJson()
        {
            JObject minutiaeJson = new JObject();
            minutiaeJson["id"] = state.Id;
            minutiaeJson["name"] = state.Minutia.Name;
            minutiaeJson["points"] = new JArray()
            {
                state.Points[0].ToJObject(),
                state.Points[1].ToJObject()
            };

            return minutiaeJson.ToString();
        }
    }
}
