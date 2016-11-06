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
            resetBordersAndRadioButtons();

            string kolor = controller.GetColorOfSelectedMinutiae(minutiaeName);
            double thickness = controller.GetThicknessOfSelectedMinutiae(minutiaeName);
            double size = controller.GetSizeOfSelectedMinutiae(minutiaeName);

            if (canStopDrawing())
            {
                stopDrawing();
            }
            if (canInsertEmpty())
            {
                insertEmpty();
            }

            drawL = helper.GetMinutiaeTypeToDraw(minutiaeName);
            drawR = helper.GetMinutiaeTypeToDraw(minutiaeName);

            drawL.Draw(window.canvasImageL, window.imageL, window.activeCanvasL, window.activeCanvasR, index);
            drawR.Draw(window.canvasImageR, window.imageR, window.activeCanvasR, window.activeCanvasL, index);
        }

        public void stopDrawing()
        {
            drawL.DeleteEvent(window.imageL, window.canvasImageL);
            drawR.DeleteEvent(window.imageR, window.canvasImageR);
        }

        private void resetBordersAndRadioButtons()
        {
            window.activeCanvasL.IsChecked = true;
            window.borderRight.BorderBrush = Brushes.Black;
            window.borderLeft.BorderBrush = Brushes.DeepSkyBlue;
        }

        public bool canStopDrawing()
        {
            return drawL != null && drawR != null;
        }

        public bool canInsertEmpty()
        {
            //warunki do wstawienia pustego elementu
            return window.listBoxImageL.Items.Count != window.listBoxImageR.Items.Count;
        }

        private void insertEmpty()
        {
            Point singlePoint = new Point(1, 1);
            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = singlePoint;
            myEllipseGeometry.RadiusX = 0;
            myEllipseGeometry.RadiusY = 0;
            Path myPath = new Path();
            myPath.StrokeThickness = 0.3;
            myPath.Data = myEllipseGeometry;
            myPath.Opacity = 0;
            myPath.Name = "Puste";
            myPath.Tag = "Puste";
            window.canvasImageR.AddLogicalChild(myPath);
            FileTransfer.ListR.Add("Puste");
            window.listBoxImageR.Items.Add("Puste");
        }
    }
}
