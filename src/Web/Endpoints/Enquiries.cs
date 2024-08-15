using Ardalis.Result.AspNetCore;
using Core.Enquiries.Features;
using MediatR;
using Web.Endpoints.Common;

namespace Web.Endpoints;

public class Enquiries : ApiEndpoint
{
    public override void Map(WebApplication app)
    {
        var group = app.MapApiGroup(this, "enquiries");

        group.MapApiPost("", PostAction);
    }

    public async Task<IResult> PostAction(ISender sender, CreateEnquiry request)
    {
        var result = await sender.Send(request);
        return result.ToMinimalApiResult();
    }
}
