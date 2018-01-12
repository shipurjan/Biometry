using ExceptionLogger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Tools.Importers
{
    /// <summary>
    /// description of columns: http://ffpis.sourceforge.net/man/mindtct.html
    /// </summary>
    public static class MinColumns
    {
        public static MinRow GetMinRow(string _row)
        {
            MinRow result = null;
            Char delimiter = ':';
            try
            {
                _row = _row.Replace(" ", string.Empty);
                string[] splitted = _row.Split(delimiter);

                if (splitted.Length > 6)
                {
                    string mx = splitted[1].Split(',')[0];
                    string my = splitted[1].Split(',')[1];

                    result = new MinRow(splitted[0], mx, my, splitted[2], splitted[3], splitted[4],
                        splitted[5], splitted[6]);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }
    }

    public enum MindtctMinutiaTypes
    {
        Bifurcation,
        RidgeEnding,
    }

    public enum MindtctFeatureTypes
    {
        Appearing,
        Disappearing
    }

    public class MinRow
    {
        public int Mn { get; }
        public int Mx { get; }
        public int My { get; }

        /// <summary>
        /// calculated to radians used for drawing on bitmap
        /// </summary>
        public double Direction { get; }

        public double ReliabilityMeasure { get; }
        public MindtctMinutiaTypes MinutiaType { get; }
        public MindtctFeatureTypes FeatureType { get; }
        public int Fn { get; }

        public MinRow(string _mn, string _mx, string _my, string _dir, string _rel, string _typ, string _ftyp,
            string _fn)
        {
            try
            {
                Mn = Convert.ToInt32(_mn);
                Mx = Convert.ToInt32(_mx);
                My = Convert.ToInt32(_my);
                Direction = getAngleInRadians(_dir);
                ReliabilityMeasure = Convert.ToDouble(_rel, CultureInfo.InvariantCulture);
                MinutiaType = _typ == "BIF" ? MindtctMinutiaTypes.Bifurcation : MindtctMinutiaTypes.RidgeEnding;
                FeatureType = _ftyp == "APP" ? MindtctFeatureTypes.Appearing : MindtctFeatureTypes.Disappearing;
                Fn = Convert.ToInt32(_fn);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private double getAngleInRadians(string _dir)
        {
            double result = 0;
            int direction = 0;
            try
            {
                // multiple direction value with constant increment to get angle in degrees
                direction = Convert.ToInt32((Convert.ToInt32(_dir) + 8) * 11.25);

                if (direction > 180)
                {
                    result = (direction - 360) * Math.PI / 180;
                }
                else
                {
                    result = direction * Math.PI / 180;
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
