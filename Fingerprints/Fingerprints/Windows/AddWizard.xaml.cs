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
            try
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

                DrawingType.ItemsSource = drawingType;

                Color.Background = Brushes.Red;
                Color.Click += (ss, ee) =>
                {
                    ColorPicker colorPicker = new ColorPicker();
                    colorPicker.Owner = this;
                    colorPicker.ShowDialog();

                    if (colorPicked != "")
                    {
                        Color.Background = (Brush)new BrushConverter().ConvertFromString(colorPicked);
                    }
                };
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listBoxRefresh()
        {
            try
            {
                listBox.ItemsSource = Database.ShowSelfDefinedMinutiae();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }            
        }

        private void DialogHost_OnDialogClosing(Object sender, DialogClosingEventArgs eventArgs)
        {
            try
            {
                if ((bool)eventArgs.Parameter)
                {
                    if (IsValidationCorrent())
                    {
                        definedMinutiae.Add(Database.AddNewMinutiae(DefinedMinutiaName.Text, (DrawingType)DrawingType.SelectedIndex + 1, Color.Background));

                        DefinedMinutiaName.Text = "";
                        DrawingType.SelectedIndex = -1;
                        HideValidationErrors();
                    }
                    else
                    {
                        eventArgs.Cancel();
                    }
                }
                else
                {
                    HideValidationErrors();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void HideValidationErrors()
        {
            try
            {
                DefinedMinutiaNameValidationError.Visibility = Visibility.Collapsed;
                DrawingTypeValidationError.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private bool IsValidationCorrent()
        {
            bool result = true;
            try
            {
                HideValidationErrors();

                if (string.IsNullOrWhiteSpace(DefinedMinutiaName.Text))
                {
                    result = false;
                    DefinedMinutiaNameValidationError.Visibility = Visibility.Visible;
                    DefinedMinutiaNameValidationError.Text = "Nazwa jest wymagana";
                }

                if (definedMinutiae.FirstOrDefault(x => x.Name.ToLower() == DefinedMinutiaName.Text.ToLower()) != null)
                {
                    result = false;
                    DefinedMinutiaNameValidationError.Visibility = Visibility.Visible;
                    DefinedMinutiaNameValidationError.Text = "Taka nazwa już istnieje";
                }

                if (DrawingType.SelectedValue == null)
                {
                    result = false;
                    DrawingTypeValidationError.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
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
