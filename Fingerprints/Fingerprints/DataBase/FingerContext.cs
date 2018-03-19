using Fingerprints.DataBase;
using System.Data.Entity;

namespace Fingerprints
{
    class FingerContext : DbContext
    {
        private const string ConnectionString_Pattern = @"Data Source=(localdb)\{0}; Integrated Security = True; AttachDbFileName={1}\biometry.mdf";

        private static string ConnectionString
        {
            get
            {
                return string.Format(ConnectionString_Pattern, LocalDB_Name, LocalDB_Directory);
            }
        }

        public static string LocalDB_Name { get; set; } = "MSSQLLocalDB";

        public static string LocalDB_Directory { get; set; }

        public FingerContext() : base(ConnectionString)
        {
            System.Data.Entity.Database.SetInitializer<FingerContext>(new UniDBInitializer<FingerContext>());
        }
        public DbSet<SelfDefinedMinutiae> SelfDefinedMinutiaes { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}
