using Fingerprints.MinutiaeTypes.Empty;
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
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints
{
    public abstract class Minutiae : AppInstance
    {
        public MinutiaState state;
        public Brush color;

        public Minutiae(MinutiaState state)
        {
            this.state = new MinutiaState()
            {
                Minutia = state.Minutia,
                Points = new List<Point>()
            };
            ConvertStateColorToBrush();
        }
        protected void ConvertStateColorToBrush()
        {
            if (state.Minutia != null && state.Minutia.Color != null)
            {
                this.color = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString(state.Minutia.Color);
            }
        }
        public void deleteChildWithGivenIndex(string canvasType, int index)
        {
            if (canvasType == "Left")
            {
                deleteLeft(index);
            }
            else
            {
                deleteRight(index);
            }
        }

        public void AddElementToSaveList(string listType, int index = -1)
        {
            if (listType == "Left")
            {
                insertMinutiaStateToFile(FileTransfer.ListL, index);
            }
            else
            {
                insertMinutiaStateToFile(FileTransfer.ListR, index);
            }
        }

        private void insertMinutiaStateToFile(List<MinutiaState> list, int index = -1)
        {
            if (index > -1)
            {
                list.Insert(index, state);
            }
            else
            {
                list.Add(state);
            }

            this.state = new MinutiaState()
            {
                Minutia = this.state.Minutia,
                Points = new List<Point>()
            };
        }

        public long getIdForMinutiae(string canvasType, int index)
        {
            if (index > -1)
            {
                return getIdOfSpecificMinutiae(canvasType, index);
            }
            else
            {
                return getIdOfLastMinutiae(canvasType);
            }
        }

        public long getIdOfLastMinutiae(string canvasType)
        {
            if (canvasType == "Left")
            {
                return getLastIdOfMinutiaeWithIsNotEmpty(mainWindow.canvasImageR, mainWindow.canvasImageL);
            }
            else
            {
                return getLastIdOfMinutiaeWithIsNotEmpty(mainWindow.canvasImageL, mainWindow.canvasImageR);
            }
        }

        public long getIdOfSpecificMinutiae(string canvasType, int index)
        {
            if (canvasType == "Left")
            {
                return mainWindow.canvasImageR.Children[index].Uid != "" ? Convert.ToInt64(mainWindow.canvasImageR.Children[index].Uid) : UnixDate.GetCurrentUnixTimestampMillis();
            }
            else
            {
                return mainWindow.canvasImageL.Children[index].Uid != "" ? Convert.ToInt64(mainWindow.canvasImageL.Children[index].Uid) : UnixDate.GetCurrentUnixTimestampMillis();
            }
        }

        public long getLastIdOfMinutiaeWithIsNotEmpty(OverridedCanvas canvas1, OverridedCanvas canvas2)
        {
            if (canvas1.Children.Count == 0)
            {
                return UnixDate.GetCurrentUnixTimestampMillis();
            }

            Shape child1 = castChildObject(canvas1.Children[mainWindow.canvasImageL.Children.Count - 1]);
            Shape child2 = castChildObject(canvas2.Children[mainWindow.canvasImageR.Children.Count - 1]);

            if (child1.Tag.ToString() == child2.Tag.ToString())
            {
                return UnixDate.GetCurrentUnixTimestampMillis();
            }
            else if (child1.Tag.ToString() == "Puste")
            {
                return UnixDate.GetCurrentUnixTimestampMillis();
            }
            else
            {
                return Convert.ToInt64(child1.Uid);
            }
        }

        public void DeleteEmptyAtIndex(OverridedCanvas canvas, int index)
        {
            if (canvas.Children.Count == 0)
            {
                return;
            }

            if (index > -1)
            {
                deleteChildWithGivenIndex(canvas.Tag.ToString(), index);
                return;
            }

            if (index == -1)
                index = canvas.Children.Count - 1;

            string name = canvas.Children[index].GetType().Name;
            string tag = "";
            if (name == "Path")
            {
                System.Windows.Shapes.Path q = (System.Windows.Shapes.Path)canvas.Children[index];
                tag = q.Tag.ToString();
            }
            if (tag == "Puste")
            {
                deleteChildWithGivenIndex(canvas.Tag.ToString(), index);
            }
        }
        private void addEmptyOnLastLine()
        {
            UserEmpty emptyL = new UserEmpty(new MinutiaState());
            UserEmpty emptyR = new UserEmpty(new MinutiaState());
            emptyR.Draw(mainWindow.canvasImageR, mainWindow.imageR);
            emptyL.Draw(mainWindow.canvasImageL, mainWindow.imageL);
        }

        protected void addEmptyLastLineIfIndexOnLastElement(int index)
        {
            if (index == -1)
                addEmptyOnLastLine();
        }

        public void AddEmptyToOpositeSite(OverridedCanvas canvas, int index)
        {
            if (index >= 0)
            {
                return;
            }

            UserEmpty empty = new UserEmpty(new MinutiaState());

            if (canvas == mainWindow.canvasImageL && CanvasCountEqual())
                empty.Draw(mainWindow.canvasImageR, mainWindow.imageR);
            else if (canvas == mainWindow.canvasImageR && CanvasCountEqual())
                empty.Draw(mainWindow.canvasImageL, mainWindow.imageL);
        }
        public bool CanvasCountEqual()
        {
            return (mainWindow.canvasImageL.Children.Count == mainWindow.canvasImageR.Children.Count);
        }
    }
}
