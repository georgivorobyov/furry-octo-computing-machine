using Sumc.Data.Migrations;
using Sumc.Models;
using System.Data.Entity;

namespace Sumc.Data
{
    public class SumcContext : DbContext
    {
        static SumcContext()
        {
            var migrations = new MigrateDatabaseToLatestVersion<SumcContext, Configuration>();
            Database.SetInitializer(migrations);
        }

        public SumcContext()
            : base(nameOrConnectionString: "SumcDatabase")
        {
        }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<Admin> Admins { get; set; }
    }
}
