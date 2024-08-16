using Core;
using Core.Common.Data;
using Core.Common.Security;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;
using Web.Components;
using Web.Components.Account;
using Web.Endpoints.Common;
using Web.Security;

Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateBootstrapLogger();

Log.Information("Starting up!");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddCascadingAuthenticationState();
    builder.Services.AddScoped<IdentityUserAccessor>();
    builder.Services.AddScoped<IdentityRedirectManager>();
    builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<IUserPayload, UserPayload>();

    // Register Logger
    builder.Services.AddSerilog((s, c) => c
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(s)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    builder.Services.AddApplicationCore(builder.Configuration, builder.Environment.IsDevelopment());

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Account/Login";
    });

    builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddBlazorBootstrap();

    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        // Seed data
        using var scope = app.Services.CreateScope();
        await scope.ServiceProvider.SeedDataAsync();
    }
    else
    {
        // Configure the HTTP request pipeline.

        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseAntiforgery();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    // Add additional endpoints required by the Identity /Account Razor components.
    app.MapAdditionalIdentityEndpoints();

    app.MapApiEndpoints();

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException && ex.Source != "Microsoft.EntityFrameworkCore.Design")
{
    Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
}
finally
{
    Log.CloseAndFlush();
}
