using Fingerprints.Resources;
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
    public class Minutiae : AppInstance, IDraw
    {
        public string Name;
        public long id { get; set; }
        public Minutiae(long id = 0)
        {
            this.id = id;
        }
        public virtual void Draw(OverridedCanvas canvas, Image image, int index = -1) { }
        public virtual void DeleteEvent(Image image, OverridedCanvas canvas) { }
        public virtual void DrawFromFile(OverridedCanvas canvas) { }

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
                if (canvasType == "Left")
                {
                    return Convert.ToInt64(window.canvasImageR.Children[index].Uid);
                }
                else
                {
                    return Convert.ToInt64(window.canvasImageL.Children[index].Uid);
                }
            }
            else
            {
                if (canvasType == "Left")
                {
                    return getLastIdOfMinutiaeWithIsNotEmpty(FileTransfer.ListR, FileTransfer.ListL);
                }
                else
                {
                    return getLastIdOfMinutiaeWithIsNotEmpty(FileTransfer.ListL, FileTransfer.ListR);
                }
            }
            return UnixDate.GetCurrentUnixTimestampMillis();
        }

        public long getLastIdOfMinutiaeWithIsNotEmpty(List<string> list1, List<string> list2)
        {
            if (list1.Count == 0)
            {
                return UnixDate.GetCurrentUnixTimestampMillis();
            }

            for (int i = list1.Count - 1; i >= 0; i++)
            {
                string[] tmp1 = list1[i].Split(';');
                string[] tmp2 = list2[i].Split(';');
                if (tmp2[1] == tmp1[1])
                {
                    return UnixDate.GetCurrentUnixTimestampMillis();
                }
                else
                {
                    if (tmp1[1] == "Puste")
                    {
                        return UnixDate.GetCurrentUnixTimestampMillis();
                    }
                    return Convert.ToInt64(tmp1[0]);
                }
            }
            return UnixDate.GetCurrentUnixTimestampMillis();
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
            emptyR.Draw(window.canvasImageR, window.imageR);
            emptyL.Draw(window.canvasImageL, window.imageL);
        }

        protected void addEmptyLastLineIfIndexOnLastElement(int index)
        {
            if (index == -1)
                addEmptyOnLastLine();
        }

        public void AddEmptyToOpositeSite(OverridedCanvas canvas, int index)
        {
            if (index >=0 )
            {
                return;
            }

            Empty empty = new Empty();

            if (canvas == window.canvasImageL && CanvasCountEqual())
                empty.Draw(window.canvasImageR, window.imageR);
            else if (canvas == window.canvasImageR && CanvasCountEqual())
                empty.Draw(window.canvasImageL, window.imageL);
        }
        public bool CanvasCountEqual()
        {
            return (window.canvasImageL.Children.Count == window.canvasImageR.Children.Count);
        }
    }
}
