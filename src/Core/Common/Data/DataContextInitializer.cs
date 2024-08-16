using Core.Collections.Data;
using Core.Common.Security;
using Core.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Common.Data;

public static class DataInitializerExtensions
{
    public static async Task SeedDataAsync(this IServiceProvider serviceProvider)
    {
        var initialiser = serviceProvider.GetRequiredService<DataContextInitializer>();
        await initialiser.SeedAsync();
    }
}

internal class DataContextInitializer
{
    private readonly ILogger<DataContextInitializer> _logger;
    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DataContextInitializer(ILogger<DataContextInitializer> logger, DataContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        try
        {
            // Default roles
            var administratorRole = new IdentityRole(KnownRoles.Administrator);

            if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await _roleManager.CreateAsync(administratorRole);
            }

            // Default users
            var administrator = new User { UserName = "admin", Email = "admin@example.com" };

            if (_userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await _userManager.CreateAsync(administrator, "Admin@123!");
                if (!string.IsNullOrWhiteSpace(administratorRole.Name))
                {
                    await _userManager.AddToRolesAsync(administrator, [administratorRole.Name]);
                }
            }

            // Default data
            // Seed, if necessary
            if (!_context.Collections.Any())
            {
                _context.Collections.Add(new Collection
                {
                    Title = "ASP.NET Core Resources",
                    Resources =
                    {
                        new Resource { Title = "ASP.NET Documentation", Notes = "Official docs", LinkToUrl = "https://learn.microsoft.com/en-gb/aspnet/core/?view=aspnetcore-8.0" },
                        new Resource { Title = "ASP.NET Core Blazor Documentation", Notes = "Official docs", LinkToUrl = "https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-8.0" },
                    }
                });

                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

}
