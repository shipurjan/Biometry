using Fingerprints.Factories;
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

    }
}
