using Core.Common.Data;

namespace Core.Collections.Data;

public record CollectionData : DataObject
{
    public int Id { get; init; }
    public string PublicId { get; init; } = null!;
    public required string Title { get; init; }
    public string Description { get; init; } = string.Empty;
    public List<ResourceData> Resources { get; init; } = [];
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? LastModifiedAt { get; init; }

    public static CollectionData Empty() => new() { Title = string.Empty };
}
