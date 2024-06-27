using Core.Providers.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Providers.Data.Extensions;

internal static class EntityExtensions
{
    public static bool HasModifiedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }

    public static PropertyBuilder<string> HasNanoidValueGenerator(this PropertyBuilder<string> builder)
    {
        builder.ValueGeneratedOnAdd()
            .HasValueGenerator<NanoidValueGenerator>()
            .HasMaxLength(12)
            .IsFixedLength();

        return builder;
    }
}
