using Core.Common.Data;

namespace Core.Collections.Data;

internal class Collection : DataModel, ITimestampableModel
{
    public int Id { get; }
    public string PublicId { get; } = null!;
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public List<Resource> Resources { get; } = [];
}
