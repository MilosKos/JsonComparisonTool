namespace JsonComparisonTool.Shared.Services;

public interface IFileService
{
    Task<string> ReadFileAsTextAsync(Stream fileStream, string fileName);
    Task<byte[]> CreateDownloadFileAsync(string content, string fileName);
    string GetContentType(string fileName);
}
