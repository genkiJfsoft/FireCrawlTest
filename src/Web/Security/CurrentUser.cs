using System.Security.Claims;
using Core.Common.Security;

namespace Web.Security;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public string? UserId => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}
