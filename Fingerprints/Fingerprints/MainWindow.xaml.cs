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
        public MainWindow()
        {
            InitializeComponent();
            Minutiae m = new Vector();
            Picture p = new Picture(this);
            p.InitializeL(); // inicjalizacja panelu LEFT
            p.InitializeR();
            m.Draw(canvasImageL, imageL);
            
            Vector v = new Vector();
                

        }

    }
}
