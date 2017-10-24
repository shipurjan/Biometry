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
using System.Windows.Data;

namespace Fingerprints
{
    class Table : AppInstance
    {
        OverridedCanvas canvasL, canvasR;
        ListBox listBoxL, listBoxR, listBoxD;
        ComboBox combobox;
        MinutiaeTypeController controller;
        List<MinutiaState> states;
        public Table()
        {
            controller = new MinutiaeTypeController();
            states = controller.getStates();

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

                    listBoxL.Items.Add(getListItemWithState(castChildObject(item).Tag.ToString()));
                    //listBoxL.Items.Add(castChildObject(item).Tag.ToString());
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
                    listBoxR.Items.Add((getListItemWithState(castChildObject(item).Tag.ToString())));
                    //listBoxR.Items.Add(castChildObject(item).Tag.ToString());
                }

                deleteListRefresh();
            };
        }

        private MinutiaState getListItemWithState(string name)
        {
            MinutiaState state = states
                .Where(x => x.Minutia.Name == name)
                .DefaultIfEmpty(new MinutiaState() { Id = 0, Minutia = new SelfDefinedMinutiae() { Name = "Puste" } })
                .First();

            //ListBoxItem listItem = new ListBoxItem();
            //listItem.Content = state;

            return state;
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

            
        }


        private bool clickedMinutiae(ListBox listboxPrimary, ListBox listboxSecondary)
        {
            if (listboxPrimary.SelectedIndex < 0 || listboxSecondary.Items.Count == 0)
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

       
    }
}
