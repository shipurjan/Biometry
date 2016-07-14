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
            listBoxSelectionChanged(listBoxImageL, canvasImageL);
            listBoxSelectionChanged(listBoxImageR, canvasImageR);
            canvasChildAdded(canvasImageL, canvasImageR, listBoxImageL, listBoxImageR);
            canvasChildAdded(canvasImageR, canvasImageL, listBoxImageR, listBoxImageL);
        }

        private void canvasChildAdded(OverridedCanvas canvas, OverridedCanvas canvas2, ListBox listbox, ListBox listbox2)
        {
            canvas.ChildAdded += (ss, ee) =>
            {
                listbox.Items.Clear();
                double top = 0;
                int elementIndex = 0;  
                foreach (var item in canvas.Children)
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
                        if (listbox.Items.Count > index)
                        {
                            listbox.Items.RemoveAt(index);
                            canvas.Children.RemoveAt(index);
                            FileTransfer.ListL.RemoveAt(index);
                        }
                        if (listbox2.Items.Count > index)
                        {
                            listbox2.Items.RemoveAt(index);
                            canvas2.Children.RemoveAt(index);
                            FileTransfer.ListR.RemoveAt(index);
                        }
                        this.canvasDelete.Children.RemoveAt(index);
                    };
                    canvasDelete.Children.Add(button);
                    Canvas.SetTop(button, top);
                    top += 20;
                    elementIndex++;
                    if (item.GetType().Name == "Path")
                    {
                        Path q = (Path)item;
                        listbox.Items.Add(q.Tag);
                    }
                    else if (item.GetType().Name == "Polyline")
                    {
                        Polyline q = (Polyline)item;
                        listbox.Items.Add(q.Tag);
                    }
                }
            };
        }
        private void listBoxSelectionChanged(ListBox listBox, OverridedCanvas canvas)
        {
            listBox.SelectionChanged += (ss, ee) =>
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
            };
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
