using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fingerprints
{
    public class MinutiaState
    {
        public SelfDefinedMinutiae Minutia { get; set; }
        public List<Point> Points { get; set; }
        public double Angle { get; set; }
        public long Id { get; set; }

        public MinutiaState()
        {

        }



    }
}
