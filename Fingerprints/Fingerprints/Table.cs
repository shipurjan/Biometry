using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fingerprints
{
    class Table
    {
        int count = 0;
        MouseButtonEventHandler handler = null;
        public void FillTable(Canvas canvas, Image image ,ListBox listBox, ComboBox comboBox)
        {
            handler += (ss, ee) =>
            {
                if (count != canvas.Children.Count && canvas.Children.Count != 0)
                {
                    listBox.Items.Add(canvas.Children[canvas.Children.Count - 1].ToString());
                    count = canvas.Children.Count;
                }
            };
            image.MouseRightButtonUp += handler;
        }

        public void SelectedObject(Canvas canvas, ListBox listBox)
        {
            listBox.SelectionChanged += (ss, ee) =>
            {
                for (int i = 0; i < canvas.Children.Count; i++)
                {
                    canvas.Children[i].Opacity = 0.5;
                }
                if (listBox.SelectedIndex != -1)
                {
                    var element = canvas.Children[listBox.SelectedIndex];
                    element.Opacity = 1;
                }
            };
        }
    }
}
