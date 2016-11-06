﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints
{
    class Table : Minutiae
    {
        OverridedCanvas canvasL, canvasR;
        ListBox listBoxL, listBoxR, listBoxD;
        Button buttonLeft, buttonRight;
        ComboBox combobox;
        public Table(MainWindow wm)
        {
            this.combobox = wm.comboBox;
            this.listBoxD = wm.listBoxDelete;
            this.canvasL = wm.canvasImageL;
            this.canvasR = wm.canvasImageR;
            this.listBoxL = wm.listBoxImageL;
            this.listBoxR = wm.listBoxImageR;
            this.buttonLeft = wm.buttonDeleteL;
            this.buttonRight = wm.buttonDeleteR;
            listBoxSelectionChanged(wm.listBoxImageL, wm.canvasImageL);
            listBoxSelectionChanged(wm.listBoxImageR, wm.canvasImageR);
            canvasLeftChildAdded();
            canvasRightChildAdded();
            listBoxD.SelectionChanged += (ss, ee) =>
            {
                if (listBoxD.SelectedIndex != -1)
                {
                    int index = listBoxD.SelectedIndex;
                    if (listBoxL.Items.Count > index)
                    {
                        deleteLeft(index);
                    }
                    if (listBoxR.Items.Count > index)
                    {
                        deleteRight(index);
                    }
                    listBoxD.Items.RemoveAt(index);
                }
            };

            contextMenu();

        }

        private void contextMenu()
        {
            List<SelfDefinedMinutiae> minType = new List<SelfDefinedMinutiae>();
            minType = new MinutiaeTypeController().Show();

            ContextMenu cmL = new ContextMenu();
            ContextMenu cmR = new ContextMenu();

            cmL.Items.Add(contextMenuLeftInsert(minType));
            cmL.Items.Add(deleteMenuContext(listBoxL, canvasL));

            cmR.Items.Add(deleteMenuContext(listBoxR, canvasR));

            listBoxR.ContextMenu = cmR;
            listBoxL.ContextMenu = cmL;
        }

        private MenuItem contextMenuLeftInsert(List<SelfDefinedMinutiae> minType)
        {
            MenuItem mi = new MenuItem() { Header = "Wstaw" };

            foreach (var type in minType)
            {
                MenuItem nMenu = new MenuItem() { Header = type.Name };
                nMenu.Click += (ss, ee) =>
                {
                    int index = listBoxL.SelectedIndex;
                    if (index == -1) { return; }
                    listBoxL.UnselectAll();
                    window.drawing.startNewDrawing(type.Name, index);
                };
                mi.Items.Add(nMenu);
            }
            return mi;
        }

        private MenuItem deleteMenuContext(ListBox listbox, OverridedCanvas canvas)
        {
            MenuItem usun = new MenuItem() { Header = "Usuń" };
            usun.Click += (ss, ee) =>
            {
                int index = listbox.SelectedIndex;
                if (index == -1)
                {
                    return;
                }
                listbox.Items.RemoveAt(index);
                canvas.Children.RemoveAt(index);
                if (canvas.Tag.ToString() == "Left")
                {
                    FileTransfer.ListL.RemoveAt(index);
                }
                else
                {
                    FileTransfer.ListR.RemoveAt(index);
                }
            };

            return usun;
        }

        private void addLeftAtIndex(int index, string name, UIElement child, string cords)
        {
            listBoxL.Items.Insert(index, name);
            canvasL.Children.Insert(index, child);
            FileTransfer.ListL.Insert(index, cords);
        }

        private void deleteLeft(int index)
        {
            listBoxL.Items.RemoveAt(index);
            canvasL.Children.RemoveAt(index);
            FileTransfer.ListL.RemoveAt(index);
        }

        private void deleteRight(int index)
        {
            listBoxR.Items.RemoveAt(index);
            canvasR.Children.RemoveAt(index);
            FileTransfer.ListR.RemoveAt(index);
        }

        private void canvasLeftChildAdded()
        {
            canvasL.ChildAdded += (ss, ee) =>
            {
                listBoxL.Items.Clear();

                foreach (var item in canvasL.Children)
                {
                    if (item.GetType().Name == "Path")
                    {
                        Path q = (Path)item;
                        listBoxL.Items.Add(q.Tag);
                    }
                    else if (item.GetType().Name == "Polyline")
                    {
                        Polyline q = (Polyline)item;
                        listBoxL.Items.Add(q.Tag);
                    }
                }
                deleteListRefresh();
            };
        }

        private void canvasRightChildAdded()
        {
            canvasR.ChildAdded += (ss, ee) =>
            {
                listBoxR.Items.Clear();
                foreach (var item in canvasR.Children)
                {
                    if (item.GetType().Name == "Path")
                    {
                        Path q = (Path)item;
                        listBoxR.Items.Add(q.Tag);
                    }
                    else if (item.GetType().Name == "Polyline")
                    {
                        Polyline q = (Polyline)item;
                        listBoxR.Items.Add(q.Tag);
                    }
                }
                deleteListRefresh();
            };
        }

        private void deleteListRefresh()
        {
            listBoxD.Items.Clear();

            int maxIndex = 0;
            if (listBoxL.Items.Count > listBoxR.Items.Count)
                maxIndex = listBoxL.Items.Count;
            else
                maxIndex = listBoxR.Items.Count;
            for (int i = 0; i < maxIndex; i++)
            {
                listBoxD.Items.Add("Usun");
            }
        }
        private void listBoxSelectionChanged(ListBox listBox, OverridedCanvas canvas)
        {
            listBox.SelectionChanged += (ss, ee) =>
            {
                if (canvas.Tag.ToString() == "Left")
                {
                    buttonLeft.Click += (s, e) =>
                    {
                        if (listBox.SelectedIndex != -1)
                        {
                            canvas.Children.RemoveAt(listBox.SelectedIndex);
                            FileTransfer.ListL.RemoveAt(listBox.SelectedIndex);
                            listBox.Items.RemoveAt(listBox.SelectedIndex);
                            deleteListRefresh();
                        }
                    };
                }
                else
                {
                    buttonRight.Click += (s, e) =>
                    {
                        if (listBox.SelectedIndex != -1)
                        {
                            canvas.Children.RemoveAt(listBox.SelectedIndex);
                            FileTransfer.ListR.RemoveAt(listBox.SelectedIndex);
                            listBox.Items.RemoveAt(listBox.SelectedIndex);
                            deleteListRefresh();
                        }
                    };
                }
                for (int i = 0; i < canvas.Children.Count; i++)
                {
                    if (canvas.Children[i] != null)
                    {
                        canvas.Children[i].Opacity = 0.5;
                    }
                }

                if (listBox.SelectedIndex != -1)
                {
                    canvas.Children[listBox.SelectedIndex].Opacity = 1;
                }
            };
        }
    }
}
