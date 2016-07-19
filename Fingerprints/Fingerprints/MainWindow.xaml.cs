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
using FontAwesome.WPF;

namespace Fingerprints
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //pack://application:,,,/fonts/#FontAwesome
        public BrushConverter converter = new System.Windows.Media.BrushConverter();
        BorderColor borderColor;
        Minutiae mL;
        Minutiae mR;
        IDraw drawL;
        IDraw drawR;
        List<SelfDefinedMinutiae> minType;
        MinutiaeTypeController controller;
        SelectionChangedEventHandler handler = null;
        public MainWindow()
        {
            
            InitializeComponent();
            buttonDeleteL.Content = FontAwesomeIcon.Trash;
            Picture p = new Picture(this);
            p.InitializeR();
            p.InitializeL();

            minType = new List<SelfDefinedMinutiae>();
            controller = new MinutiaeTypeController();
            minType = controller.Show();
            comboBox.ItemsSource = minType;
            comboBoxChanged();
            InitTable();
            //Database.InitialData();
            this.Closed += (ss, ee) =>
            {
                FileTransfer.Save();
            };
        }

        public void InitTable()
        {
            Table table = new Table(canvasImageL, canvasImageR, listBoxImageL, listBoxImageR, listBoxDelete, buttonDeleteL, buttonDeleteR);

        }

        public void comboBoxChanged()
        {
            handler += (ss, ee) =>
            {
                borderLeft.BorderBrush = Brushes.DeepSkyBlue;
                borderRight.BorderBrush = Brushes.Black;
                string kolor = minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.Color).FirstOrDefault();

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

                if (minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.TypeId).First() == 2)
                {
                    double size = minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.Size).First();
                    drawL = new Vector(comboBox.SelectedValue.ToString() ,kolor, size);
                    drawR = new Vector(comboBox.SelectedValue.ToString(), kolor, size);
                }
                if (minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.TypeId).First() == 1)
                {
                    double size = minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.Size).First();
                    drawL = new SinglePoint(comboBox.SelectedValue.ToString(), kolor, size);
                    drawR = new SinglePoint(comboBox.SelectedValue.ToString(), kolor, size);
                }
                if (minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.TypeId).First() == 3)
                {
                    drawL = new CurveLine(kolor);
                    drawR = new CurveLine(kolor);
                }
                if (minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.TypeId).First() == 4)
                {
                    double size = minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.Size).First();
                    drawL = new Triangle(comboBox.SelectedValue.ToString(), kolor);
                    drawR = new Triangle(comboBox.SelectedValue.ToString(), kolor);
                }
                if (minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.TypeId).First() == 5)
                {
                    double size = minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.Size).First();
                    drawL = new Peak(comboBox.SelectedValue.ToString(), kolor);
                    drawR = new Peak(comboBox.SelectedValue.ToString(), kolor);
                }

                drawL.Draw(canvasImageL, imageL, borderLeft, borderRight);
                drawR.Draw(canvasImageR, imageR, borderRight, borderLeft);
            };
            comboBox.SelectionChanged += handler;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(listBoxDelete.Items.Count.ToString());
        }

        private void wizardAdd_Click(object sender, RoutedEventArgs e)
        {
            Window1 win = new Window1();
            win.ShowDialog();
            comboBox.SelectionChanged -= handler;
            minType = controller.Show();
            comboBox.ItemsSource = minType;
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
            listBoxImageL.UnselectAll();
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

    }
}
