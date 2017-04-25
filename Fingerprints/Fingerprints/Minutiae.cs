using Fingerprints.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Fingerprints
{
    public abstract class Minutiae : AppInstance
    {
        public string Name;
        public long id { get; set; }
        public Minutiae(long id = 0)
        {
            this.id = id;
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
                insertStringToList(FileTransfer.ListL, index);
            }
            else
            {
                insertStringToList(FileTransfer.ListR, index);
            }
        }

        private void insertStringToList(List<string> list, int index = -1)
        {
            if (index > -1)
            {
                list.Insert(index, ToString());
            }
            else
            {
                list.Add(ToString());
            }
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

            if (child1.Tag == child2.Tag)
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
            Empty emptyL = new Empty();
            Empty emptyR = new Empty();
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

            Empty empty = new Empty();

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
