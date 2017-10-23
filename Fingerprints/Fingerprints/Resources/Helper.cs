using Fingerprints.MinutiaeTypes;
using Fingerprints.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

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
            return (checkIfImageLEmpty() && checkIfImageREmpty() == true) ? true:false;
        }
        public bool checkCanvasChildrenCount()
        {
            return mw.listBoxImageL.Items.Count != mw.listBoxImageR.Items.Count;
        }

        private bool checkIfImageLEmpty()
        {
            return mw.imageL.Source != null ? true : false;
        }

        private bool checkIfImageREmpty()
        {
            return mw.imageR.Source != null ? true : false;
        }

        public void deleteUnnecessaryEmpty()
        {
            while ((mw.listBoxImageL.Items.Count - 1 >= 0 && ((MinutiaState)mw.listBoxImageL.Items[mw.listBoxImageL.Items.Count - 1]).MinutiaName == "Puste" && ((MinutiaState)mw.listBoxImageR.Items[mw.listBoxImageR.Items.Count - 1]).MinutiaName == "Puste"))
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
    }
}
