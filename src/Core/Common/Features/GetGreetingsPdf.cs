using Ardalis.Result;
using Core.Common.Assets;
using Core.Common.FileProviders;
using Core.Common.Pdf;
using Core.Common.Security;
using Core.Identity.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Core.Common.Features;

[Authorize]
public record GetGreetingsPdf() : IRequest<Result<PublicStorageFileInfo>>
{
}

internal class GetGreetingsPdfHandler(UserManager<User> userManager, IRequestUserAccessor userAccessor, PublicStorageProvider publicStorage, IPdfConverter pdfConverter) : IRequestHandler<GetGreetingsPdf, Result<PublicStorageFileInfo>>
{
    public async Task<Result<PublicStorageFileInfo>> Handle(GetGreetingsPdf request, CancellationToken cancellationToken)
    {
        try
        {
            var user = (await userManager.GetUserAsync(userAccessor.EnsureAuthenticated()))!;

            var filePath = Path.Combine("Greetings", $"{user.UserName}.pdf");
            var fileInfo = publicStorage.GetFileInfo(filePath);

            if (!fileInfo.Exists)
            {
                var template = await AssetsHelper.ReadEmbeddedTemplateFileAsync("Common/Assets/GreetingsPdfTemplate.htm");
                var html = await template.RenderAsync(new
                {
                    Name = user.UserName,
                });

                var outputBytes = await pdfConverter.FromHtmlAsync(html);

                fileInfo = await publicStorage.WriteFileBytesAsync(filePath, outputBytes, cancellationToken);
            }

            return fileInfo is PublicStorageFileInfo info && fileInfo.Exists ? Result<PublicStorageFileInfo>.Success(info) : Result.Conflict();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}

