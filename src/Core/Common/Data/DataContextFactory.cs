using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Common.Data;

internal class DataContextFactory(IServiceProvider serviceProvider) : IDbContextFactory<DataContext>
{
    public DataContext CreateDbContext()
    {
        return serviceProvider == null
            ? throw new InvalidOperationException("IServiceProvider must be configured.")
            : ActivatorUtilities.CreateInstance<DataContext>(serviceProvider);
    }
}
