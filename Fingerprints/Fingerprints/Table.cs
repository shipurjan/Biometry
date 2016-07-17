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
                listBoxL.Items.Clear();
                double top = 0;
                int elementIndex = 0;

                foreach (var item in canvasL.Children)
                {
                    Button button = new Button();
                    button.Height = 20;
                    button.Width = 30;
                    button.Background = Brushes.Aqua;
                    button.Tag = elementIndex;
                    button.Content = elementIndex;
                    button.Click += (s, e) =>
                    {
                        int index = Convert.ToInt16(button.Tag);
                        if (listBoxL.Items.Count > index)
                        {
                            listBoxL.Items.RemoveAt(index);
                            canvasL.Children.RemoveAt(index);
                            FileTransfer.ListL.RemoveAt(index);
                        }
                        if (listBoxR.Items.Count > index)
                        {
                            listBoxR.Items.RemoveAt(index);
                            canvasR.Children.RemoveAt(index);
                            FileTransfer.ListR.RemoveAt(index);
                        }
                    };
                    canvasD.Children.Add(button);
                    Canvas.SetTop(button, top);
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
                listBoxR.Items.Clear();
                double top = 0;
                int elementIndex = 0;

                foreach (var item in canvasR.Children)
                {
                    Button button = new Button();
                    button.Height = 20;
                    button.Width = 30;
                    button.Background = Brushes.Aqua;
                    button.Tag = elementIndex;
                    button.Content = elementIndex;
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
                        //this.canvasDelete.Children.RemoveAt(index);
                    };
                    canvasD.Children.Add(button);
                    Canvas.SetTop(button, top);
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

        private void rightClickMenu()
        {
            listBoxL.MouseRightButtonDown += (ss, ee) =>
            {
                Menu menu = new Menu();
            };
        }
    }
}
