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
        MinutiaeTypeController controller;
        SelectionChangedEventHandler comboboxHandler = null;
        public DrawService drawer;
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;
            Picture picture = new Picture(this);
            drawer = new DrawService();
            picture.InitializeR();
            picture.InitializeL();
            controller = new MinutiaeTypeController();
            comboBox.ItemsSource = controller.Show();
            InitTable();
            saveEvents();
            comboBoxChanged();
            addEmpty.Click += addEmpty_Click;
        }

        public void comboBoxChanged()
        {
            comboboxHandler += (ss, ee) =>
            {
                if (comboBox.SelectedValue != null)
                {
                    drawer.startNewDrawing(comboBox.SelectedValue.ToString());
                }
            };

            comboBox.SelectionChanged += comboboxHandler;
        }

        public void setComboboxTitle(int index = -1)
        {
            comboBox.SelectionChanged -= comboboxHandler;
            comboBox.SelectedIndex = index;
            comboBox.SelectionChanged += comboboxHandler;
        }

        public void InitTable()
        {
            Table table = new Table();
        }

        private void wizardAdd_Click(object sender, RoutedEventArgs e)
        {
            Window1 win = new Window1();
            win.ShowDialog();
            drawer.stopDrawing();
            comboBox.ItemsSource = controller.Show();
        }

        public void saveEvents()
        {
            this.Closing += (ss, ee) =>
            {
                if (MessageBox.Show("Czy zapisać zmiany?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    FileTransfer.Save();
                }
            };

            saveButton.Click += (ss, ee) =>
            {
                FileTransfer.Save();
            };
        }
        private void addEmpty_Click(object sender, EventArgs e)
        {
            Empty empty = new Empty();
            if (canvasImageL.Children.Count > canvasImageR.Children.Count)
                empty.Draw(canvasImageR, imageR);
            else if (canvasImageL.Children.Count < canvasImageR.Children.Count)
                empty.Draw(this.canvasImageL, imageR, 0);
            else
                MessageBox.Show("Nie można dodać pustego, jeżeli mamy tyle samo elementów");
        }

        private void saveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "xyt|*.xyt";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        FileTransfer.ConvertToXytAndSave(saveFileDialog1.FileName);
                        break;
                }
            }
        }
    }
}