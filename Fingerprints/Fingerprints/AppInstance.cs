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
        public MainWindow window { get; set; }
        public AppInstance()
        {
            window = (MainWindow)Application.Current.MainWindow;
        }

        public void deleteLeft(int index)
        {
            if (index == -1)
            {
                return;
            }

            window.listBoxImageL.Items.RemoveAt(index);
            window.canvasImageL.Children.RemoveAt(index);

            if (FileTransfer.ListL.Count > index)
                FileTransfer.ListL.RemoveAt(index);
        }

        public void deleteRight(int index)
        {
            if (index == -1)
            {
                return;
            }

            window.listBoxImageR.Items.RemoveAt(index);
            window.canvasImageR.Children.RemoveAt(index);

            if (FileTransfer.ListR.Count > index)
                FileTransfer.ListR.RemoveAt(index);
        }
    }
}
