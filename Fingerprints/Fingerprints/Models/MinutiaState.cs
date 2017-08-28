using Fingerprints.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Fingerprints.Resources;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using System.Windows.Media.Imaging;

namespace Fingerprints.Models
{
    public class MinutiaState : BindableBase
    {
        public SelfDefinedMinutiae Minutia { get; set; }
        public List<Point> Points { get; set; }
        public double Angle { get; set; }
        private long _id;
        public long Id { get { return _id; } set { SetProperty(ref _id, value); } }

        public string MinutiaName { get { return Minutia.Name; } }

        public MinutiaState()
        {
            Points = new List<Point>();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public List<int> intPoints
        {
            get
            {
                var intPoints = new List<int>();
                foreach (var item in Points)
                {
                    var floorPoint = item.ToFloorPoint();
                    intPoints.Add(Convert.ToInt16(floorPoint.X));
                    intPoints.Add(Convert.ToInt16(floorPoint.Y));
                }
                return intPoints;
            }
        }

        public MinutiaFileState ToMinutiaFileState()
        {
            return new MinutiaFileState()
            {
                Id = this.Id,
                Name = this.Minutia.Name,
                Angle = this.Angle,
                Points = this.Points
            };
        }

        public override string ToString()
        {
            return Minutia.Name;
        }
    }
}
