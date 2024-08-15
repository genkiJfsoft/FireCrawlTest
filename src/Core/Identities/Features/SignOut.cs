using Ardalis.Result;
using Core.Identities.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Core.Identities.Features;

public record SignOut : IRequest<Result>
{
    public required string UserName { get; init; }
    public required string Password { get; init; }
    public bool RememberMe { get; init; } = false;
}

internal class SignOutHandler(SignInManager<User> signInManager) : IRequestHandler<SignOut, Result>
{
    public async Task<Result> Handle(SignOut request, CancellationToken cancellationToken)
    {
        await signInManager.SignOutAsync();

        return Result.Success();
    }
}
