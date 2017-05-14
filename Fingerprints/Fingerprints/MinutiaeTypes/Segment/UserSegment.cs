using Fingerprints.Models;
using Fingerprints.Resources;
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

namespace Fingerprints.MinutiaeTypes.Segment
{
    class UserSegment : Segment, IDraw
    {
        Point tmp1, tmp2;
        int clickCount;
        MouseButtonEventHandler handler = null;
        MouseEventHandler mouseMove = null;
        GeometryGroup group = new GeometryGroup();

        public UserSegment(MinutiaState state) : base(state)
        {
            state.Points = new List<Point>();
            tmp1 = new Point();
            tmp2 = new Point();
        }

        public void Draw(OverridedCanvas canvas, Image image, int index = -1)
        {
            AddHandler(canvas, image, index);
            image.MouseRightButtonDown += handler;
            image.MouseMove += mouseMove;
        }

        public void Stop(Image image, OverridedCanvas canvas)
        {
            image.MouseRightButtonDown -= handler;
            image.MouseMove -= mouseMove;
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

            mouseMove += (ss, ee) =>
            {
                if (clickCount > 0 && clickCount < 3)
                {
                    drawCompleteLine(ee, canvas, clickCount);
                }
            };
            
        }

        private void AddToCanvas(object ss, MouseButtonEventArgs ee, OverridedCanvas canvas, Image image, int index)
        {
            Path myPath = new Path();
            if (clickCount == 0)
            {
                state.Id = getIdForMinutiae(canvas.Tag.ToString(), index);
                tmp1 = ee.GetPosition(canvas).ToFloorPoint();
                state.Points.ReplaceOrAddOnLastIndex(0, tmp1);
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
        }
    }
}
