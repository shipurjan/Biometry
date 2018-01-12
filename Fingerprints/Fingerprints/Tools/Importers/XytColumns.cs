using ExceptionLogger;
using Fingerprints.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Tools.Importers
{
    public static class XytColumns
    {
        public static XytRow GetXytRow(string _row)
        {
            XytRow result = null;
            Char delimiter = ' ';
            try
            {
                string[] splitted = _row.Split(delimiter);
                result = new XytRow(splitted[0], splitted[1], splitted[2]);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }
    }

    public class XytRow
    {
        public int X { get; }
        public int Y { get; }
        public double Angle { get; }

        public XytRow(string _x, string _y, string _angle)
        {
            try
            {
                X = Convert.ToInt32(_x);
                Y = Convert.ToInt32(_y);
                Angle = GetAngleInRadians(_angle);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private double GetAngleInRadians(string _dir)
        {
            double result = 0;
            int direction = 0;
            try
            {
                // multiple direction value with constant increment to get angle in degrees
                direction = Convert.ToInt32(_dir);
                

                if (direction > 180)
                {
                    result = (360 - direction) * Math.PI / 180;
                }
                else
                {
                    result = (-1 * direction) * Math.PI / 180;
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
