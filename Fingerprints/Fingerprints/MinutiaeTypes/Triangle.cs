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
    class Triangle : Minutiae, IDraw
    {
        Point tmp1, tmp2;
        int clickCount;
        MouseButtonEventHandler handler = null;
        MouseEventHandler mouseMove = null;
        GeometryGroup group = new GeometryGroup();


        public Triangle(MinutiaState state)
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
                    tmp1 = ee.GetPosition(canvas).ToFloorPoint();
                    state.Points.Insert(0, tmp1);
                    var linetmp = new LineGeometry();

                    DeleteEmptyAtIndex(canvas, index);
                    AddEmptyToOpositeSite(canvas, index);

                    group.Children.Add(linetmp);
                    drawCompleteLine(ee, canvas, clickCount);

                    myPath.Stroke = color;
                    myPath.StrokeThickness = state.Minutia.Thickness;
                    myPath.Data = group;
                    myPath.Tag = state.Minutia.Name;
                    myPath.Uid = state.Id.ToString();

                    canvas.AddLogicalChild(myPath, index);
                    clickCount++;
                }
                else if (clickCount == 1)
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
                    clickCount++;
                }
                else if (clickCount == 2)
                {
                    var linetmp = new LineGeometry();
                    group.Children.Add(linetmp);
                    drawCompleteLine(ee, canvas, clickCount);
                    clickCount++;
                    drawCompleteLine(ee, canvas, clickCount);
                    myPath.Stroke = color;
                    myPath.StrokeThickness = state.Minutia.Thickness;
                    myPath.Data = group;
                    myPath.Tag = state.Minutia.Name;
                    myPath.Uid = state.Id.ToString();

                    deleteAndAdd(canvas, myPath, index);
                    clickCount = 0;
                    AddElementToSaveList(canvas.Tag.ToString(), index);
                    index = -1;
                    group = null;
                    group = new GeometryGroup();
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
        private void drawCompleteLine(MouseEventArgs ee, OverridedCanvas canvas, int clickCount)
        {
            if (clickCount == 1)
            {
                tmp2 = ee.GetPosition(canvas).ToFloorPoint();
                state.Points.ReplaceOrAddOnLastIndex(1, tmp2);
                ((LineGeometry)group.Children[clickCount - 1]).StartPoint = tmp1;
                ((LineGeometry)group.Children[clickCount - 1]).EndPoint = tmp2;
            }
            else if (clickCount == 2)
            {
                tmp2 = ee.GetPosition(canvas).ToFloorPoint();
                state.Points.ReplaceOrAddOnLastIndex(2, tmp2);
                ((LineGeometry)group.Children[clickCount - 1]).StartPoint = ((LineGeometry)group.Children[clickCount - 2]).EndPoint;
                ((LineGeometry)group.Children[clickCount - 1]).EndPoint = tmp2;
            }
            else if (clickCount == 3)
            {
                ((LineGeometry)group.Children[clickCount - 1]).StartPoint = ((LineGeometry)group.Children[clickCount - 2]).EndPoint;
                ((LineGeometry)group.Children[clickCount - 1]).EndPoint = ((LineGeometry)group.Children[0]).StartPoint;
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
            return state.Id + ";" + state.Minutia.Name + ";" + Math.Floor(state.Points[0].X).ToString() + ";" + Math.Floor(state.Points[0].Y).ToString() + ";" + Math.Floor(state.Points[1].X).ToString() + ";" + Math.Floor(state.Points[1].Y).ToString() + ";" + Math.Floor(state.Points[2].X).ToString() + ";" + Math.Floor(state.Points[2].Y).ToString();
        }

        public void DrawFromFile(OverridedCanvas canvas)
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

        public string ToJson()
        {
            JObject minutiaeJson = new JObject();
            minutiaeJson["id"] = state.Id;
            minutiaeJson["name"] = state.Minutia.Name;

            minutiaeJson["points"] = new JArray()
            {
                state.Points[0].ToJObject(),
                state.Points[1].ToJObject(),
                state.Points[2].ToJObject(),
            };

            return minutiaeJson.ToString();
        }
    }
}
