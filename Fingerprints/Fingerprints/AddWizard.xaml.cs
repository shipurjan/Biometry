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
        public Window1()
        {
            InitializeComponent();
            List<string> drawingType = new List<string>();
            List<string> colors = new List<string>();
            List<double> size = new List<double>();

            drawingType.Add("Punkt");
            drawingType.Add("Prosta skierowana");
            drawingType.Add("Krzywa dowolna");

            colors.Add("Czerwony");
            colors.Add("Zielony");
            colors.Add("Niebieski");
            colors.Add("Żółty");
            colors.Add("Pomarańczowy");
            colors.Add("Fioletowy");
            colors.Add("Czarny");
            colors.Add("Różowy");
            colors.Add("Jasno zielony");
            colors.Add("Jasno niebiesko");

            size.Add(0.1);
            size.Add(0.25);
            size.Add(0.5);
            size.Add(1);
            size.Add(2);            

            comboBoxType.ItemsSource = drawingType;
            comboBoxColor.ItemsSource = colors;
            comboBoxSize.ItemsSource = size;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                Database.AddNewMinutiae(textBox.Text, 1, comboBoxType.SelectedIndex + 1, comboBoxColor.SelectedValue.ToString(), Convert.ToDouble(comboBoxSize.SelectedItem));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
