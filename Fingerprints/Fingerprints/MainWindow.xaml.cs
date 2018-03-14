using Fingerprints.Factories;
using Fingerprints.Models;
using Fingerprints.ViewModels;
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
        //public DrawService drawer;
        public MainWindow()
        {
            Application.Current.MainWindow = this;
            var viewModel = new MainWindowViewModel();
            InitializeComponent();
            DataContext = viewModel;
        }

        private void addEmpty_Click(object sender, RoutedEventArgs e)
        {
            MinutiaDataGrid.Items.Refresh();
        }
    }
}