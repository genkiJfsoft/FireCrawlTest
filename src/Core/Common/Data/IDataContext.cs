using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Collections.Data;
using Microsoft.EntityFrameworkCore;

namespace Core.Common.Data;

/// <summary>
/// Database context for Entity Framework
/// </summary>
internal interface IDataContext
{
    public DbSet<Resource> Resources { get; set; }
    public DbSet<Collection> Collections { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
