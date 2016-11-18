using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints
{
    class Helper
    {
        MainWindow mw;
        MinutiaeTypeController controller;
        public Helper(MainWindow mw, MinutiaeTypeController controller)
        {
            this.mw = mw;
            this.controller = controller;
        }

        public bool canInsertEmpty()
        {
            return mw.listBoxImageL.Items.Count != mw.listBoxImageR.Items.Count;
        }
        
        public void insertEmpty()
        {
            Empty empty = new Empty();
            if (mw.canvasImageL.Children.Count > mw.canvasImageR.Children.Count)
                empty.Draw(mw.canvasImageR, mw.imageR);
            else if (mw.canvasImageL.Children.Count < mw.canvasImageR.Children.Count)
                empty.Draw(mw.canvasImageL, mw.imageL);
        }
        public void deleteUnnecessaryEmpty()
        {
            while ((mw.listBoxImageL.Items.Count - 1 >= 0 && (string)mw.listBoxImageL.Items[mw.listBoxImageL.Items.Count - 1] == "Puste" && (string)mw.listBoxImageR.Items[mw.listBoxImageR.Items.Count - 1] == "Puste"))
            {
                int index = mw.listBoxImageL.Items.Count - 1;
                mw.listBoxImageL.Items.RemoveAt(index);
                mw.canvasImageL.Children.RemoveAt(index);
                if (FileTransfer.ListL.Count > mw.listBoxImageL.Items.Count)
                    FileTransfer.ListL.RemoveAt(mw.listBoxImageL.Items.Count);
                mw.listBoxImageR.Items.RemoveAt(index);
                mw.canvasImageR.Children.RemoveAt(index);
                if (FileTransfer.ListR.Count > mw.listBoxImageR.Items.Count)
                    FileTransfer.ListR.RemoveAt(mw.listBoxImageR.Items.Count);
            }
        }


        public IDraw GetMinutiaeTypeToDraw(string minutiaeName)
        {
            string kolor = controller.GetColorOfSelectedMinutiae(minutiaeName);
            double thickness = controller.GetThicknessOfSelectedMinutiae(minutiaeName);
            double size = controller.GetSizeOfSelectedMinutiae(minutiaeName);
            IDraw draw = null;
            if (controller.GetTypeIdOfSelectedMinutiae(minutiaeName) == 2)
            {
                draw = new Vector(minutiaeName, kolor, size, thickness);
            }
            if (controller.GetTypeIdOfSelectedMinutiae(minutiaeName) == 1)
            {
                draw = new SinglePoint(minutiaeName, kolor, size, thickness);
            }
            if (controller.GetTypeIdOfSelectedMinutiae(minutiaeName) == 3)
            {
                draw = new CurveLine(minutiaeName, kolor, thickness, null);
            }
            if (controller.GetTypeIdOfSelectedMinutiae(minutiaeName) == 4)
            {
                draw = new Triangle(minutiaeName, kolor, thickness);
            }
            if (controller.GetTypeIdOfSelectedMinutiae(minutiaeName) == 5)
            {
                draw = new Peak(minutiaeName, kolor, thickness);
            }
            return draw;
        }
    }
}
