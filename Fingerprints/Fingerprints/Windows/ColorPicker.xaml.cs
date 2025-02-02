﻿using ExceptionLogger;
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
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : Window
    {
        public ColorPicker()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window1.colorPicked = colorPicker.SelectedColor.ToString();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Wybierz kolor");
            }
        }
    }
}
