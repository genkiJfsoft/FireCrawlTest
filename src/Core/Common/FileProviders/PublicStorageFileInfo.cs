using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;

namespace Core.Common.FileProviders;

public class PublicStorageFileInfo : IFileInfo
{
    private readonly string _requestPath;
    private readonly PhysicalFileInfo _info;

    public PublicStorageFileInfo(string requestPath, PhysicalFileInfo info)
    {
        _requestPath = requestPath;
        _info = info;
    }

    /// <inheritdoc />
    public bool Exists => _info.Exists;

    /// <inheritdoc />
    public long Length => _info.Length;

    /// <inheritdoc />
    public string PhysicalPath => _info.PhysicalPath;

    public string RequestPath => _requestPath;

    /// <inheritdoc />
    public string Name => _info.Name;

    /// <inheritdoc />
    public DateTimeOffset LastModified => _info.LastModified;

    /// <summary>
    /// Always false.
    /// </summary>
    public bool IsDirectory => false;

    /// <inheritdoc />
    public Stream CreateReadStream() => _info.CreateReadStream();
}
