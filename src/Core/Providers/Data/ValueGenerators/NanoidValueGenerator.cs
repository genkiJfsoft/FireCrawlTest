using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using NanoidDotNet;

namespace Core.Providers.Data.ValueGenerators;

internal class NanoidValueGenerator : ValueGenerator<string>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
    {
        return Nanoid.Generate(Nanoid.Alphabets.LettersAndDigits, 12);
    }
}
