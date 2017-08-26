using Fingerprints.Models;
using Fingerprints.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints.MinutiaeTypes.Peak
{
    class UserPeak : Peak, IDraw
    {
        MouseButtonEventHandler handler = null;
        MouseEventHandler mouseMove = null;
        int clickCount;
        public UserPeak(MinutiaState state) : base(state)
        {
            state.Points = new List<Point>();
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
                    state.Points.ReplaceOrAddOnLastIndex(0, tmp1);
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
                    myPath.Stroke = color;
                    myPath.StrokeThickness = state.Minutia.Thickness;
                    myPath.Data = group;
                    myPath.Tag = state.Minutia.Name;
                    myPath.Uid = state.Id.ToString();
                    deleteAndAdd(canvas, myPath, index);
                    AddElementToSaveList(canvas.Tag.ToString(), index);
                    group = null;
                    group = new GeometryGroup();
                    clickCount = 0;
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

        public void Stop(Image image, OverridedCanvas canvas)
        {
            image.MouseRightButtonDown -= handler;
            image.MouseMove -= mouseMove;
            canvas.MouseRightButtonDown -= handler;
        }

        private void drawCompleteLine(MouseEventArgs ee, Canvas canvas, int clickCount)
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
    }
}
