using Core.Common.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Logging;

namespace Core.Common.FileProviders;

public class PublicStorageProvider
{
    private readonly ILogger<PublicStorageProvider> _logger;

    private readonly PublicStorageConfiguration _config;

    private readonly DirectoryInfo _directory;

    private readonly PhysicalFileProvider _fileProvider;

    public readonly StaticFileOptions StaticFileOptions;

    public PublicStorageProvider(ILogger<PublicStorageProvider> logger, IConfiguration configuration)
    {
        _logger = logger;
        _config = configuration.GetSection<PublicStorageConfiguration>(PublicStorageConfiguration.Key);

        _directory = new DirectoryInfo(_config.GetFullPath());

        if (!_directory.Exists) _directory.Create();

        _fileProvider = new PhysicalFileProvider(_config.GetFullPath());

        StaticFileOptions = new StaticFileOptions()
        {
            FileProvider = _fileProvider,
            RequestPath = _config.RequestPath,
        };
    }

    public IFileInfo GetFileInfo(string subPath)
    {
        var info = _fileProvider.GetFileInfo(subPath);

        return info is PhysicalFileInfo fileInfo ? new PublicStorageFileInfo(RequestPath(subPath), fileInfo) : info;
    }

    public string RequestPath() => _config.RequestPath;

    public string RequestPath(string subPath) => Path.Combine(_config.RequestPath, subPath);

    public DirectoryInfo CreateSubdirectory(string subPath)
    {
        try
        {
            return _directory.CreateSubdirectory(subPath);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating Public Storage subPath \"{subPath}\": {ex}", subPath, ex);

            throw;
        }
    }

    public async Task<IFileInfo> WriteFileBytesAsync(string subPath, byte[] bytes, CancellationToken cancellationToken)
    {
        try
        {
            var dirName = Path.GetDirectoryName(subPath);

            if (!string.IsNullOrEmpty(dirName))
            {
                CreateSubdirectory(dirName);
            }

            await File.WriteAllBytesAsync(Path.Combine(_directory.FullName, subPath), bytes, cancellationToken);

            return GetFileInfo(subPath);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error writing file \"{subPath}\" to Public Storage: {ex}", subPath, ex);

            throw;
        }
    }
}
