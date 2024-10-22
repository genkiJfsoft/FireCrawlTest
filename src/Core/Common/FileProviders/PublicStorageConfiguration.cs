namespace Core.Common.FileProviders;

public record PublicStorageConfiguration
{
    public const string Key = "PublicStorage";

    public required string Path { get; init; }

    public required string RequestPath { get; init; }

    public string GetFullPath() => System.IO.Path.Combine(Directory.GetCurrentDirectory(), Path);
}
