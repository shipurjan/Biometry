using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Fingerprints
{
    class MinutiaeType
    {
        public int MinutiaeTypeId { get; set; }
        public string Name { get; set; }
        public int DrawType { get; set; }
        public string Color { get; set; }
    }
}
