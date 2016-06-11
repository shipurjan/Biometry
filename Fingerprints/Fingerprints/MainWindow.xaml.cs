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
        Minutiae mL;
        Minutiae mR;
        IDraw drawL;
        IDraw drawR;
        List<MinutiaeType> minType;
        public MainWindow()
        {
            Table table = new Table();
            InitializeComponent();
            Picture p = new Picture(this);
            p.InitializeR();
            p.InitializeL();

            minType = new List<MinutiaeType>();
            MinutiaeTypeController controller = new MinutiaeTypeController();
            minType = controller.Show();
            comboBox.ItemsSource = minType;
            comboBoxChanged();
            table.FillTable(canvasImageL, imageL ,listBoxImageL, comboBox);
            table.SelectedObject(canvasImageL, listBoxImageL);
            //Database.InitialData();

            button.Click += (ss, ee) =>
            {
                if (canvasImageL.Children.Count > 0)
                {
                    canvasImageL.Children.RemoveAt(canvasImageL.Children.Count - 1);
                }
            };
        }

        public void comboBoxChanged()
        {
            Dictionary<string, Brush> kolory = new Dictionary<string, Brush>();
            kolory.Add("Czerwony", Brushes.Red);
            kolory.Add("Niebieski", Brushes.Blue);
            kolory.Add("Żółty", Brushes.Yellow);
            kolory.Add("Zielony", Brushes.Green);

            comboBox.SelectionChanged += (ss, ee) =>
            {

                if (minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.DrawType).First() == 1)
                {
                    var kolor = kolory[minType.Where(x => x.DrawType == 1 && x.Name == comboBox.SelectedValue.ToString()).Select(y => y.Color).First()];
                    drawL = new Vector(kolor);
                    drawR = new Vector(kolor);
                }
                if (minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.DrawType).First() == 0)
                {
                    var kolor = kolory[minType.Where(x => x.DrawType == 0 && x.Name == comboBox.SelectedValue.ToString()).Select(y => y.Color).First()];
                    drawL = new SinglePoint(kolor);
                    drawR = new SinglePoint(kolor);
                }
                if (minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.DrawType).First() == 2)
                {
                    var kolor = kolory[minType.Where(x => x.DrawType == 2 && x.Name == comboBox.SelectedValue.ToString()).Select(y => y.Color).First()];
                    drawL = new CurveLine(kolor);
                    drawR = new CurveLine(kolor);
                }

                drawL.Draw(canvasImageL, imageL);
                drawR.Draw(canvasImageR, imageR);
            };
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
