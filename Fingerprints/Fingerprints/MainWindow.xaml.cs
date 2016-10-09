using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fingerprints
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public BrushConverter converter = new System.Windows.Media.BrushConverter();
        IDraw drawL;
        IDraw drawR;
        Helper helper;
        MinutiaeTypeController controller;
        SelectionChangedEventHandler handler = null;
        public MainWindow()
        {
            InitializeComponent();
            Picture p = new Picture(this);
            p.InitializeR();
            p.InitializeL();
            radioButtonEventInit();
            controller = new MinutiaeTypeController();
            helper = new Helper(this, controller);
            comboBox.ItemsSource = controller.Show();
            comboBoxChanged();
            InitTable();
            addEmpty.Click += addEmpty_Click;
            
            //Database.InitialData();
            this.Closed += (ss, ee) =>
            {
                FileTransfer.Save();
            };

            saveButton.Click += (ss, ee) =>
            {
                FileTransfer.Save();
            };
        }

        public void InitTable()
        {
            Table table = new Table(this);
        }

        public void comboBoxChanged()
        {
            handler += (ss, ee) =>
            {
                borderLeft.BorderBrush = Brushes.DeepSkyBlue;
                borderRight.BorderBrush = Brushes.Black;

                string selectedValue = "";
                if (comboBox.SelectedValue != null)
                    selectedValue = comboBox.SelectedValue.ToString();

                string kolor = controller.GetColorOfSelectedMinutiae(selectedValue);
                double thickness = controller.GetThicknessOfSelectedMinutiae(selectedValue);
                double size = controller.GetSizeOfSelectedMinutiae(selectedValue);

                if (drawL != null && drawR != null)
                {
                    drawL.DeleteEvent(imageL, canvasImageL);
                    drawR.DeleteEvent(imageR, canvasImageR);
                }
                if (listBoxImageL.Items.Count != listBoxImageR.Items.Count)
                {
                    Point singlePoint = new Point(1,1);                    
                    EllipseGeometry myEllipseGeometry = new EllipseGeometry();
                    myEllipseGeometry.Center = singlePoint;
                    myEllipseGeometry.RadiusX = 0;
                    myEllipseGeometry.RadiusY = 0;
                    Path myPath = new Path();
                    myPath.StrokeThickness = 0.3;
                    myPath.Data = myEllipseGeometry;
                    myPath.Opacity = 0;
                    myPath.Name = "Puste";
                    myPath.Tag = "Puste";
                    canvasImageR.Children.Add(myPath);
                    FileTransfer.ListR.Add("Puste");
                    listBoxImageR.Items.Add("Puste");
                }

                drawL = helper.GetMinutiaeTypeToDraw;
                drawR = helper.GetMinutiaeTypeToDraw;

                drawL.Draw(canvasImageL, imageL, activeCanvasL, activeCanvasR);
                drawR.Draw(canvasImageR, imageR, activeCanvasR, activeCanvasL);
            };
            comboBox.SelectionChanged += handler;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void wizardAdd_Click(object sender, RoutedEventArgs e)
        {
            Window1 win = new Window1();
            win.ShowDialog();
            comboBox.SelectionChanged -= handler;
            comboBox.ItemsSource = controller.Show();
            comboBoxChanged();
        }

        private void leftMenuClick_Delete(object sender, RoutedEventArgs e)
        {
            int index = listBoxImageL.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            listBoxImageL.Items.RemoveAt(index);
            canvasImageL.Children.RemoveAt(index);
            FileTransfer.ListL.RemoveAt(index);
        }
        private void rightMenuClick_Delete(object sender, RoutedEventArgs e)
        {
            int index = listBoxImageR.SelectedIndex;
            if (index == -1)
            {
                return;
            }

            listBoxImageR.Items.RemoveAt(index);
            canvasImageR.Children.RemoveAt(index);
            FileTransfer.ListR.RemoveAt(index);
        }

        void activeCanvasL_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.IsChecked == true)
                {
                    borderRight.BorderBrush = Brushes.Black;
                    borderLeft.BorderBrush = Brushes.DeepSkyBlue;
                }
            }
        }
        void activeCanvasR_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.IsChecked == true)
                {
                    borderLeft.BorderBrush = Brushes.Black;
                    borderRight.BorderBrush = Brushes.DeepSkyBlue;
                }
            }
        }
        private void addEmpty_Click(object sender, EventArgs e)
        {
            Empty empty = new Empty();
            if(activeCanvasL.IsChecked == true)
            {
                empty.Draw(canvasImageL, imageL, activeCanvasL, activeCanvasR);
                FileTransfer.ListL.Add("Puste");
            }
            else
            {
                empty.Draw(canvasImageR, imageR, activeCanvasR, activeCanvasL);
                FileTransfer.ListR.Add("Puste");
            }
            
        }
        private void radioButtonEventInit()
        {
            activeCanvasL.Checked += activeCanvasL_CheckedChanged;
            activeCanvasR.Checked += activeCanvasR_CheckedChanged;
        }
    }
}
