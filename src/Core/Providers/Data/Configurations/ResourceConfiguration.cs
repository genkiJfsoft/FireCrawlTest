using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Providers.Data.Configurations;

internal class ResourceConfiguration : IEntityTypeConfiguration<Resource>
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
