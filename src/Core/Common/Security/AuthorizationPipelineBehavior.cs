using System.Reflection;
using Core.Common.Data;
using Core.Common.Exceptions;
using Core.Identities.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Core.Common.Security;

public class AuthorizationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICurrentUser _currentUser;
    private readonly UserManager<User> _userManager;
    private readonly IUserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;

    public AuthorizationPipelineBehavior(
        ICurrentUser currentUser,
        UserManager<User> userManager,
        IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService)
    {
        _currentUser = currentUser;
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        if (authorizeAttributes.Any())
        {
            // Must be authenticated user
            if (_currentUser.UserId == null)
            {
                throw new UnauthorizedAccessException();
            }

            var user = await _userManager.FindByIdAsync(_currentUser.UserId) ?? throw new UnauthorizedAccessException();

            await HandleRoleBasedAuthorizationsAsync(user, authorizeAttributes);
            await HandlePolicyBasedAuthorizationsAsync(user, authorizeAttributes);
        }

        // User is authorized / authorization not required
        return await next();
    }
    private async Task HandleRoleBasedAuthorizationsAsync(User user, IEnumerable<AuthorizeAttribute> authorizeAttributes)
    {
        var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));

        if (authorizeAttributesWithRoles.Any())
        {
            var authorized = false;

            foreach (var roles in authorizeAttributesWithRoles.Where(a => a.Roles != null).Select(a => a.Roles!.Split(',')))
            {
                foreach (var role in roles)
                {
                    var isInRole = user != null && await _userManager.IsInRoleAsync(user, role.Trim());
                    if (isInRole)
                    {
                        authorized = true;
                        break;
                    }
                }
            }

            // Must be a member of at least one role in roles
            if (!authorized)
            {
                throw new ForbiddenAccessException();
            }
        }
    }

    private async Task HandlePolicyBasedAuthorizationsAsync(User user, IEnumerable<AuthorizeAttribute> authorizeAttributes)
    {
        var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));
        if (authorizeAttributesWithPolicies.Any())
        {
            foreach (var policy in authorizeAttributesWithPolicies.Where(a => a.Policy != null).Select(a => a.Policy!))
            {
                var principal = await _userClaimsPrincipalFactory.CreateAsync(user!);
                var result = await _authorizationService.AuthorizeAsync(principal, policy);

                var authorized = result.Succeeded;

                if (!authorized)
                {
                    throw new ForbiddenAccessException();
                }
            }
        }
    }
}
