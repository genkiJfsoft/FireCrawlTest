using System.Reflection;
using Core.Collections.Data;
using Core.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Common.Data;

/// <summary>
/// Database context for Entity Framework
/// </summary>
internal class DataContext : IdentityDbContext<User>, IDataContext
{
    public const string ConnectionStringName = "DefaultConnection";

    public DbSet<Resource> Resources { get; set; }
    public DbSet<Collection> Collections { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected DataContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
