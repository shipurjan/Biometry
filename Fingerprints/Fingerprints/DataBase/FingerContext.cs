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
        public FingerContext()
        {
            System.Data.Entity.Database.SetInitializer<FingerContext>(new CreateDatabaseIfNotExists<FingerContext>());            
        }
        public DbSet<Type> Types { get; set; }
        public DbSet<SelfDefinedMinutiae> SelfDefinedMinutiaes { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}
