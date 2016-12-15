using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints
{
    public class DrawingEventHandler : Minutiae
    {
        IDraw drawL;
        IDraw drawR;
        Helper helper;
        MinutiaeTypeController controller;
        public DrawingEventHandler()
        {
            controller = new MinutiaeTypeController();
            helper = new Helper(window, controller);
        }

        public void startNewDrawing(string minutiaeName, int index = -1)
        {
            startLeftDrawing(minutiaeName, index);
            startRightDrawing(minutiaeName, index);
        }

        public void startLeftDrawing(string minutiaeName, int index = -1)
        {
            stopLeftDrawing();
            drawL = helper.GetMinutiaeTypeToDraw(minutiaeName);
            drawL.Draw(window.canvasImageL, window.imageL, index);
        }
        public void startRightDrawing(string minutiaeName, int index = -1)
        {
            stopRightDrawing();
            drawR = helper.GetMinutiaeTypeToDraw(minutiaeName);
            drawR.Draw(window.canvasImageR, window.imageR, index);
        }

        public void stopDrawing()
        {
            stopLeftDrawing();
            stopRightDrawing();
        }

        public void stopLeftDrawing()
        {
            if (drawL != null)
            {
                drawL.DeleteEvent(window.imageL, window.canvasImageL);
            }
        }

        public void stopRightDrawing()
        {
            if (drawR != null)
            {
                drawR.DeleteEvent(window.imageR, window.canvasImageR);
            }
        }

        public bool canStopDrawing()
        {
            return drawL != null && drawR != null;
        }

        public bool canInsertEmpty()
        {
            return window.listBoxImageL.Items.Count != window.listBoxImageR.Items.Count;
        }

        private void insertEmpty()
        {
            Empty empty = new Empty();

            if (window.canvasImageL.Children.Count > window.canvasImageR.Children.Count)
                empty.Draw(window.canvasImageR, window.imageR);
            else if (window.canvasImageL.Children.Count < window.canvasImageR.Children.Count)
                empty.Draw(window.canvasImageL, window.imageR);
        }
        private void deleteUnnecessaryEmpty()
        {
            while (((string)window.listBoxImageL.Items[window.listBoxImageL.Items.Count - 1] == "Puste" && (string)window.listBoxImageR.Items[window.listBoxImageR.Items.Count - 1] == "Puste"))
            {
                window.listBoxImageL.Items.RemoveAt(window.listBoxImageL.Items.Count - 1);
                window.canvasImageL.Children.RemoveAt(window.listBoxImageL.Items.Count - 1);

                if (FileTransfer.ListL.Count > window.listBoxImageL.Items.Count)
                    FileTransfer.ListL.RemoveAt(window.listBoxImageL.Items.Count);

                window.listBoxImageR.Items.RemoveAt(window.listBoxImageR.Items.Count - 1);
                window.canvasImageR.Children.RemoveAt(window.listBoxImageR.Items.Count - 1);

                if (FileTransfer.ListR.Count > window.listBoxImageR.Items.Count)
                    FileTransfer.ListR.RemoveAt(window.listBoxImageR.Items.Count);
            }
        }
    }
}
