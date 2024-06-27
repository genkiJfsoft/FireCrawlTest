namespace Core.Entities;

internal class Resource : BaseEntity, IEntityTimestampable
{
    public int Id { get; }
    public int CollectionId { get; init; }
    public required string Title { get; init; }
    public string? Notes { get; init; }
    public required string LinkToUrl { get; init; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public Collection Collection { get; } = null!;
}
