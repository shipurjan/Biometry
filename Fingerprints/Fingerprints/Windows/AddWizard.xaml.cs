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
            colorPicked = "";
            InitializeComponent();
            List<string> drawingType = new List<string>();
            List<string> colors = new List<string>();
            List<double> size = new List<double>();
            drawingType.Add("Punkt");
            drawingType.Add("Prosta skierowana");
            drawingType.Add("Krzywa dowolna");
            drawingType.Add("Trojkat");
            drawingType.Add("Daszek");
            size.Add(0.1);
            size.Add(0.25);
            size.Add(0.5);
            size.Add(1);
            size.Add(2);
            thicknessCombobox.Items.Add(0.3);
            thicknessCombobox.Items.Add(0.5);
            thicknessCombobox.Items.Add(0.7);
            thicknessCombobox.Items.Add(1);
            thicknessCombobox.Items.Add(1.3);
            thicknessCombobox.Items.Add(1.5);

            comboBoxType.ItemsSource = drawingType;
            comboBoxSize.ItemsSource = size; 
            buttonColorPicker.Click += (ss, ee) =>
            {
                ColorPicker colorPicker = new ColorPicker();
                colorPicker.ShowDialog();

                if (colorPicked != "")
                {
                    buttonColorPicker.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString(colorPicked);
                }
            };
            listBoxRefresh();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                Database.AddNewMinutiae(textBox.Text, comboBoxType.SelectedIndex + 1, colorPicked, Convert.ToDouble(comboBoxSize.SelectedItem), Convert.ToDouble(thicknessCombobox.SelectedItem));
                this.Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Uzupełnij dane");
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedValue != null)
            {
                Database.DeleteMinutiae(listBox.SelectedValue as SelfDefinedMinutiae);
                listBoxRefresh();
            }
        }

        private void listBoxRefresh()
        {
            listBox.ItemsSource = Database.ShowSelfDefinedMinutiae();
        }
    }
}
