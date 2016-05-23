using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints
{
    class CurveLine : Minutiae
    {
        Polyline baseLine;
        Point currentPoint;
        bool newLine;
        
        MouseButtonEventHandler handlerMouseDown = null;
        MouseEventHandler handler = null;

        public override void Draw(Canvas canvas, Image image)
        {
            handlerMouseDown += (ss, ee) =>
            {
                newLine = true;
            };
            image.MouseDown += handlerMouseDown;

            handler += (ss, ee) =>
            {
                if (ee.RightButton == MouseButtonState.Pressed)
                {
                    if (newLine)
                    {
                        baseLine = new Polyline
                        {
                            Stroke = Brushes.Red,
                            StrokeThickness = 0.3
                        };

                        canvas.Children.Add(baseLine);
                        newLine = false;
                    }

                    currentPoint = ee.GetPosition(canvas);
                    baseLine.Points.Add(currentPoint);
                }

            };
            image.MouseMove += handler;
        }

        public void DeleteEvent(Image image)
        {
            image.MouseMove -= handler;
        }
    }
}

