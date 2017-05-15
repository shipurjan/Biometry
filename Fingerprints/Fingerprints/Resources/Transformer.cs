using Fingerprints.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fingerprints.Resources
{
    public class Transformer
    {
        List<SelfDefinedMinutiae> minutiaeList;
        public Transformer()
        {
            minutiaeList = new MinutiaeTypeController().GetAllMinutiaeTypes();
        }

        public List<string> getBozorthFormat(List<MinutiaState> minutiaeList)
        {
            List<string> bozorthList = new List<string>();

            foreach (var item in minutiaeList)
            {
                switch (item.Minutia.TypeId)
                {
                    case 2:
                        bozorthList.Add(transformVectorToXYT(item));
                        break;
                    case 5:
                        bozorthList.Add(transformPeakToXYT(item));
                        break;
                    default:
                        break;
                }
            }

            return bozorthList;
        }

        public string transformPeakToXYT(MinutiaState item)
        {
            Point firstPoint = item.Points[0];
            Point secondPoint = item.Points[1];
            Point thirdPoint = item.Points[2];
            // Tworzymy 2 wektory
            Point vectorA = new Point(firstPoint.X - secondPoint.X, firstPoint.Y - secondPoint.Y);
            Point vectorB = new Point(thirdPoint.X - secondPoint.X, thirdPoint.Y - secondPoint.Y);

            // Wyznaczamy wektory jednostkowe
            Point unitVectorA = new Point(vectorA.X / vectorLenght(vectorA), vectorA.Y / vectorLenght(vectorA));
            Point unitVectorB = new Point(vectorB.X / vectorLenght(vectorB), vectorB.Y / vectorLenght(vectorB));

            // Wyznaczamy wektor dwusiecznej kata
            Point bisectVector = new Point((unitVectorA.X + unitVectorB.X) / 2, (unitVectorA.Y + unitVectorB.Y) / 2);

            // Wyznaczamy punkt koncoy wektora dwusiecznej
            Point middlePoint = new Point(secondPoint.X + bisectVector.X, secondPoint.Y + bisectVector.Y);

            //Wyznaczamy kat do płaszczyzny 
            double deltaX = middlePoint.X - secondPoint.X;
            double deltaY = middlePoint.Y - secondPoint.Y;
            double angleInRadian = Math.Atan2(deltaY, deltaX);

            return secondPoint.X + " " + secondPoint.Y + " " + Utils.angleInDegrees(angleInRadian);
        }

        public double vectorLenght(Point vector)
        {
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public string transformVectorToXYT(MinutiaState item)
        {
            return item.Points.First().X + " " + item.Points.First().Y + " " + Utils.angleInDegrees(Convert.ToDouble(item.Angle));
        }
        public List<string> getListWithoutEmptyObjects(List<string> list)
        {
            List<string> temp = new List<string>();
            foreach (var item in list)
            {
                if (getNameFromListElement(item) != "Puste")
                {
                    temp.Add(item);
                }
            }

            return temp;
        }

        public int getMinutiaeDrawingType(string item)
        {
            var type = minutiaeList.Where(x => x.Name == getNameFromListElement(item)).FirstOrDefault();
            if (type == null)
            {
                return 0;
            }
            else
            {
                return type.TypeId;
            }
        }

        public string getNameFromListElement(string listElement)
        {
            return listElement.Split(';')[1];
        }
    }
}