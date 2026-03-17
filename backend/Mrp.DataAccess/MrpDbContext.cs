using Mrp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Mrp.DataAccess.Configurations;

namespace Mrp.DataAccess
{
    public class MrpDbContext(DbContextOptions<MrpDbContext> options) : DbContext(options)
    {
        public DbSet<ItemEntity> Items { get; set; }
        public DbSet<WatchBomEntity> WatchBoms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new WatchBomConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
