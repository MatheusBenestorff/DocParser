using System.Text.RegularExpressions;
using System.Text.Json.Nodes;

namespace DocParser.Core;

public class ParserEngine
{
    public JsonObject Execute(string textContent, ExtractionProfile profile)
    {
        var result = new JsonObject();
        Console.WriteLine($"[Core] Processing profile: {profile.ProfileName}...");
        
        // Logic will be implemented here
        return result;
    }
}