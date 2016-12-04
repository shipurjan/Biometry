using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Fingerprints.Resources;

namespace Fingerprints
{
    class Table : AppInstance
    {
        OverridedCanvas canvasL, canvasR;
        ListBox listBoxL, listBoxR, listBoxD;
        ComboBox combobox;
        public Table()
        {
            this.combobox = window.comboBox;
            this.listBoxD = window.listBoxDelete;
            this.canvasL = window.canvasImageL;
            this.canvasR = window.canvasImageR;
            this.listBoxL = window.listBoxImageL;
            this.listBoxR = window.listBoxImageR;

            listBoxSelectionChanged(window.listBoxImageL, window.canvasImageL);
            listBoxSelectionChanged(window.listBoxImageR, window.canvasImageR);
            canvasLeftChildAdded();
            canvasRightChildAdded();
            deleteRowEvent();
            contextMenu();

        }

        public void deleteRowEvent()
        {
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
        }

        private void contextMenu()
        {
            List<SelfDefinedMinutiae> minType = new List<SelfDefinedMinutiae>();
            minType = new MinutiaeTypeController().Show();

            ContextMenu cmL = new ContextMenu();
            ContextMenu cmR = new ContextMenu();

            cmL.Items.Add(contextMenuLeftInsert(minType));
            cmL.Items.Add(deleteMenuContext(listBoxL, canvasL));

            cmR.Items.Add(contextMenuRightInsert(minType));
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
                    //window.comboBox.SelectedIndex = index; //TODO 
                    window.drawing.startRightDrawing(type.Name);
                    window.drawing.startLeftDrawing(type.Name, index);
                };
                mi.Items.Add(nMenu);
            }
            return mi;
        }

        private MenuItem contextMenuRightInsert(List<SelfDefinedMinutiae> minType)
        {
            MenuItem mi = new MenuItem() { Header = "Wstaw" };

            foreach (var type in minType)
            {
                MenuItem nMenu = new MenuItem() { Header = type.Name };
                nMenu.Click += (ss, ee) =>
                {
                    int index = listBoxR.SelectedIndex;
                    if (index == -1) { return; }
                    listBoxR.UnselectAll();
                    //window.comboBox.SelectedIndex = index; //TODO 
                    window.drawing.startLeftDrawing(type.Name);
                    window.drawing.startRightDrawing(type.Name, index);
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
            listBox.SelectionChangedEvent(() => SelectionChangedMethod(listBox, canvas));
        }

        private void SelectionChangedMethod(ListBox listBox, OverridedCanvas canvas)
        {
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

            if (listBox.Name.ToString() == "listBoxImageL")
            {
                leftChildIndex = listBox.SelectedIndex;

                drawRightIfEmpty();
            }
            else
            {
                rightChildIndex = listBox.SelectedIndex;

                drawLeftIfEmpty();
            }
        }

        private void drawRightIfEmpty()
        {
            if (leftChildIndex > -1 && window.listBoxImageR.Items.Count > leftChildIndex && window.listBoxImageR.Items[leftChildIndex].ToString() == "Puste")
            {
                window.drawing.startRightDrawing(window.listBoxImageL.Items[leftChildIndex].ToString(), leftChildIndex);
                window.drawing.startLeftDrawing(window.listBoxImageL.Items[leftChildIndex].ToString());
            }
        }

        private void drawLeftIfEmpty()
        {
            if(rightChildIndex > -1 && window.listBoxImageL.Items.Count > rightChildIndex && window.listBoxImageL.Items[rightChildIndex].ToString() == "Puste")
            {
                window.drawing.startLeftDrawing(window.listBoxImageR.Items[rightChildIndex].ToString(), rightChildIndex);
                window.drawing.startRightDrawing(window.listBoxImageR.Items[rightChildIndex].ToString());
            }
        }
    }
}
