using backend.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace backend.DataAccess
{
    public class MrpContext(DbContextOptions<MrpContext> options) : DbContext(options)
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Bom> Boms { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<OperationType>();

            modelBuilder.Entity<Item>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<Bom>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Stock>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Stock>()
                .Property(s => s.Operation)
                .HasConversion<int>()
                .HasColumnType("integer");
        }
    }

}
