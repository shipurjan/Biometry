using ExceptionLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Fingerprints.Tools.LinestoPointsConverter
{
    public class LinesToPointsConverter
    {
        public PointCollection convertLinesToPoints(PointCollection points)
        {
            double a, b, c = 0;
            PointCollection convertedPoints = new PointCollection();

            try
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    Point p1, p2;
                    p1 = points[i];
                    p2 = points[i + 1];

                    a = calculateA(p1, p2);
                    b = calculateB(p1, p2);
                    c = calculateC(p1, p2);

                    if (p1.X < p2.X && b != 0)
                    {

                        for (double x = p1.X; x <= p2.X; x++)
                        {
                            Point createdPoint = createPoint(a, b, c, x);
                            convertedPoints = fillSpaceBetweenPointsAndAddPoint(convertedPoints, createdPoint);
                        }
                    }
                    else if (b != 0)
                    {
                        for (double x = p1.X; x >= p2.X; x--)
                        {
                            Point createdPoint = createPoint(a, b, c, x);
                            convertedPoints = fillSpaceBetweenPointsAndAddPoint(convertedPoints, createdPoint);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return convertedPoints;
        }

        public PointCollection fillSpaceBetweenPointsAndAddPoint(PointCollection points, Point p1)
        {
            try
            {
                if (points.LastOrDefault() != new Point() { X = 0, Y = 0 })
                {
                    while (lengthOfLine(p1, points.LastOrDefault()) > 1.5)
                    {
                        Point lastPoint = points.LastOrDefault();
                        if (p1.Y < lastPoint.Y)
                        {
                            lastPoint.Y -= 1;
                        }
                        else
                        {
                            lastPoint.Y += 1;
                        }
                        points.Add(lastPoint);
                    }
                }
                AddToPointCollectinoIfNotExists(points, p1);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }

            return points;
        }

        public PointCollection fillSpaceBetweenPoints(Point p1, Point p2)
        {
            PointCollection result = null;
            try
            {
                result = new PointCollection();
                result.Add(p1);
                result.Add(p2);
                result = convertLinesToPoints(result);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        public double calculateA(Point point1, Point point2)
        {
            double result = 0.0;
            try
            {
                result = point1.Y - point2.Y;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        public double calculateB(Point point1, Point point2)
        {
            double result = 0.0;
            try
            {
                result = point2.X - point1.X;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        public double calculateC(Point point1, Point point2)
        {
            double result = 0.0;
            try
            {
                result = (point1.X - point2.X) * point1.Y + (point2.Y - point1.Y) * point1.X;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        public Point createPoint(double a, double b, double c, double x)
        {
            Point result = new Point();
            try
            {
                result = new Point() { X = x, Y = Math.Floor(((-a * x) - c) / b) };
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        public double lengthOfLine(Point p1, Point p2)
        {
            double result = 0.0;
            try
            {
                result = Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        private void AddToPointCollectinoIfNotExists(PointCollection points, Point pointToAdd)
        {
            try
            {
                if (!points.Contains(pointToAdd))
                {
                    points.Add(pointToAdd);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
