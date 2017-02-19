using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Resources
{
    public static class Utils
    {
        public static int angleInDegrees(double angleInRadian)
        {
            int angle = (int)Math.Round(angleInRadian * 180 / 3.14);

            return angle < 0 ? angle *= -1 : angle = 360 - angle;
        }
    }
}
