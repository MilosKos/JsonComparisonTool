using System.Text.Json;

namespace JsonComparisonTool.Core.Models;

public class ComparisonResult
{
    public List<JsonElement> OnlyInFirst { get; set; } = new();
    public List<JsonElement> OnlyInSecond { get; set; } = new();
    public List<DifferenceDetail> Differences { get; set; } = new();
    public ComparisonSummary Summary { get; set; } = new();
    public DateTime ComparisonTimestamp { get; set; } = DateTime.UtcNow;
}

public class DifferenceDetail
{
    public JsonElement ObjectFromFirst { get; set; }
    public JsonElement ObjectFromSecond { get; set; }
    public List<FieldDifference> FieldDifferences { get; set; } = new();
}

public class FieldDifference
{
    public string FieldPath { get; set; } = string.Empty;
    public string? ValueInFirst { get; set; }
    public string? ValueInSecond { get; set; }
}

public class ComparisonSummary
{
    public int TotalObjectsInFirst { get; set; }
    public int TotalObjectsInSecond { get; set; }
    public int MatchingObjects { get; set; }
    public int OnlyInFirstCount { get; set; }
    public int OnlyInSecondCount { get; set; }
    public int ObjectsWithDifferences { get; set; }
    public string[] ComparedFields { get; set; } = Array.Empty<string>();
}
