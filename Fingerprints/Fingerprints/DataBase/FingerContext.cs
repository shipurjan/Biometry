using Fingerprints.DataBase;
using System.Data.Entity;

namespace Fingerprints
{
    class FingerContext : DbContext
    {
        public FingerContext()
        {
            System.Data.Entity.Database.SetInitializer<FingerContext>(new UniDBInitializer<FingerContext>());
        }
        public DbSet<SelfDefinedMinutiae> SelfDefinedMinutiaes { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}
