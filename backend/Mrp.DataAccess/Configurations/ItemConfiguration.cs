using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mrp.DataAccess.Entities;

namespace Mrp.DataAccess.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<ItemEntity>
    {
        public void Configure(EntityTypeBuilder<ItemEntity> builder)
        {
            builder.HasKey(i => i.Id);

            builder
                .HasMany(i => i.Parents)
                .WithOne(w => w.Parent)
                .HasForeignKey(i => i.ParentId);

            builder
                .HasMany(i => i.Children)
                .WithOne(w => w.Child)
                .HasForeignKey(i => i.ChildId);

            builder.Property(i => i.Name).IsRequired();
        }
    }
}
