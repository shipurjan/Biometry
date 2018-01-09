using ExceptionLogger;
using Fingerprints.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<SelfDefinedMinutiae> definedMinutiae;

        public static string colorPicked;
        public Window1()
        {
            colorPicked = "";
            definedMinutiae = new ObservableCollection<SelfDefinedMinutiae>(Database.ShowSelfDefinedMinutiae());

            InitializeComponent();
            listBox.ItemsSource = definedMinutiae;
            
            List<string> drawingType = new List<string>();

            drawingType.Add("Punkt");
            drawingType.Add("Prosta skierowana");
            drawingType.Add("Krzywa dowolna");
            drawingType.Add("Trojkat");
            drawingType.Add("Daszek");
            drawingType.Add("Odcinek");
            
            comboBoxType.ItemsSource = drawingType;

            buttonColorPicker.Click += (ss, ee) =>
            {
                ColorPicker colorPicker = new ColorPicker();
                colorPicker.Owner = this;
                colorPicker.ShowDialog();

                if (colorPicked != "")
                {
                    buttonColorPicker.Background = (Brush)new BrushConverter().ConvertFromString(colorPicked);
                }
            };
            //listBoxRefresh();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                Database.AddNewMinutiae(textBox.Text, (DrawingType)comboBoxType.SelectedIndex + 1);
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Uzupełnij dane");
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listBoxRefresh()
        {
            listBox.ItemsSource = Database.ShowSelfDefinedMinutiae();
        }

        private void DialogHost_OnDialogClosing(Object sender, DialogClosingEventArgs eventArgs)
        {

        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PackIcon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SelfDefinedMinutiae definedMinutia = null;
            try
            {
                if (listBox.SelectedValue != null && MessageBox.Show("Czy na pewno chcesz usunąć zdefiniowany typ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    definedMinutia = listBox.SelectedValue as SelfDefinedMinutiae;
                    Database.DeleteMinutiae(definedMinutia);
                    definedMinutiae.Remove(definedMinutia);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
