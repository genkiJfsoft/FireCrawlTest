namespace Core.Common.Data;

/// <summary>
/// Currently authenticated user data holder
/// </summary>
public interface ICurrentUser
{
    string? UserId { get; }
}
