using System.Security.Claims;

namespace Core.Common.Security;

public static class ClaimsPrincipalExtensions
{
    public static bool IsAuthenticated(this ClaimsPrincipal? c) => c?.Identity?.IsAuthenticated ?? false;

    public static string? GetUserId(this ClaimsPrincipal c) => c.FindFirstValue(ClaimTypes.NameIdentifier);

    public static string GetRequiredUserId(this ClaimsPrincipal c) => c.GetUserId() ?? throw new UnauthorizedAccessException();

    public static string[] GetRoles(this ClaimsPrincipal c) => c.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToArray();
}
