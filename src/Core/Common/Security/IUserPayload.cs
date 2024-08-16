namespace Core.Common.Security;

/// <summary>
/// Currently authenticated user data holder
/// </summary>
public interface IUserPayload
{
    string? UserId { get; }
}
