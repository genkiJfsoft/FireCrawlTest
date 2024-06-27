namespace Core.Entities;

internal class Collection : BaseEntity, IEntityTimestampable
{
    public int Id { get; }
    public string PublicId { get; } = null!;
    public required string Title { get; init; }
    public string Description { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public List<Resource> Resources { get; } = [];
}
