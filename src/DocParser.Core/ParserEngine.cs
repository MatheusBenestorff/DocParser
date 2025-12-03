using System.Text.RegularExpressions;
using System.Text.Json.Nodes;

namespace DocParser.Core;

public class ParserEngine
{
    public JsonObject Execute(string textContent, ExtractionProfile profile)
    {
        var result = new JsonObject();
        Console.WriteLine($"[Engine] Processing profile: {profile.ProfileName}...");

        foreach (var rule in profile.Rules)
        {
            string? extractedValue = null;

            // STRATEGY DECISION
            if (rule.Method == ExtractionMethod.Regex)
            {
                extractedValue = ExtractWithRegex(textContent, rule);
            }
            else if (rule.Method == ExtractionMethod.TextRange)
            {
                extractedValue = ExtractWithTextRegion(textContent, rule);
            }

            // POST-PROCESSING (Cleaning and Typing)
            if (rule.TrimWhitespace && extractedValue != null)
            {
                extractedValue = extractedValue.Trim();
            }

            // TYPE PARSER
            if (extractedValue != null)
            {
                result.Add(rule.TargetField, extractedValue);
            }
            else
            {
                result.Add(rule.TargetField, null);
            }
        }

        return result;
    }

    // REGEX 
    private string? ExtractWithRegex(string text, ExtractionRule rule)
    {
        if (string.IsNullOrEmpty(rule.RegexPattern)) return null;

        var options = rule.CaseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase;
        var match = Regex.Match(text, rule.RegexPattern, options | RegexOptions.Singleline);

        if (match.Success && match.Groups.Count > rule.RegexGroup)
        {
            return match.Groups[rule.RegexGroup].Value;
        }
        return null;
    }

    // TEXT RANGE 
    private string? ExtractWithTextRegion(string text, ExtractionRule rule)
    {
        if (string.IsNullOrEmpty(rule.StartAnchor)) return null;

        var comparison = rule.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

        int startIndex = text.IndexOf(rule.StartAnchor, comparison);
        if (startIndex == -1) return null;

        startIndex += rule.StartAnchor.Length;

        int endIndex;
        if (!string.IsNullOrEmpty(rule.EndAnchor))
        {
            endIndex = text.IndexOf(rule.EndAnchor, startIndex, comparison);
            if (endIndex == -1) return null;
        }
        else
        {
            endIndex = text.Length;
        }

        int length = endIndex - startIndex;
        if (length <= 0) return string.Empty;

        return text.Substring(startIndex, length);
    }
}