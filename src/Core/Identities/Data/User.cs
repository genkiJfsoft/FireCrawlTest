using Core.Common.Data;
using Microsoft.AspNetCore.Identity;

namespace Core.Identities.Data;

public class User : IdentityUser, IDataModel, ITimestampableModel
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
}
