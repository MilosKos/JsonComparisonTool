using JsonComparisonTool.Shared.Services;
using System.Text;

namespace JsonComparisonTool.Web.Services;

public class WebFileService : IFileService
{
    public async Task<string> ReadFileAsTextAsync(Stream fileStream, string fileName)
    {
        Console.WriteLine($"WebFileService.ReadFileAsTextAsync called for: {fileName}");
        Console.WriteLine($"Stream CanRead: {fileStream.CanRead}, Length: {fileStream.Length}, Position: {fileStream.Position}");
        
        using var reader = new StreamReader(fileStream);
        var content = await reader.ReadToEndAsync();
        
        Console.WriteLine($"File content read, length: {content?.Length ?? 0}");
        if (content?.Length > 0)
        {
            Console.WriteLine($"First 100 chars: {content.Substring(0, Math.Min(100, content.Length))}");
        }
        
        return content;
    }

    public async Task<byte[]> CreateDownloadFileAsync(string content, string fileName)
    {
        await Task.CompletedTask; // For async consistency
        return Encoding.UTF8.GetBytes(content);
    }

    public string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".json" => "application/json",
            ".csv" => "text/csv",
            ".html" => "text/html",
            ".txt" => "text/plain",
            _ => "application/octet-stream"
        };
    }
}
