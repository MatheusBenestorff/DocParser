namespace DocParser.Core;

public class ExtractionRule
{
    // Basic Metadata
    public string TargetField { get; set; } = string.Empty;
    public string TargetType { get; set; } = "String"; // Integer, Decimal, Date, String

    // Strategy Definition
    public ExtractionMethod Method { get; set; } = ExtractionMethod.Regex;

    // Configuration for Regex
    public string? RegexPattern { get; set; }
    public int RegexGroup { get; set; } = 1;

    // Configuration for Text Range
    public string? StartAnchor { get; set; }
    public string? EndAnchor { get; set; }

    // Global Settings
    public bool TrimWhitespace { get; set; } = true;
    public bool CaseSensitive { get; set; } = false;
}