using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints
{
    class CurveLine : Minutiae
    {
        Brush color;
        Polyline baseLine;
        Point currentPoint;
        bool newLine;
        bool clickCount = true;

        MouseButtonEventHandler handlerMouseDown = null;
        MouseEventHandler handler = null;

        public CurveLine(string color)
        {
            this.color = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString(color);
        }
        /// <summary>
        /// Dodaje handlery do myszy, rysuje linie ciagla, zapisuje jako liste puktow
        /// </summary>
        /// <param name="canvas">Atkualny canvas</param>
        /// <param name="image">Aktualny obrazek</param>
        /// <param name="border1">Ramka 1</param>
        /// <param name="border2">Ramka 2</param>
        public override void Draw(OverridedCanvas canvas, Image image, Border border1, Border border2)
        {
            handlerMouseDown += (ss, ee) =>
            {
                newLine = true;               
            };

            image.MouseDown += handlerMouseDown;
            canvas.MouseDown += handlerMouseDown;

            handler += (ss, ee) =>
            {
                if (ee.RightButton == MouseButtonState.Pressed && border1.BorderBrush == Brushes.DeepSkyBlue)
                {
                    if (newLine)
                    {
                        baseLine = new Polyline
                        {
                            Stroke = color,
                            StrokeThickness = 0.3
                        };
                        //canvas.Children.Add(baseLine);
                        baseLine.Name = Name;
                        canvas.AddLogicalChild(baseLine);   
                        newLine = false; 
                    }
                    currentPoint = ee.GetPosition(canvas);
                    baseLine.Points.Add(currentPoint);
                    clickCount = false;
                }
                if (ee.RightButton == MouseButtonState.Released && clickCount == false)
                {
                    if (border1.BorderBrush == Brushes.Black)
                    {
                        border1.BorderBrush = Brushes.DeepSkyBlue;
                        border2.BorderBrush = Brushes.Black;
                        clickCount = true;
                        //canvas.Children[canvas.Children.Count - 1].Opacity = 0.5;
                    }
                    else if (border2.BorderBrush == Brushes.Black)
                    {                        
                        border1.BorderBrush = Brushes.Black;
                        border2.BorderBrush = Brushes.DeepSkyBlue;
                        //canvas.Children[canvas.Children.Count - 1].Opacity = 0.5;
                        clickCount = true;
                    }
                }
            };
            image.MouseMove += handler;
            canvas.MouseMove += handler;
        }

        public override void DeleteEvent(Image image, OverridedCanvas canvas)
        {
            image.MouseMove -= handler;
            image.MouseDown -= handlerMouseDown;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}

