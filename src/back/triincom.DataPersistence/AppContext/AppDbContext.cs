using Microsoft.EntityFrameworkCore;
using triincom.Core.Entities;

namespace triincom.DataPersistence.AppContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<ApplicationEntity> Applications { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
