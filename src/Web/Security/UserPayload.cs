using System.Security.Claims;
using Core.Common.Security;

namespace Web.Security;

public class UserPayload(IHttpContextAccessor httpContextAccessor) : IUserPayload
{
    public string? UserId => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}
