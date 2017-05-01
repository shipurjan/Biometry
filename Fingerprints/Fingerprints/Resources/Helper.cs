﻿using Fingerprints.Models;
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
        public void insertEmpty()
        {
            Empty empty = new Empty();
            if (mw.canvasImageL.Children.Count > mw.canvasImageR.Children.Count)
                empty.Draw(mw.canvasImageR, mw.imageR);
            else if (mw.canvasImageL.Children.Count < mw.canvasImageR.Children.Count)
                empty.Draw(mw.canvasImageL, mw.imageL);
            else return;
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

        public void addEmptyOnLastLine()
        {
            Empty emptyL = new Empty();
            Empty emptyR = new Empty();
            emptyR.Draw(mw.canvasImageR, mw.imageR);
            emptyL.Draw(mw.canvasImageL, mw.imageL);
        }

        public IDraw GetMinutiaeTypeToDraw(string minutiaeName)
        {
            MinutiaState state = new MinutiaState
            {
                Minutia = controller.GetMinutia(minutiaeName),
                Points = new List<Point>()
            };
            IDraw draw = null;

            switch (state.Minutia.TypeId)
            {
                case 1:
                    draw = new SinglePoint(state);
                    break;
                case 2:
                    draw = new Vector(state);
                    break;
                case 3:
                    draw = new CurveLine(state);
                    break;
                case 4:
                    draw = new Triangle(state);
                    break;
                case 5:
                    draw = new Peak(state);
                    break;
                case 6:
                    draw = new Segment(state);
                    break;
            }
            return draw;
        }
    }
}
