using Core.Entities;
using Core.Providers.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Providers.Data.Configurations;

internal class CollectionConfiguration : IEntityTypeConfiguration<Collection>
{
    public void Configure(EntityTypeBuilder<Collection> builder)
    {
        builder
            .ToTable("Collection");

        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.PublicId)
            .HasNanoidValueGenerator();

        builder
            .HasMany(e => e.Resources)
            .WithOne(e => e.Collection);
    }
}
