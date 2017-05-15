using Fingerprints.Models;
using Fingerprints.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints.MinutiaeTypes.CurveLine
{
    class CurveLine : Minutiae
    {
        protected Polyline baseLine;
        public CurveLine(MinutiaState state)
        {
            this.state = state;
            ConvertStateColorToBrush();
        }
        public override string ToString()
        {
            string points = null;
            if (baseLine != null && baseLine.Points.Count > 0)
            {
                foreach (var point in convertLinesToPoints(baseLine.Points))
                {
                    points += point.X + ";" + point.Y + ";";
                }
                return state.Id + ";" + state.Minutia.Name + ";" + points;
            }
            return "";
        }

        // Converter
        public PointCollection convertLinesToPoints(PointCollection points)
        {
            double a, b, c = 0;
            PointCollection convertedPoints = new PointCollection();

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
            return convertedPoints;
        }

        public PointCollection fillSpaceBetweenPointsAndAddPoint(PointCollection points, Point p1)
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
            points.Add(p1);

            return points;
        }

        public double calculateA(Point point1, Point point2)
        {
            return point1.Y - point2.Y;
        }

        public double calculateB(Point point1, Point point2)
        {
            return point2.X - point1.X;
        }

        public double calculateC(Point point1, Point point2)
        {
            return (point1.X - point2.X) * point1.Y + (point2.Y - point1.Y) * point1.X;
        }

        public Point createPoint(double a, double b, double c, double x)
        {
            return new Point() { X = x, Y = Math.Floor(((-a * x) - c) / b) };
        }

        public double lengthOfLine(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
        }

        public string ToJson()
        {
            JObject minutiaeJson = new JObject();
            minutiaeJson["id"] = state.Id;
            minutiaeJson["name"] = state.Minutia.Name;

            JArray pointsArray = new JArray();
            foreach (var point in convertLinesToPoints(baseLine.Points))
            {
                pointsArray.Add(point.ToJObject());
            }

            minutiaeJson["points"] = pointsArray;

            return minutiaeJson.ToString();
        }
    }
}
