using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetcoreApiTemplate.Data.Configuration;
using NetcoreApiTemplate.Models.Table;
using System.Data;

namespace NetcoreApiTemplate.Data.Context
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.LazyLoadingEnabled = false;
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ////base.OnModelCreating(modelBuilder);
            ///
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }
        public IDbConnection Connection => Database.GetDbConnection();
        public DbSet<Employee> Employees { get; set; }
    }
}
