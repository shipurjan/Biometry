using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fingerprints
{
    class Picture
    {
        Point firstPoint = new Point();
        MainWindow mw;
        public Picture(MainWindow mw)
        {
            this.mw = mw;
        }

        public void InitializeL()
        {
            Init(mw.canvasImageL, mw.imageL, mw.openLeftImage);
        }
        public void InitializeR()
        {
            Init(mw.canvasImageR, mw.imageR, mw.openRightImage);
        }

        private void Init(Canvas canvasImage, Image image, Button button)
        {
            button.Click += (ss, ee) =>
            {
                OpenFileDialog openFile = new OpenFileDialog();
                if (openFile.ShowDialog() == true)
                {
                    image.Source = new BitmapImage(new Uri(openFile.FileName));
                    if (image.Tag.ToString() == "Left")
                    { 
                        FileTransfer.LeftImagePath = System.IO.Path.ChangeExtension(openFile.FileName, ".txt");
                        FileTransfer.LoadLeftFile();
                    }
                    else
                    {
                        FileTransfer.RightImagePath = System.IO.Path.ChangeExtension(openFile.FileName, ".txt");
                        FileTransfer.LoadRightFile();
                    }
                }
                Canvas.SetTop(canvasImage, Canvas.GetTop(image));
                Canvas.SetLeft(canvasImage, Canvas.GetLeft(image));
            };


            image.MouseLeftButtonDown += (ss, ee) =>
            {
                firstPoint = ee.GetPosition(mw);
                image.CaptureMouse();
            };

            image.MouseWheel += (ss, ee) =>
            {
                Matrix matline = canvasImage.RenderTransform.Value;
                Matrix mat = image.RenderTransform.Value;
                Point mouse = ee.GetPosition(image);

                if (ee.Delta > 0)
                {
                    mat.ScaleAtPrepend(1.15, 1.15, mouse.X, mouse.Y);
                    matline.ScaleAtPrepend(1.15, 1.15, mouse.X, mouse.Y);
                }
                else
                {
                    mat.ScaleAtPrepend(1 / 1.15, 1 / 1.15, mouse.X, mouse.Y);
                    matline.ScaleAtPrepend(1 / 1.15, 1 / 1.15, mouse.X, mouse.Y);
                }

                MatrixTransform mtf = new MatrixTransform(mat);
                image.RenderTransform = mtf;
                MatrixTransform mtfl = new MatrixTransform(matline);
                canvasImage.RenderTransform = mtfl;
            };

            image.MouseMove += (ss, ee) =>
            {
                if (ee.LeftButton == MouseButtonState.Pressed)
                {
                    Point temp = ee.GetPosition(mw);
                    Point res = new Point(firstPoint.X - temp.X, firstPoint.Y - temp.Y);

                    Canvas.SetLeft(image, Canvas.GetLeft(image) - res.X);
                    Canvas.SetTop(image, Canvas.GetTop(image) - res.Y);
                    Canvas.SetLeft(canvasImage, Canvas.GetLeft(image) - res.X);
                    Canvas.SetTop(canvasImage, Canvas.GetTop(image) - res.Y);
                    firstPoint = temp;
                }
            };
            image.MouseUp += (ss, ee) => { image.ReleaseMouseCapture(); };
        }

        private void loadMinutiae(List<string> list)
        {
            List<SelfDefinedMinutiae> minutiaeList = new MinutiaeTypeController().Show();
            foreach (var item in list)
            {
                string[] tmp = item.Split(';');

            }
        }

    }
}
