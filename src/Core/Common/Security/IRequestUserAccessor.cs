using System.Security.Claims;

namespace Core.Common.Security;

/// <summary>
/// Used to supply current user's <see cref="ClaimsPrincipal"/> in <see cref="MediatR.IRequest"/>.
/// </summary>
public interface IRequestUserAccessor
{
    /// <summary>
    /// Gets the user for this request.
    /// </summary>
    public ClaimsPrincipal? User { get; }
}

internal static class UserAccessorExtensions
{
    public static ClaimsPrincipal EnsureAuthenticated(this IRequestUserAccessor source) => (source.User?.IsAuthenticated() ?? false) ? source.User : throw new UnauthorizedAccessException();
}
