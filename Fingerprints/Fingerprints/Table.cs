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
using Fingerprints.Models;
using Fingerprints.MinutiaeTypes.Empty;

namespace Fingerprints
{
    class Table : AppInstance
    {
        OverridedCanvas canvasL, canvasR;
        ListBox listBoxL, listBoxR, listBoxD;
        ComboBox combobox;
        public Table()
        {
            this.combobox = mainWindow.comboBox;
            this.listBoxD = mainWindow.listBoxDelete;
            this.canvasL = mainWindow.canvasImageL;
            this.canvasR = mainWindow.canvasImageR;
            this.listBoxL = mainWindow.listBoxImageL;
            this.listBoxR = mainWindow.listBoxImageR;

            listBoxSelectionChanged(mainWindow.listBoxImageL, mainWindow.canvasImageL);
            listBoxSelectionChanged(mainWindow.listBoxImageR, mainWindow.canvasImageR);
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
            List<MinutiaState> minType = new List<MinutiaState>();
            minType = new MinutiaeTypeController().getStates();

            ContextMenu cmL = new ContextMenu();
            ContextMenu cmR = new ContextMenu();

            cmL.Items.Add(contextMenuLeftInsert(minType));
            cmL.Items.Add(deleteMenuContext(listBoxL, canvasL));

            cmR.Items.Add(contextMenuRightInsert(minType));
            cmR.Items.Add(deleteMenuContext(listBoxR, canvasR));

            listBoxR.ContextMenu = cmR;
            listBoxL.ContextMenu = cmL;
        }

        private MenuItem contextMenuLeftInsert(List<MinutiaState> minType)
        {
            MenuItem mi = new MenuItem() { Header = "Wstaw" };

            foreach (var type in minType)
            {
                MenuItem nMenu = new MenuItem() { Header = type.Minutia.Name };
                nMenu.Click += (ss, ee) =>
                {
                    int index = listBoxL.SelectedIndex;
                    if (index == -1) { return; }
                    listBoxL.UnselectAll();
                    mainWindow.setComboboxTitle(minType.FindIndex(a => a.Minutia.Name == type.Minutia.Name));
                    mainWindow.drawer.startRightDrawing(type);
                    mainWindow.drawer.startLeftDrawing(type, index);
                };
                mi.Items.Add(nMenu);
            }
            return mi;
        }

        private MenuItem contextMenuRightInsert(List<MinutiaState> minType)
        {
            MenuItem mi = new MenuItem() { Header = "Wstaw" };

            foreach (var type in minType)
            {
                MenuItem nMenu = new MenuItem() { Header = type.Minutia.Name };
                nMenu.Click += (ss, ee) =>
                {
                    int index = listBoxR.SelectedIndex;
                    if (index == -1) { return; }
                    listBoxR.UnselectAll();
                    mainWindow.setComboboxTitle(minType.FindIndex(a => a.Minutia.Name == type.Minutia.Name));
                    mainWindow.drawer.startLeftDrawing(type);
                    mainWindow.drawer.startRightDrawing(type, index);
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
                UserEmpty empty = new UserEmpty(new MinutiaState());
                if (canvas.Tag.ToString() == "Left")
                {
                    FileTransfer.ListL.RemoveAt(index);

                    empty.Draw(mainWindow.canvasImageL, mainWindow.imageL, index);
                }
                else
                {
                    FileTransfer.ListR.RemoveAt(index);

                    empty.Draw(mainWindow.canvasImageR, mainWindow.imageR, index);
                }

            };

            return usun;
        }

        private void canvasLeftChildAdded()
        {
            canvasL.ChildAdded += (ss, ee) =>
            {
                listBoxL.Items.Clear();
                foreach (UIElement item in canvasL.Children)
                {
                    listBoxL.Items.Add(castChildObject(item).Tag);
                }

                deleteListRefresh();
            };
        }

        private void canvasRightChildAdded()
        {
            canvasR.ChildAdded += (ss, ee) =>
            {
                listBoxR.Items.Clear();
                foreach (UIElement item in canvasR.Children)
                {
                    listBoxR.Items.Add(castChildObject(item).Tag);
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

                drawByLeftTableClick();
            }
            else
            {
                rightChildIndex = listBox.SelectedIndex;

                drawByRightTableClick();
            }
        }

        private void drawByRightTableClick()
        {
            if (clickedEmpty(listBoxR, listBoxL))
            {
                clickOnEmptyObject("Right");
            }
            else if (clickedMinutiae(listBoxR, listBoxL))
            {
                clickOnMinutiaeObject("Right");
            }
        }

        private void drawByLeftTableClick()
        {
            if (clickedEmpty(listBoxL, listBoxR))
            {
                clickOnEmptyObject("Left");
            }
            else if (clickedMinutiae(listBoxL, listBoxR))
            {
                clickOnMinutiaeObject("Left");
            }
        }

        private bool clickedMinutiae(ListBox listboxPrimary, ListBox listboxSecondary)
        {
            if (listboxPrimary.SelectedIndex < 0)
            {
                return false;
            }
            else if (listboxPrimary.SelectedItem.ToString() == "Puste")
            {
                return false;
            }
            else if (listboxSecondary.Items[listboxPrimary.SelectedIndex].ToString() != "Puste")
            {
                return false;
            }
            return true;
        }

        private bool clickedEmpty(ListBox listboxPrimary, ListBox listboxSecondary)
        {
            if (listboxPrimary.SelectedIndex < 0)
            {
                return false;
            }
            else if (listboxPrimary.SelectedItem.ToString() != "Puste")
            {
                return false;
            }
            else if (listboxSecondary.Items[listboxPrimary.SelectedIndex].ToString() == "Puste")
            {
                return false;
            }

            return true;
        }

        private void clickOnMinutiaeObject(string side)
        {
            if (side == "Left")
            {
                mainWindow.drawer.startRightDrawing(mainWindow.listBoxImageL.SelectedItem as MinutiaState, mainWindow.listBoxImageL.SelectedIndex);
                mainWindow.drawer.startLeftDrawing(mainWindow.listBoxImageL.SelectedItem as MinutiaState);
            }
            else
            {
                mainWindow.drawer.startLeftDrawing(mainWindow.listBoxImageR.SelectedItem as MinutiaState, mainWindow.listBoxImageR.SelectedIndex);
                mainWindow.drawer.startRightDrawing(mainWindow.listBoxImageR.SelectedItem as MinutiaState);
            }
        }
        private void clickOnEmptyObject(string side)
        {
            if (side == "Left")
            {
                mainWindow.drawer.startLeftDrawing(mainWindow.listBoxImageR.Items[mainWindow.listBoxImageL.SelectedIndex] as MinutiaState, mainWindow.listBoxImageL.SelectedIndex);
                mainWindow.drawer.startRightDrawing(mainWindow.listBoxImageR.Items[mainWindow.listBoxImageL.SelectedIndex] as MinutiaState);
            }
            else
            {
                mainWindow.drawer.startRightDrawing(mainWindow.listBoxImageL.Items[mainWindow.listBoxImageR.SelectedIndex] as MinutiaState, mainWindow.listBoxImageR.SelectedIndex);
                mainWindow.drawer.startLeftDrawing(mainWindow.listBoxImageL.Items[mainWindow.listBoxImageR.SelectedIndex] as MinutiaState);
            }
        }
    }
}
