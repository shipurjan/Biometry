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
        int countL = 0;
        int countR = 0;
        MouseButtonEventHandler handlerL = null;
        MouseButtonEventHandler handlerR = null;
        public void FillTableL(Canvas canvasL, Image imageL ,ListBox listBoxL, ComboBox comboBox)
        {
            handlerL += (ss, ee) =>
            {
                if (countL != canvasL.Children.Count && canvasL.Children.Count != 0)
                {
                    //listBox.Items.Add(canvas.Children[canvas.Children.Count - 1].ToString());
                    listBoxL.Items.Add(comboBox.SelectedItem.ToString());
                    countL = canvasL.Children.Count;
                }
            };
            imageL.MouseRightButtonUp += handlerL;
        }

        public void FillTableR(Canvas canvasR, Image imageR, ListBox listBoxR, ComboBox comboBox)
        {
            handlerR += (ss, ee) =>
            {
                if (countR != canvasR.Children.Count && canvasR.Children.Count != 0)
                {
                    //listBox.Items.Add(canvas.Children[canvas.Children.Count - 1].ToString());
                    listBoxR.Items.Add(comboBox.SelectedItem.ToString());
                    countR = canvasR.Children.Count;
                }
            };
            imageR.MouseRightButtonUp += handlerR;
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
