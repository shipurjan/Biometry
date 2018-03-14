using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fingerprints.Models;

namespace Fingerprints
{
    public class SelfDefinedMinutiae
    {
        public int SelfDefinedMinutiaeId { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public DrawingType DrawingType { get; set; }
        public string  Color { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
