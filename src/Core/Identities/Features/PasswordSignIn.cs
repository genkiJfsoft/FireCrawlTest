using Ardalis.Result;
using Core.Identities.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Core.Identities.Features;

public record PasswordSignIn : IRequest<Result>
{
    public required string UserName { get; init; }
    public required string Password { get; init; }
    public bool RememberMe { get; init; } = false;
}

internal class PasswordSignInHandler(SignInManager<User> signInManager) : IRequestHandler<PasswordSignIn, Result>
{
    public async Task<Result> Handle(PasswordSignIn request, CancellationToken cancellationToken)
    {
        // TODO: add validation

        var result = await signInManager.PasswordSignInAsync(request.UserName, request.Password, request.RememberMe, false);

        return result.Succeeded ? Result.Success() : Result.Error();
    }
}
