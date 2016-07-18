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
        OverridedCanvas canvasL, canvasR, canvasD;
        ListBox listBoxL, listBoxR;
        Button buttonLeft, buttonRight;
        public Table(OverridedCanvas canvasImageL, OverridedCanvas canvasImageR, ListBox listBoxImageL, ListBox listBoxImageR, OverridedCanvas canvasDelete, Button buttonDeleteLeft, Button buttonDeleteRight)
        {
            this.canvasL = canvasImageL;
            this.canvasR = canvasImageR;
            this.canvasD = canvasDelete;
            this.listBoxL = listBoxImageL;
            this.listBoxR = listBoxImageR;
            this.buttonLeft = buttonDeleteLeft;
            this.buttonRight = buttonDeleteRight;
            listBoxSelectionChanged(listBoxImageL, canvasImageL);
            listBoxSelectionChanged(listBoxImageR, canvasImageR);
            canvasLeftChildAdded();
            canvasRightChildAdded();
        }

        private void canvasLeftChildAdded()
        {
            canvasL.ChildAdded += (ss, ee) =>
            {
                canvasD.Children.Clear();
                listBoxL.Items.Clear();
                double top = 0;
                int elementIndex = 0;

                foreach (var item in canvasL.Children)
                {
                    buttonConfiguration(elementIndex, top);
                    top += 20;
                    elementIndex++;
                    
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
            };
        }

        private void canvasRightChildAdded()
        {
            canvasR.ChildAdded += (ss, ee) =>
            {
                canvasD.Children.Clear();
                listBoxR.Items.Clear();
                double top = 0;
                int elementIndex = 0;

                foreach (var item in canvasR.Children)
                {
                    buttonConfiguration(elementIndex, top);
                    top += 20;
                    elementIndex++;
                    
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
            };
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
                canvasD.Children.RemoveAt(index);
            };
            canvasD.AddLogicalChild(button);
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
