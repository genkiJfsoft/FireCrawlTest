namespace Core.Features.Collections;

public record CollectionDto
{
    public int Id { get; init; }
    public string PublicId { get; init; } = null!;
    public required string Title { get; init; }
    public string Description { get; init; } = string.Empty;
    public List<ResourceDto> Resources { get; init; } = [];
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? LastModifiedAt { get; init; }

    public static CollectionDto Empty() => new() { Title = string.Empty };
}
