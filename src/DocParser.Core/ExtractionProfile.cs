namespace DocParser.Core;


public class ExtractionProfile
{
    public string ProfileName { get; set; } = string.Empty;
    public List<ExtractionRule> Rules { get; set; } = new();
}