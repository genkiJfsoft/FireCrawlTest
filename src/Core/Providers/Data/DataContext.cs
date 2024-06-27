using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Providers.Data;

internal class DataContext : DbContext
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
