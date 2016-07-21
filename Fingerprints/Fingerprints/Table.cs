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

namespace Fingerprints
{
    class Table
    {
        OverridedCanvas canvasL, canvasR;
        ListBox listBoxL, listBoxR, listBoxD;
        Button buttonLeft, buttonRight;
        Border borderLeft, borderRight;
        public Table(OverridedCanvas canvasImageL, OverridedCanvas canvasImageR, ListBox listBoxImageL, ListBox listBoxImageR, ListBox listboxDelete, Button buttonDeleteLeft, Button buttonDeleteRight)
        {
            this.borderLeft = borderLeft;
            this.borderRight = borderRight;
            this.listBoxD = listboxDelete;
            this.canvasL = canvasImageL;
            this.canvasR = canvasImageR;
            this.listBoxL = listBoxImageL;
            this.listBoxR = listBoxImageR;
            this.buttonLeft = buttonDeleteLeft;
            this.buttonRight = buttonDeleteRight;
            listBoxSelectionChanged(listBoxImageL, canvasImageL);
            listBoxSelectionChanged(listBoxImageR, canvasImageR);
            canvasLeftChildAdded();
            canvasRightChildAdded();
            listBoxD.SelectionChanged += (ss, ee) =>
            {
                if (listBoxD.SelectedIndex != -1)
                {
                    int index = listBoxD.SelectedIndex;
                    if (listBoxL.Items.Count > index )
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
            //MenuItem mi = new MenuItem() { Header = "Wstaw" };
            //MenuItem nMenu = new MenuItem() { Header = "Second" };
            //mi.Items.Add(nMenu);
            ContextMenu cm = new ContextMenu();

            cm.Items.Add(contextMenuLeft(minType));
            listBoxL.ContextMenu = cm;
        }

        private MenuItem contextMenuLeft(List<SelfDefinedMinutiae> list)
        {
            
            MenuItem mi = new MenuItem() { Header = "Wstaw" };

            foreach (var item in list)
            {
                MenuItem nMenu = new MenuItem() { Header = item.Name };
                nMenu.Click += (ss, ee) =>
                {
                    int index = listBoxL.SelectedIndex;
                    if (index == -1)  {return; }
                    listBoxL.UnselectAll();
                };
                mi.Items.Add(nMenu);
            }

            return mi;
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

        private void buttonConfiguration(int elementIndex, double top)
        {
            Button button = new Button();
            button.Height = 20;
            button.Width = 30;
            button.Background = Brushes.CadetBlue;
            button.Tag = elementIndex;
            button.Click += (s, e) =>
            {
                int index = Convert.ToInt16(button.Tag);
                if (listBoxR.Items.Count > index)
                {
                    listBoxR.Items.RemoveAt(index);
                    canvasR.Children.RemoveAt(index);
                    FileTransfer.ListR.RemoveAt(index);
                }
                if (listBoxL.Items.Count > index)
                {
                    listBoxL.Items.RemoveAt(index);
                    canvasL.Children.RemoveAt(index);
                    FileTransfer.ListL.RemoveAt(index);
                }
            };
            Canvas.SetTop(button, top);
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
