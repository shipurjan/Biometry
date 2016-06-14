using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core;
using System.ComponentModel.DataAnnotations;


namespace Fingerprints
{
    class SelfDefinedMinutiae
    {
        [Key]
        public int MinutiaeId { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public int TypeId { get; set; }
        public string  Color { get; set; }
        public double Size { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
