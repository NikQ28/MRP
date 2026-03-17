using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mrp.DataAccess.Entities;
using Mrp.DataAccess.Configurations;

namespace Mrp.DataAccess.Configurations
{
    public class WatchBomConfiguration : IEntityTypeConfiguration<WatchBomEntity>
    {
        public void Configure(EntityTypeBuilder<WatchBomEntity> builder)
        {
            builder.HasKey(w => w.Id);

            builder
                .HasOne(w => w.Parent)
                .WithMany(i => i.Parents)
                .HasForeignKey(w => w.ParentId);

            builder
                .HasOne(w => w.Child)
                .WithMany(i => i.Children)
                .HasForeignKey(w => w.ChildId);

            builder.Property(w => w.ParentId).IsRequired();
            builder.Property(w => w.ChildId).IsRequired();
            builder.Property(w => w.Count).IsRequired();
        }
    }
}
