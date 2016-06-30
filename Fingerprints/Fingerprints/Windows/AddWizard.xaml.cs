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
using System.Windows.Shapes;

namespace Fingerprints
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public static string colorPicked;
        public Window1()
        {
            InitializeComponent();
            List<string> drawingType = new List<string>();
            List<string> colors = new List<string>();
            List<double> size = new List<double>();

            drawingType.Add("Punkt");
            drawingType.Add("Prosta skierowana");
            drawingType.Add("Krzywa dowolna");

            size.Add(0.1);
            size.Add(0.25);
            size.Add(0.5);
            size.Add(1);
            size.Add(2);            

            comboBoxType.ItemsSource = drawingType;
            comboBoxSize.ItemsSource = size; 
            buttonColorPicker.Click += (ss, ee) =>
            {
                ColorPicker colorPicker = new ColorPicker();
                colorPicker.ShowDialog();
                buttonColorPicker.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString(colorPicked);
            };
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                Database.AddNewMinutiae(textBox.Text, 1, comboBoxType.SelectedIndex + 1, colorPicked, Convert.ToDouble(comboBoxSize.SelectedItem));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
