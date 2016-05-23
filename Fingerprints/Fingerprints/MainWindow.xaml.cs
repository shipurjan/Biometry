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

            InitializeComponent();
            Picture p = new Picture(this);
            p.InitializeR();
            p.InitializeL();

            minType = new List<MinutiaeType>();
            MinutiaeTypeController controller = new MinutiaeTypeController();
            minType = controller.Show();
            comboBox.ItemsSource = minType;
            comboBoxChanged();

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
            comboBox.SelectionChanged += (ss, ee) =>
            {
                if (minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.DrawType).First() == 1)
                {
                    drawL = new Vector();
                    drawR = new Vector();
                }
                if (minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.DrawType).First() == 0)
                {
                    drawL = new SinglePoint();
                    drawR = new SinglePoint();
                }
                if (minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.DrawType).First() == 2)
                {
                    drawL = new CurveLine();
                    drawR = new CurveLine();
                }

                drawL.Draw(canvasImageL, imageL);
                drawR.Draw(canvasImageR, imageR);
            };
        }
    }
}
