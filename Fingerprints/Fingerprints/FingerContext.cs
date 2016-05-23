using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Fingerprints
{
    class FingerContext : DbContext
    {
        public FingerContext() : base("fingerprint")
        {
                
        }
        public DbSet<MinutiaeType> MinutiaeTypes { get; set; }
    }
}
