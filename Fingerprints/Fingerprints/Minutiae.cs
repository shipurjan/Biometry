using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Fingerprints
{
    public class Minutiae : AppInstance, IDraw
    {
        public string Name;
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
        public void DeleteEmptyAtIndex(OverridedCanvas canvas, int index)
        {
            if (canvas.Children.Count == 0)
            {
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
                canvas.Children.RemoveAt(index);
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
