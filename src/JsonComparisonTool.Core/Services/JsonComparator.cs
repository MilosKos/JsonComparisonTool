using System.Text.Json;
using JsonComparisonTool.Core.Models;

namespace JsonComparisonTool.Core.Services;

public class JsonComparator
{
    private readonly ComparisonConfig _config;

    public JsonComparator(ComparisonConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public ComparisonResult CompareJsonArrays(string json1, string json2)
    {
        try
        {
            var array1 = JsonSerializer.Deserialize<JsonElement[]>(json1) ?? Array.Empty<JsonElement>();
            var array2 = JsonSerializer.Deserialize<JsonElement[]>(json2) ?? Array.Empty<JsonElement>();

            return CompareArrays(array1, array2);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Invalid JSON format: {ex.Message}", ex);
        }
    }

    private ComparisonResult CompareArrays(JsonElement[] array1, JsonElement[] array2)
    {
        var result = new ComparisonResult();
        
        // Initialize summary
        result.Summary.TotalObjectsInFirst = array1.Length;
        result.Summary.TotalObjectsInSecond = array2.Length;
        result.Summary.ComparedFields = _config.ComparisonFields;

        var matched1 = new HashSet<int>();
        var matched2 = new HashSet<int>();

        // Find matches and differences
        for (int i = 0; i < array1.Length; i++)
        {
            var bestMatch = FindBestMatch(array1[i], array2, matched2);
            
            if (bestMatch.HasValue)
            {
                var (matchIndex, differences) = bestMatch.Value;
                matched1.Add(i);
                matched2.Add(matchIndex);

                if (differences.Count > 0)
                {
                    result.Differences.Add(new DifferenceDetail
                    {
                        ObjectFromFirst = array1[i],
                        ObjectFromSecond = array2[matchIndex],
                        FieldDifferences = differences
                    });
                    result.Summary.ObjectsWithDifferences++;
                }
                else
                {
                    result.Summary.MatchingObjects++;
                }
            }
        }

        // Add unmatched objects
        for (int i = 0; i < array1.Length; i++)
        {
            if (!matched1.Contains(i))
            {
                result.OnlyInFirst.Add(array1[i]);
                result.Summary.OnlyInFirstCount++;
            }
        }

        for (int j = 0; j < array2.Length; j++)
        {
            if (!matched2.Contains(j))
            {
                result.OnlyInSecond.Add(array2[j]);
                result.Summary.OnlyInSecondCount++;
            }
        }

        return result;
    }

    private (int Index, List<FieldDifference> Differences)? FindBestMatch(
        JsonElement obj1, 
        JsonElement[] array2, 
        HashSet<int> excludeIndices)
    {
        for (int j = 0; j < array2.Length; j++)
        {
            if (excludeIndices.Contains(j)) continue;

            var differences = CompareObjects(obj1, array2[j]);
            
            // If all comparison fields match exactly, it's a match
            if (differences.Count == 0 || !differences.Any(d => _config.ComparisonFields.Contains(d.FieldPath)))
            {
                return (j, differences);
            }
        }

        return null;
    }

    private List<FieldDifference> CompareObjects(JsonElement obj1, JsonElement obj2)
    {
        var differences = new List<FieldDifference>();

        if (_config.ComparisonFields.Length == 0)
        {
            // Compare all fields if no specific fields configured
            CompareAllFields(obj1, obj2, "", differences);
        }
        else
        {
            // Compare only specified fields
            foreach (var field in _config.ComparisonFields)
            {
                CompareSpecificField(obj1, obj2, field, differences);
            }
        }

        return differences;
    }

    private void CompareSpecificField(JsonElement obj1, JsonElement obj2, string fieldPath, List<FieldDifference> differences)
    {
        var value1 = GetNestedValue(obj1, fieldPath);
        var value2 = GetNestedValue(obj2, fieldPath);

        if (!ValuesEqual(value1, value2))
        {
            differences.Add(new FieldDifference
            {
                FieldPath = fieldPath,
                ValueInFirst = value1?.ToString(),
                ValueInSecond = value2?.ToString()
            });
        }
    }

    private void CompareAllFields(JsonElement obj1, JsonElement obj2, string basePath, List<FieldDifference> differences)
    {
        if (obj1.ValueKind != obj2.ValueKind)
        {
            differences.Add(new FieldDifference
            {
                FieldPath = basePath,
                ValueInFirst = obj1.ToString(),
                ValueInSecond = obj2.ToString()
            });
            return;
        }

        if (obj1.ValueKind == JsonValueKind.Object)
        {
            var props1 = obj1.EnumerateObject().ToDictionary(p => p.Name, p => p.Value);
            var props2 = obj2.EnumerateObject().ToDictionary(p => p.Name, p => p.Value);

            var allKeys = props1.Keys.Union(props2.Keys);

            foreach (var key in allKeys)
            {
                var currentPath = string.IsNullOrEmpty(basePath) ? key : $"{basePath}.{key}";
                
                if (!props1.ContainsKey(key))
                {
                    differences.Add(new FieldDifference
                    {
                        FieldPath = currentPath,
                        ValueInFirst = null,
                        ValueInSecond = props2[key].ToString()
                    });
                }
                else if (!props2.ContainsKey(key))
                {
                    differences.Add(new FieldDifference
                    {
                        FieldPath = currentPath,
                        ValueInFirst = props1[key].ToString(),
                        ValueInSecond = null
                    });
                }
                else
                {
                    CompareAllFields(props1[key], props2[key], currentPath, differences);
                }
            }
        }
        else if (!ValuesEqual(obj1, obj2))
        {
            differences.Add(new FieldDifference
            {
                FieldPath = basePath,
                ValueInFirst = obj1.ToString(),
                ValueInSecond = obj2.ToString()
            });
        }
    }

    private JsonElement? GetNestedValue(JsonElement obj, string path)
    {
        var parts = path.Split('.');
        var current = obj;

        foreach (var part in parts)
        {
            if (current.ValueKind != JsonValueKind.Object || !current.TryGetProperty(part, out current))
            {
                return null;
            }
        }

        return current;
    }

    private bool ValuesEqual(JsonElement? value1, JsonElement? value2)
    {
        if (value1 == null && value2 == null) return true;
        if (value1 == null || value2 == null) return false;

        var str1 = value1.Value.ToString();
        var str2 = value2.Value.ToString();

        return _config.CaseSensitive 
            ? str1 == str2 
            : string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
    }
}
