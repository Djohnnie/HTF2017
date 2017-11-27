using Microsoft.EntityFrameworkCore;

namespace HTF2017.DataAccess
{
    public class HtfDbContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Android> Androids { get; set; }

        public DbSet<SensoryData> SensoryData { get; set; }

        public DbSet<SensoryDataRequest> SensoryDataRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=tcp:htf2017.database.windows.net,1433;Initial Catalog=htf2017;Persist Security Info=False;User ID=djohnnie;Password=Gadash25;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>().HasKey(x => x.Id).ForSqlServerIsClustered(clustered: false);
            modelBuilder.Entity<Location>().HasIndex(x => x.SysId).IsUnique().ForSqlServerIsClustered();
            modelBuilder.Entity<Location>().Property(x => x.SysId).ValueGeneratedOnAdd();

            modelBuilder.Entity<Team>().HasKey(x => x.Id).ForSqlServerIsClustered(clustered: false);
            modelBuilder.Entity<Team>().HasIndex(x => x.SysId).IsUnique().ForSqlServerIsClustered();
            modelBuilder.Entity<Team>().Property(x => x.SysId).ValueGeneratedOnAdd();

            modelBuilder.Entity<Android>().HasKey(x => x.Id).ForSqlServerIsClustered(clustered: false);
            modelBuilder.Entity<Android>().HasIndex(x => x.SysId).IsUnique().ForSqlServerIsClustered();
            modelBuilder.Entity<Android>().Property(x => x.SysId).ValueGeneratedOnAdd();

            modelBuilder.Entity<SensoryData>().HasKey(x => x.Id).ForSqlServerIsClustered(clustered: false);
            modelBuilder.Entity<SensoryData>().HasIndex(x => x.SysId).IsUnique().ForSqlServerIsClustered();
            modelBuilder.Entity<SensoryData>().Property(x => x.SysId).ValueGeneratedOnAdd();

            modelBuilder.Entity<SensoryDataRequest>().HasKey(x => x.Id).ForSqlServerIsClustered(clustered: false);
            modelBuilder.Entity<SensoryDataRequest>().HasIndex(x => x.SysId).IsUnique().ForSqlServerIsClustered();
            modelBuilder.Entity<SensoryDataRequest>().Property(x => x.SysId).ValueGeneratedOnAdd();
        }
    }
}