﻿using Fingerprints.Models;
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
    public class MinutiaState
    {
        public SelfDefinedMinutiae Minutia { get; set; }
        public List<Point> Points { get; set; }
        public double Angle { get; set; }
        public long Id { get; set; }
        public string MinutiaName { get { return Minutia.Name; } }

        public MinutiaState()
        {
            Points = new List<Point>();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public MinutiaFileState ToMinutiaFileState()
        {
            return new MinutiaFileState()
            {
                Id = Convert.ToString(this.Id),
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
