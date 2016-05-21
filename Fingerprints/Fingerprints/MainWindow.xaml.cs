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
            Minutiae m1 = new Vector();
            IDraw draw = m;
            IDraw draw1 = m1;
            Picture p = new Picture(this);
            p.InitializeR();
            p.InitializeL();
            draw.Draw(canvasImageL, imageL);
            draw1.Draw(canvasImageR, imageR);

            button.Click += (ss, ee) =>
            {
                canvasImageL.Children.RemoveAt(canvasImageL.Children.Count - 1);
            };
        }
    }
}
