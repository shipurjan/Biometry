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
        Helper helper;
        MinutiaeTypeController controller;
        public Picture(MainWindow mw)
        {
            this.mw = mw;
            controller = new MinutiaeTypeController();
            helper = new Helper(this.mw, controller);
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
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.bmp) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.bmp";
                if (openFile.ShowDialog() == true)
                {
                    image.Source = new BitmapImage(new Uri(openFile.FileName));
                    BitmapImage b = new BitmapImage(new Uri(openFile.FileName));
                    image.Height = b.PixelHeight;
                    image.Width = b.PixelWidth;
                    if (image.Tag.ToString() == "Left")
                    {
                        FileTransfer.Save();
                        mw.listBoxImageL.Items.Clear();
                        FileTransfer.ListL.Clear();
                        FileTransfer.LeftImagePath = System.IO.Path.ChangeExtension(openFile.FileName, ".txt");
                        FileTransfer.LoadLeftFile();
                        canvasImage.Children.Clear();
                        if (helper.canInsertEmpty())
                            fillEmptyListWithEmpty();
                        sortMinutiaeListLById();
                        deleteEmptyLine();
                        loadMinutiae(FileTransfer.ListL, canvasImage);
                        mw.canvasImageR.Children.Clear();
                        loadMinutiae(FileTransfer.ListR, mw.canvasImageR);

                    }
                    else
                    {
                        FileTransfer.Save();
                        mw.listBoxImageR.Items.Clear();
                        FileTransfer.ListR.Clear();
                        FileTransfer.RightImagePath = System.IO.Path.ChangeExtension(openFile.FileName, ".txt");
                        FileTransfer.LoadRightFile();
                        canvasImage.Children.Clear();
                        if (helper.canInsertEmpty())
                            fillEmptyListWithEmpty();
                        sortMinutiaeListRById();
                        deleteEmptyLine();
                        loadMinutiae(FileTransfer.ListR, canvasImage);
                        mw.canvasImageL.Children.Clear();
                        loadMinutiae(FileTransfer.ListL, mw.canvasImageL);
                    }
                }
                Canvas.SetTop(canvasImage, Canvas.GetTop(image));
                Canvas.SetLeft(canvasImage, Canvas.GetLeft(image));

                if (helper.canInsertEmpty())
                {
                    helper.deleteUnnecessaryEmpty();
                    helper.addEmptyOnLastLine();
                }
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
            List<SelfDefinedMinutiae> minutiaeList = new MinutiaeTypeController().GetAllMinutiaeTypes();

            IDraw draw = null;
            foreach (var item in list)
            {
                string[] tmp = item.Split(';');
                
                var type = minutiaeList.Where(x => x.Name == tmp[1]).FirstOrDefault();

                if(type == null)
                {
                    draw = new Empty();
                }
                else if (type.TypeId == 1)
                {
                    draw = new SinglePoint(type.Name, type.Color, type.Size, type.Thickness, Convert.ToDouble(tmp[2]), Convert.ToDouble(tmp[3]), Convert.ToInt64(tmp[0]));
                }
                else if (type.TypeId == 2)
                {
                    draw = new Vector(type.Name, type.Color, type.Size, type.Thickness, Convert.ToDouble(tmp[2]), Convert.ToDouble(tmp[3]), Convert.ToDouble(tmp[4]), Convert.ToInt64(tmp[0]));
                }
                else if (type.TypeId == 3)
                {
                    draw = new CurveLine(type.Name, type.Color, type.Thickness, tmp, Convert.ToInt64(tmp[0]));
                }
                else if (type.TypeId == 4)
                {
                    draw = new Triangle(type.Name, type.Color, type.Thickness, Convert.ToDouble(tmp[2]), Convert.ToDouble(tmp[3]), Convert.ToDouble(tmp[4]), Convert.ToDouble(tmp[5]), Convert.ToDouble(tmp[6]), Convert.ToDouble(tmp[7]), Convert.ToInt64(tmp[0]));
                }
                else if (type.TypeId == 5)
                {
                    draw = new Peak(type.Name, type.Color, type.Thickness, Convert.ToDouble(tmp[2]), Convert.ToDouble(tmp[3]), Convert.ToDouble(tmp[4]), Convert.ToDouble(tmp[5]), Convert.ToDouble(tmp[6]), Convert.ToDouble(tmp[7]), Convert.ToInt64(tmp[0]));
                }
                else if (type.TypeId == 6)
                {
                    draw = new Segment(type.Name, type.Color, type.Thickness, Convert.ToDouble(tmp[2]), Convert.ToDouble(tmp[3]), Convert.ToDouble(tmp[4]), Convert.ToDouble(tmp[5]), Convert.ToInt64(tmp[0]));
                }
                else
                {
                    draw = new Empty();
                }

                draw.DrawFromFile(canvas);

            }
        }
        private void fillEmpty()
        {
            Empty emptyObject = new Empty();
            int l = mw.canvasImageL.Children.Count;
            int r = mw.canvasImageR.Children.Count;
            if (l > r)
            {
                for (int i = 0; i < l-r; i++)
                {
                    emptyObject.DrawFromFile(mw.canvasImageR);
                }
            }
            else
            {
                for (int i = 0; i < r - l; i++)
                {
                    emptyObject.DrawFromFile(mw.canvasImageL);
                }
            }
        }

        private void sortMinutiaeListRById()
        {
            for (int indexFrom = 0; indexFrom < FileTransfer.ListL.Count(); indexFrom++)
            {
                string minutiaeIdL = getIdFromListElement(FileTransfer.ListL[indexFrom]);
                if (minutiaeIdL == "0")
                    continue;

                for (int indexTo = 0; indexTo < FileTransfer.ListR.Count(); indexTo++)
                {
                    var minutiaeIdR = getIdFromListElement(FileTransfer.ListR[indexTo]);
                    if (minutiaeIdL == minutiaeIdR)
                    {
                        swapElementsR(indexFrom, indexTo);
                        break;
                    }
                    if (indexTo == FileTransfer.ListR.Count() - 1)
                    {
                        FileTransfer.ListR.Insert(indexFrom, "0;Puste");
                        FileTransfer.ListL.Add("0;Puste");
                        break;
                    }
                }
            }
        }

        private void sortMinutiaeListLById()
        {
            for (int indexFrom = 0; indexFrom < FileTransfer.ListR.Count(); indexFrom++)
            {
                string minutiaeIdR = getIdFromListElement(FileTransfer.ListR[indexFrom]);
                if (minutiaeIdR == "0")
                    continue;

                for (int indexTo = 0; indexTo < FileTransfer.ListL.Count(); indexTo++)
                {
                    var minutiaeIdL = getIdFromListElement(FileTransfer.ListL[indexTo]);
                    if (minutiaeIdL == minutiaeIdR)
                    {
                        swapElementsL(indexFrom, indexTo);
                        break;
                    }
                    if (indexTo == FileTransfer.ListL.Count() - 1)
                    {
                        FileTransfer.ListL.Insert(indexFrom, "0;Puste");
                        FileTransfer.ListR.Add("0;Puste");
                        break;
                    }

                }
                
            }
        }

        private void fillEmptyListWithEmpty()
        {
            if (FileTransfer.ListL.Count() > FileTransfer.ListR.Count())
            {
                for (int i = FileTransfer.ListR.Count(); i < FileTransfer.ListL.Count(); i++)
                    FileTransfer.ListR.Add("0;Puste");
            }
            else
            {
                for (int i = FileTransfer.ListL.Count(); i < FileTransfer.ListR.Count(); i++)
                    FileTransfer.ListL.Add("0;Puste");
            }
        }

        private void checkIfInsertNeededR(int indexFrom, int indexTo)
        {
            if (indexTo == FileTransfer.ListR.Count() - 1)
            {
                FileTransfer.ListR.Insert(indexFrom -1 , "0;Puste");
                FileTransfer.ListL.Add("0;Puste");
            }                               
        }

        private void checkIfInsertNeededL(int indexFrom, int indexTo)
        {
            if (indexTo == FileTransfer.ListL.Count() - 1)
            {             
                FileTransfer.ListL.Insert(indexFrom - 1, "0;Puste");
                FileTransfer.ListR.Add("0;Puste");             
            }
        }

        private string getIdFromListElement(string listElement)
        {
            return listElement.Split(';')[0];
        }

        private void swapElementsR(int indexFrom, int indexTo)
        {
            var tmp = FileTransfer.ListR[indexFrom];
            FileTransfer.ListR[indexFrom] = FileTransfer.ListR[indexTo];
            FileTransfer.ListR[indexTo] = tmp;
        }

        private void swapElementsL(int indexFrom, int indexTo)
        {
            var tmp = FileTransfer.ListL[indexFrom];
            FileTransfer.ListL[indexFrom] = FileTransfer.ListL[indexTo];
            FileTransfer.ListL[indexTo] = tmp;
        }

        private void deleteEmptyLine()
        {
            if (FileTransfer.ListL.Count == 0 || FileTransfer.ListR.Count() == 0)
            {
                return;
            }
            int count = FileTransfer.ListL.Count();
            for (int index = 0; index < count; index++)
            {
                if (FileTransfer.ListL[index] == "0;Puste" && FileTransfer.ListR[index] == "0;Puste")
                {
                    FileTransfer.ListL.RemoveAt(index);
                    FileTransfer.ListR.RemoveAt(index);
                    count--;
                    index--;
                }
            }
        }

        //private void sortMinutiaeList()
        //{
        //    if (FileTransfer.ListL.Count() > FileTransfer.ListR.Count())
        //        sortMinutiaeListRById();
        //    else
        //        sortMinutiaeListLById();
        //}



    }
}
