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
        public static BrushConverter converter = new System.Windows.Media.BrushConverter();
        BorderColor borderColor;
        Dictionary<string, Brush> kolory = new Dictionary<string, Brush>();
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
            SetColors();


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
        }

        public void InitTable()
        {
            Table table = new Table();
            table.FillTableR(canvasImageR, imageR, listBoxImageR, comboBox);
            table.SelectedObject(canvasImageR, listBoxImageR, canvasImageL);
            table.FillTableL(canvasImageL, imageL, listBoxImageL, comboBox);
            table.SelectedObject(canvasImageL, listBoxImageL, canvasImageR);

            button.Click += (ss, ee) =>
            {
                if (listBoxImageL.Items.Count != 0)
                {
                    int indexL = listBoxImageL.SelectedIndex;
                    if (indexL >= 0)
                    {
                        try
                        {
                            
                            if (listBoxImageR.Items[indexL] != null)
                            {
                                table.UpdateCount(canvasImageL, canvasImageR);
                                listBoxImageL.UnselectAll();
                                listBoxImageR.UnselectAll();
                                listBoxImageL.Items.RemoveAt(indexL);
                                listBoxImageR.Items.RemoveAt(indexL);
                                canvasImageL.Children.RemoveAt(indexL);
                                canvasImageR.Children.RemoveAt(indexL);
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Do wyboru tej opcji potrzebne są 2 minucje");
                        }
                    }
                }
                
            };
        }

        public void comboBoxChanged()
        {
            handler += (ss, ee) =>
            {
                borderLeft.BorderBrush = Brushes.DeepSkyBlue;
                borderRight.BorderBrush = Brushes.Black;
                Brush kolor = (Brush)converter.ConvertFromString(minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.Color).FirstOrDefault());

                if (drawL != null && drawR != null)
                {
                    drawL.DeleteEvent(imageL);
                    drawR.DeleteEvent(imageR);
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
                    canvasImageR.Children.Add(myPath);
                    listBoxImageR.Items.Add("Puste");
                }

                if (minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.TypeId).First() == 2)
                {
                    double size = minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.Size).First();
                    drawL = new Vector(kolor, size);
                    drawR = new Vector(kolor, size);
                }
                if (minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.TypeId).First() == 1)
                {
                    double size = minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.Size).First();
                    drawL = new SinglePoint(kolor, size);
                    drawR = new SinglePoint(kolor, size);
                }
                if (minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.TypeId).First() == 3)
                {
                    drawL = new CurveLine(kolor);
                    drawR = new CurveLine(kolor);
                }
                drawL.Draw(canvasImageL, imageL, borderLeft, borderRight);
                drawR.Draw(canvasImageR, imageR, borderRight, borderLeft);
            };
            comboBox.SelectionChanged += handler;
        }

        public void SetColors()
        {
            kolory.Add("Czerwony", Brushes.Red);
            kolory.Add("Niebieski", Brushes.Blue);
            kolory.Add("Żółty", Brushes.Yellow);
            kolory.Add("Zielony", Brushes.Green);
            kolory.Add("Pomarańczowy", Brushes.Orange);
            kolory.Add("Fioletowy", Brushes.Purple);
            kolory.Add("Czarny", Brushes.Black);
            kolory.Add("Różowy", Brushes.Pink);
            kolory.Add("Jasno zielony", Brushes.LightGreen);
            kolory.Add("Jasno niebiesko", Brushes.LightBlue);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
