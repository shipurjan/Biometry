using Fingerprints.Tools.ImageFilters;
using Fingerprints.ViewModels;
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

namespace Fingerprints.Windows
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : UserControl
    {
        public FilterWindow()
        { }

        public FilterWindow(DrawingService _drawingService, FilterImageType _type)
        {
            var viewModel = new FilterWindowViewModel(_drawingService, _type);
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
