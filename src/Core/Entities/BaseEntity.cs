namespace Core.Entities;
internal class BaseEntity
{
}

/// <summary>
/// A marker for entities with timestamps
/// </summary>
internal interface IEntityTimestampable
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
}
