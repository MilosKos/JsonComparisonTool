namespace JsonComparisonTool.Core.Models;

public class ComparisonConfig
{
    public string[] ComparisonFields { get; set; } = Array.Empty<string>();
    public bool CaseSensitive { get; set; } = true;
    public bool IgnoreArrayOrder { get; set; } = false;
    public bool IgnoreExtraFields { get; set; } = false;
    public string OutputFormat { get; set; } = "json"; // json, csv, html
}
