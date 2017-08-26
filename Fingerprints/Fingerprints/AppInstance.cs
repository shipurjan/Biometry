using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fingerprints
{
    public class AppInstance
    {
        public int leftChildIndex { get; set; }
        public int rightChildIndex { get; set; }
        public MainWindow mainWindow { get; set; }
        public AppInstance()
        {
            mainWindow = (MainWindow)Application.Current.MainWindow;
        }

        /// <summary>
        /// Delete drawing object at specific index from left elements (canvas, image, file)
        /// </summary>
        /// <param name="index"></param>
        public void deleteLeft(int index)
        {
            if (index == -1)
            {
                return;
            }

            mainWindow.listBoxImageL.Items.RemoveAt(index);
            mainWindow.canvasImageL.Children.RemoveAt(index);

            if (FileTransfer.ListL.Count > index)
                FileTransfer.ListL.RemoveAt(index);
        }

        /// <summary>
        /// Delete drawing object at specific index from right elements (canvas, image, file)
        /// </summary>
        /// <param name="index"></param>
        public void deleteRight(int index)
        {
            if (index == -1)
            {
                return;
            }

            mainWindow.listBoxImageR.Items.RemoveAt(index);
            mainWindow.canvasImageR.Children.RemoveAt(index);

            if (FileTransfer.ListR.Count > index)
                FileTransfer.ListR.RemoveAt(index);
        }

        /// <summary>
        /// Cast canvas child to Shape
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public System.Windows.Shapes.Shape castChildObject(UIElement child)
        {
            if (child.GetType().Name == "Path")
            {
                return (System.Windows.Shapes.Path)child;
            }
            else if(child.GetType().Name == "Line")
            {
                return (System.Windows.Shapes.Line)child;
            }
            else
            {
                return (System.Windows.Shapes.Polyline)child;
            }
        }
    }
}
