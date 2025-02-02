﻿using ExceptionLogger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fingerprints.Models
{
    public class MinutiaFileState
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Angle { get; set; }
        public List<Point> Points { get; set; }

        public MinutiaFileState()
        {
            try
            {
                Points = new List<Point>();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public MinutiaState ToMinutiaState()
        {
            var db = new FingerContext();
            var Minutia = db.SelfDefinedMinutiaes.Where(x => x.Name == this.Name).FirstOrDefault();
            db.Dispose();

            return new MinutiaState()
            {
                Id = Convert.ToInt64(this.Id),
                Minutia = Minutia,
                Angle = this.Angle,
                Points = this.Points
            };
        }
    }
}
