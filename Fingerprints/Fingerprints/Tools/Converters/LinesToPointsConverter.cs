using ExceptionLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Fingerprints.Tools.Converters
{
    public static class LinesToPointsConverter
    {
        public static List<Point> ConvertPoints(List<Point> points)
        {
            List<Point> result = null;
            try
            {
                result = new List<Point>();

                for (int i = 0; i < points.Count - 1; i++)
                {
                    result.AddRange(LinePoints(points[i], points[i + 1]));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        public static List<Point> LinePoints(Point point1, Point point2)
        {
            List<Point> result = null;
            try
            {
                result = new List<Point>();
                double steps = 0;
                var deltaX = point2.X - point1.X;
                var deltaY = point2.Y - point1.Y;

                if (Math.Abs(deltaX) > Math.Abs(deltaY))
                    steps = Math.Abs(deltaX);

                if (Math.Abs(deltaY) > Math.Abs(deltaX))
                    steps = Math.Abs(deltaY);

                var Xincrement = deltaX / steps;
                var Yincrement = deltaY / steps;

                var x = point1.X;
                var y = point1.Y;
                result.Add(new Point(Math.Floor(x), Math.Floor(y)));

                for (int i = 0; i < steps; i++)
                {
                    x = x + Xincrement;
                    y = y + Yincrement;

                    result.Add(new Point(Math.Floor(x), Math.Floor(y)));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }
    }
}
