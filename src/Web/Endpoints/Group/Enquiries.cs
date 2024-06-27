using Ardalis.Result.AspNetCore;
using Core.Features.Enquiries;
using MediatR;
using Web.Endpoints.Shared;

namespace Web.Endpoints.Group;

public class Enquiries : EndpointGroup
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(PostAction);
    }

    public async Task<IResult> PostAction(ISender sender, CreateEnquiry request)
    {
        var result = await sender.Send(request);
        return result.ToMinimalApiResult();
    }
}
