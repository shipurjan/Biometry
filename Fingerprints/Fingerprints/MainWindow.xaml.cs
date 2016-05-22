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
        public MainWindow()
        {

            //var dict = new Dictionary<string, Action>();
            //dict.Add("Półprosta skierowana", () => { mL = new Vector(); mR = new Vector(); });
            //dict.Add("Por", () => { mL = new SinglePoint(); mR = new SinglePoint(); });
            //dict.Add("Krzywa", () => { mL = new CurveLine(); mR = new CurveLine(); });

            InitializeComponent();
            Picture p = new Picture(this);
            p.InitializeR();
            p.InitializeL();
            MinutiaeTypes types = new MinutiaeTypes();
            this.comboBox.ItemsSource = types.dic.Values;
            Database db = new Database();
            db.InsertData();
            button.Content = db.connection.State.ToString();

            comboBox.SelectionChanged += (ss, ee) =>
            {
                if (comboBox.SelectedValue.ToString() == types.dic[0].ToString())
                {
                    drawL = new Vector();
                    drawR = new Vector();
                }
                if (comboBox.SelectedValue.ToString() == types.dic[1].ToString())
                {
                    drawL = new SinglePoint();
                    drawR = new SinglePoint();
                }
                if (comboBox.SelectedValue.ToString() == types.dic[2].ToString())
                {
                    drawL = new CurveLine();
                    drawR = new CurveLine();
                }

                drawL.Draw(canvasImageL, imageL);
                drawR.Draw(canvasImageR, imageR);
            };

            button.Click += (ss, ee) =>
            {
                if (canvasImageL.Children.Count > 0)
                {
                    canvasImageL.Children.RemoveAt(canvasImageL.Children.Count - 1);
                }
            };
        }
    }
}
