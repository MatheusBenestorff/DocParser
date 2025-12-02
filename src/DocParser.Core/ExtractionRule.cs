namespace DocParser.Core;

public class ExtractionRule
{
    public string TargetField { get; set; } = string.Empty;
    public string RegexPattern { get; set; } = string.Empty;
    public string Type { get; set; } = "String"; // String, Integer, Decimal, Date
    public int GroupIndex { get; set; } = 1;
}