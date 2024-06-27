namespace Core.Features.Collections;

public class ResourceDto
{
    public int Id { get; init; }
    public int CollectionId { get; init; }
    public required string Title { get; init; }
    public string? Notes { get; init; }
    public required string LinkToUrl { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? LastModifiedAt { get; init; }
}
