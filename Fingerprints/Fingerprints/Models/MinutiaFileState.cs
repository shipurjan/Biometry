using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fingerprints.Models
{
    public class MinutiaFileState
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Angle { get; set; }
        public List<Point> Points { get; set; }

        public MinutiaState ToMinutiaState()
        {
            var db = new FingerContext();
            var Minutia = db.SelfDefinedMinutiaes.Where(x => x.Name == this.Name).FirstOrDefault();
            db.Dispose();

            return new MinutiaState()
            {
                Id = this.Id,
                Minutia = Minutia,
                Angle = this.Angle,
                Points = this.Points
            };
        }
    }
}
