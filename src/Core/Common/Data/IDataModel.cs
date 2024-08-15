namespace Core.Common.Data;

/// <summary>
/// A marker for data models.
/// 
/// Used by Entity Framework to shape the data in database
/// </summary>
internal interface IDataModel
{

}

/// <summary>
/// A marker for data models with timestamps
/// </summary>
internal interface ITimestampableModel : IDataModel
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
}
