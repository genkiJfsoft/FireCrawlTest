using Ardalis.Result.AspNetCore;
using Core.Collections.Features;
using MediatR;
using Web.Endpoints.Common;

namespace Web.Endpoints;

public class Collections : ApiEndpoint
{
    public override void Map(WebApplication app)
    {
        var group = app.MapApiGroup(this, "collections");

        group.MapApiGet("", GetCollectionsAction);
        group.MapApiPost("", CreateCollectionAction);
    }

    public async Task<IResult> GetCollectionsAction(ISender sender)
    {
        var result = await sender.Send(new GetCollections() { PerPage = 0 });
        return result.ToMinimalApiResult();
    }

    public async Task<IResult> CreateCollectionAction(ISender sender, CreateCollection request)
    {
        var result = await sender.Send(request);
        return result.ToMinimalApiResult();
    }
}
