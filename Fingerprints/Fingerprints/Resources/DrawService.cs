using Fingerprints.Factories;
using Fingerprints.MinutiaeTypes.Empty;
using Fingerprints.Models;
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
    public class DrawService : AppInstance
    {
        IDraw drawL;
        IDraw drawR;
        Helper helper;
        MinutiaeTypeController controller;
        MinutiaFactory factory;
        public DrawService()
        {
            controller = new MinutiaeTypeController();
            helper = new Helper(mainWindow, controller);
        }

        public DrawService(MinutiaFactory factory)
        {
            this.factory = factory;
        }

        public void startNewDrawing(MinutiaState state, int index = -1)
        {
            startLeftDrawing(state, index);
            startRightDrawing(state, index);
        }

        public void startLeftDrawing(MinutiaState state, int index = -1)
        {
            stopLeftDrawing();
            //drawL = helper.GetMinutiaeTypeToDraw(minutiaeName);
            drawL = factory.Create(state);
            drawL.Draw(mainWindow.canvasImageL, mainWindow.imageL, index);
        }
        public void startRightDrawing(MinutiaState state, int index = -1)
        {
            stopRightDrawing();
            //drawR = helper.GetMinutiaeTypeToDraw(minutiaeName);
            drawR = factory.Create(state);
            drawR.Draw(mainWindow.canvasImageR, mainWindow.imageR, index);
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
                drawL.Stop(mainWindow.imageL, mainWindow.canvasImageL);
            }
        }

        public void stopRightDrawing()
        {
            if (drawR != null)
            {
                drawR.Stop(mainWindow.imageR, mainWindow.canvasImageR);
            }
        }

        public bool canStopDrawing()
        {
            return drawL != null && drawR != null;
        }

        public bool canInsertEmpty()
        {
            return mainWindow.listBoxImageL.Items.Count != mainWindow.listBoxImageR.Items.Count;
        }

        private void insertEmpty()
        {
            UserEmpty empty = new UserEmpty(new MinutiaState());

            if (mainWindow.canvasImageL.Children.Count > mainWindow.canvasImageR.Children.Count)
                empty.Draw(mainWindow.canvasImageR, mainWindow.imageR);
            else if (mainWindow.canvasImageL.Children.Count < mainWindow.canvasImageR.Children.Count)
                empty.Draw(mainWindow.canvasImageL, mainWindow.imageR);
        }
        private void deleteUnnecessaryEmpty()
        {
            while (((string)mainWindow.listBoxImageL.Items[mainWindow.listBoxImageL.Items.Count - 1] == "Puste" && (string)mainWindow.listBoxImageR.Items[mainWindow.listBoxImageR.Items.Count - 1] == "Puste"))
            {
                mainWindow.listBoxImageL.Items.RemoveAt(mainWindow.listBoxImageL.Items.Count - 1);
                mainWindow.canvasImageL.Children.RemoveAt(mainWindow.listBoxImageL.Items.Count - 1);

                if (FileTransfer.ListL.Count > mainWindow.listBoxImageL.Items.Count)
                    FileTransfer.ListL.RemoveAt(mainWindow.listBoxImageL.Items.Count);

                mainWindow.listBoxImageR.Items.RemoveAt(mainWindow.listBoxImageR.Items.Count - 1);
                mainWindow.canvasImageR.Children.RemoveAt(mainWindow.listBoxImageR.Items.Count - 1);

                if (FileTransfer.ListR.Count > mainWindow.listBoxImageR.Items.Count)
                    FileTransfer.ListR.RemoveAt(mainWindow.listBoxImageR.Items.Count);
            }
        }
    }
}
