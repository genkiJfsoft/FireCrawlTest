using Core.Common.Security;
using System.Security.Claims;

namespace Web.Services;

public class HttpContextUserAccessor(IHttpContextAccessor httpContextAccessor) : IRequestUserAccessor
{
    public ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;
}
