﻿using Microsoft.Win32;
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
        BorderColor borderColor;
        Dictionary<string, Brush> kolory = new Dictionary<string, Brush>();
        Minutiae mL;
        Minutiae mR;
        IDraw drawL;
        IDraw drawR;
        List<MinutiaeType> minType;
        public MainWindow()
        {
            InitializeComponent();
            SetColors();
            borderColor = new BorderColor() { BorderLeftColor = Brushes.Black, BorderRightColor = Brushes.Black };
            borderLeft.DataContext = borderColor;
            borderRight.DataContext = borderColor;

            Picture p = new Picture(this);
            p.InitializeR();
            p.InitializeL();

            minType = new List<MinutiaeType>();
            MinutiaeTypeController controller = new MinutiaeTypeController();
            minType = controller.Show();
            comboBox.ItemsSource = minType;
            comboBoxChanged();
            InitTable();

            //Database.InitialData();
        }

        public void InitTable()
        {
            button.Click += (ss, ee) =>
            {
                if (listBoxImageL.Items.Count != 0)
                {
                    int indexL = listBoxImageL.SelectedIndex;
                    if (indexL >= 0)
                    {
                        listBoxImageL.UnselectAll();
                        listBoxImageR.UnselectAll();
                        listBoxImageL.Items.RemoveAt(indexL);
                        listBoxImageR.Items.RemoveAt(indexL);
                        canvasImageL.Children.RemoveAt(indexL);
                        canvasImageR.Children.RemoveAt(indexL);
                    }
                }
            };

            Table table = new Table();
            table.FillTableR(canvasImageR, imageR, listBoxImageR, comboBox);
            table.SelectedObject(canvasImageR, listBoxImageR, canvasImageL);
            table.FillTableL(canvasImageL, imageL, listBoxImageL, comboBox);
            table.SelectedObject(canvasImageL, listBoxImageL, canvasImageR);
        }

        public void comboBoxChanged()
        {
            comboBox.SelectionChanged += (ss, ee) =>
            {
                if (minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.DrawType).First() == 1)
                {
                    var kolor = kolory[minType.Where(x => x.DrawType == 1 && x.Name == comboBox.SelectedValue.ToString()).Select(y => y.Color).First()];
                    drawL = new Vector(kolor, 1.2);
                    drawR = new Vector(kolor, 1.2);
                }
                if (minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.DrawType).First() == 0)
                {
                    var kolor = kolory[minType.Where(x => x.DrawType == 0 && x.Name == comboBox.SelectedValue.ToString()).Select(y => y.Color).First()];
                    drawL = new SinglePoint(kolor);
                    drawR = new SinglePoint(kolor);
                }
                if (minType.Where(x => x.Name == comboBox.SelectedValue.ToString()).Select(y => y.DrawType).First() == 2)
                {
                    var kolor = kolory[minType.Where(x => x.DrawType == 2 && x.Name == comboBox.SelectedValue.ToString()).Select(y => y.Color).First()];
                    drawL = new CurveLine(kolor);
                    drawR = new CurveLine(kolor);
                }
                drawL.Draw(canvasImageL, imageL, borderLeft);
                drawR.Draw(canvasImageR, imageR, borderRight);
            };
        }

        public void SetColors()
        {
            kolory.Add("Czerwony", Brushes.Red);
            kolory.Add("Niebieski", Brushes.Blue);
            kolory.Add("Żółty", Brushes.Yellow);
            kolory.Add("Zielony", Brushes.Green);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            borderColor.BorderLeftColor = Brushes.Cyan;
            borderColor.BorderRightColor = Brushes.Cyan;
        }
    }
}
