using Fingerprints.Interfaces;
using System;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using Fingerprints.Resources;
using ExceptionLogger;
using Fingerprints.ViewModels;
using System.Linq;

namespace Fingerprints.MinutiaeTypes
{
    class TriangleState : MinutiaStateBase, IMouseClickable, IDrawable, IMouseMoveable
    {
        private bool closePolyline = false; 

        public TriangleState(DrawingService _oDrawingService, int? _atIndex = null) : base(_oDrawingService, _atIndex)
        {
        }

        public void DrawProcedure()
        {
            try
            {
                if (!closePolyline)
                    // Draw polyline based on current point array
                    WriteableBmp.DrawPolyline(IntPoints, Colors.OrangeRed);                
                else            
                    // Draw closed polyline    
                    WriteableBmp.DrawPolyline(GetClosedPolygonArray(IntPoints), Colors.OrangeRed);                  
                
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void MouseClick(object sender, MouseButtonEventArgs args)
        {
            try
            {
                if (args.RightButton == MouseButtonState.Pressed)
                {
                    if (Points.Count == 0)
                    {
                        Points.Add(args.GetPosition((IInputElement)sender).ToFloorPoint());
                    }
                    else if (Points.Count == 2)
                    {
                        Points.Add(args.GetPosition((IInputElement)sender).ToFloorPoint());
                        closePolyline = true;
                    }
                    else if (Points.Count == 3)
                    {                                            
                        DrawingService.InitiateNewDrawing();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void MouseMove(object sender, MouseEventArgs args)
        {
            try
            {
                if (Points.Count == 1)
                {
                    Points.Add(args.GetPosition((IInputElement)sender).ToFloorPoint());
                }
                else if (Points.Count == 2)
                {
                    Points[1] = (args.GetPosition((IInputElement)sender).ToFloorPoint());
                }
                else if (Points.Count == 3)
                {
                    Points[2] = (args.GetPosition((IInputElement)sender).ToFloorPoint());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Creates tmp array for closing Polyline
        /// </summary>
        /// <param name="_intPoints"></param>
        /// <returns></returns>
        private int[] GetClosedPolygonArray(int[] _intPoints)
        {
            int[] intTmp = null;
            try
            {
                // Init new array 2 elements bigger then given array
                intTmp = new int[_intPoints.Length + 2];

                // Copy items from one array to another
                _intPoints.CopyTo(intTmp, 0);

                // Add point from beggining, to end of array
                intTmp[6] = intTmp[0];
                intTmp[7] = intTmp[1];
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return intTmp;
        }
    }
}
