using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core;
using System.ComponentModel.DataAnnotations;


namespace Fingerprints
{
    class Project
    {
        [Key]
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<SelfDefinedMinutiae> SelfDefinedMinutiaes { get; set; }
    }
}
