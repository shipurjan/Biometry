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

        private void Init(OverridedCanvas canvasImage, Image image, Button button)
        {
            button.Click += (ss, ee) =>
            {
                canvasImage.Children.Clear();
                OpenFileDialog openFile = new OpenFileDialog();
                if (openFile.ShowDialog() == true)
                {
                    image.Source = new BitmapImage(new Uri(openFile.FileName));
                    BitmapImage b = new BitmapImage(new Uri(openFile.FileName));
                    image.Height = b.PixelHeight;
                    image.Width = b.PixelWidth;
                    if (image.Tag.ToString() == "Left")
                    { 
                        FileTransfer.LeftImagePath = System.IO.Path.ChangeExtension(openFile.FileName, ".txt");
                        FileTransfer.LoadLeftFile();
                        canvasImage.Children.Clear();
                        loadMinutiae(FileTransfer.ListL, canvasImage);
                    }
                    else
                    {
                        FileTransfer.RightImagePath = System.IO.Path.ChangeExtension(openFile.FileName, ".txt");
                        FileTransfer.LoadRightFile();
                        canvasImage.Children.Clear();
                        loadMinutiae(FileTransfer.ListR, canvasImage);
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

        private void loadMinutiae(List<string> list, OverridedCanvas canvas)
        {
            List<SelfDefinedMinutiae> minutiaeList = new MinutiaeTypeController().Show();

            foreach (var item in list)
            {
                string[] tmp = item.Split(';');

                var type = minutiaeList.Where(x => x.Name == tmp[0]).FirstOrDefault();
                if (type.TypeId == 1)
                {
                    SinglePoint p = new SinglePoint(type.Name, type.Color, type.Size, Convert.ToDouble(tmp[1]), Convert.ToDouble(tmp[2]));
                    p.DrawFromFile(canvas);

                }
                else if (type.TypeId == 2)
                {
                    Vector v = new Vector(type.Name, type.Color, type.Size, Convert.ToDouble(tmp[1]), Convert.ToDouble(tmp[2]), Convert.ToDouble(tmp[3]));
                    v.DrawFromFile(canvas);
                }
                else if (type.TypeId == 3)
                {

                }
                else if (type.TypeId == 4)
                {
                    Triangle t = new Triangle(type.Name, type.Color, Convert.ToDouble(tmp[1]), Convert.ToDouble(tmp[2]), Convert.ToDouble(tmp[3]), Convert.ToDouble(tmp[4]), Convert.ToDouble(tmp[5]), Convert.ToDouble(tmp[6]));
                    t.DrawFromFile(canvas);
                }
                else if (type.TypeId == 5)
                {
                    Peak p = new Peak(type.Name, type.Color, Convert.ToDouble(tmp[1]), Convert.ToDouble(tmp[2]), Convert.ToDouble(tmp[3]), Convert.ToDouble(tmp[4]), Convert.ToDouble(tmp[5]), Convert.ToDouble(tmp[6]));
                    p.DrawFromFile(canvas);
                }
                else if (type.TypeId == 6)
                {
                    Empty emptyObject = new Empty();
                    emptyObject.DrawFromFile(canvas);
                }

            }
        }

    }
}
