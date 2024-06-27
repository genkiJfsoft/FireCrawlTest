using Ardalis.Result.AspNetCore;
using Core.Features.Collections;
using MediatR;
using Web.Endpoints.Shared;

namespace Web.Endpoints.Group;

public class Collections : EndpointGroup
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetCollectionsAction)
            .MapPost(CreateCollectionAction);
    }

    public async Task<IResult> GetCollectionsAction(ISender sender)
    {
        var result = await sender.Send(new GetCollections());
        return result.ToMinimalApiResult();
    }

    public async Task<IResult> CreateCollectionAction(ISender sender, CreateCollection request)
    {
        var result = await sender.Send(request);
        return result.ToMinimalApiResult();
    }
}
