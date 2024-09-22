using Ardalis.GuardClauses;
using Core.Common.Security;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Web.Endpoints.Common;

public static class ApiEndpointBuilderExtensions
{
    public static IEndpointRouteBuilder MapApiGet(this IEndpointRouteBuilder builder, [StringSyntax("Route")] string pattern, Delegate handler)
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapGet(pattern, handler)
            .WithName(handler.Method.Name);

        return builder;
    }

    public static IEndpointRouteBuilder MapApiPost(this IEndpointRouteBuilder builder, [StringSyntax("Route")] string pattern, Delegate handler)
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapPost(pattern, handler)
            .WithName(handler.Method.Name);

        return builder;
    }

    public static IEndpointRouteBuilder MapApiPut(this IEndpointRouteBuilder builder, [StringSyntax("Route")] string pattern, Delegate handler)
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapPut(pattern, handler)
            .WithName(handler.Method.Name);

        return builder;
    }

    public static IEndpointRouteBuilder MapApiDelete(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern)
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapDelete(pattern, handler)
            .WithName(handler.Method.Name);

        return builder;
    }
    public static RouteGroupBuilder MapApiGroup(this WebApplication app, ApiEndpoint endpoint, string? name = null)
    {
        name = string.IsNullOrEmpty(name) ? endpoint.GetType().Name : name;

        return app
            .MapGroup($"/api/{name}")
            .WithGroupName(name)
            .WithTags(name)
            .WithOpenApi();
    }

    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        var apiEndpointTypes = Assembly.GetExecutingAssembly().GetExportedTypes()
            .Where(t => t.IsSubclassOf(typeof(ApiEndpoint)));

        foreach (var type in apiEndpointTypes)
        {
            if (Activator.CreateInstance(type) is ApiEndpoint instance)
            {
                instance.Map(app);
            }
        }

        return app;
    }
}
