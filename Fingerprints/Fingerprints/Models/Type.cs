using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Fingerprints
{
    class Type
    {
        public int TypeId { get; set; }
        public string Name { get; set; }
        public int DrawingType { get; set; }
        public virtual ICollection<SelfDefinedMinutiae> SelfDefinedMinutiaes { get; set; }

        public override string ToString()
        {
            return Name; 
        }
    }
}
