using Core.Common.Data;

namespace Core.Collections.Data;

public record ResourceData : DataObject
{
    public int Id { get; init; }
    public int CollectionId { get; init; }
    public required string Title { get; init; }
    public string? Notes { get; init; }
    public required string LinkToUrl { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? LastModifiedAt { get; init; }
}
