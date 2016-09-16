using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fingerprints
{
    class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<SelfDefinedMinutiae> SelfDefinedMinutiaes { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
