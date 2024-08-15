using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Collections.Data;

internal class ResourceModelConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder
            .ToTable("Resource");

        builder
            .HasKey(e => e.Id);

        builder
            .HasOne(e => e.Collection)
            .WithMany(e => e.Resources)
            .HasForeignKey(e => e.CollectionId);
    }
}
