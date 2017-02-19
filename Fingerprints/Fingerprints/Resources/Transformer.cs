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

        public List<string> getBozorthFormat(List<string> minutiaeList)
        {
            List<string> bozorthList = new List<string>();

            List<string> temp = new List<string>();

            foreach (string item in minutiaeList)
            {
                switch (getMinutiaeDrawingType(item))
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

        public string transformPeakToXYT(string item)
        {
            string[] array = item.Split(';');
            Point firstPoint = new Point(Convert.ToInt16(array[2]), Convert.ToInt16(array[3]));
            Point secondPoint = new Point(Convert.ToInt16(array[4]), Convert.ToInt16(array[5]));
            Point thirdPoint = new Point(Convert.ToInt16(array[6]), Convert.ToInt16(array[7]));
            Point middlePoint = new Point((firstPoint.X + thirdPoint.X) / 2, (firstPoint.Y + thirdPoint.Y) / 2);
            double deltaX = middlePoint.X - secondPoint.X;
            double deltaY = middlePoint.Y - secondPoint.Y;
            double angleInRadian = Math.Atan2(deltaY, deltaX);

            return secondPoint.X + " " + secondPoint.Y + " " + Utils.angleInDegrees(angleInRadian);
        }

        public string transformVectorToXYT(string item)
        {
            string[] array = item.Split(';');
            double angle = Convert.ToDouble(array[4]) * 180 / 3.14;

            return array[2] + " " + array[3] + " " + Utils.angleInDegrees(angle);
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
