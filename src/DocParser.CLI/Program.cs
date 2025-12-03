using System.Text.Json;
using System.Text.Json.Serialization;
using DocParser.Core;

Console.WriteLine("========================================");
Console.WriteLine("   DocParser CLI - Extraction Engine    ");
Console.WriteLine("========================================");

string profilePath = args.Length > 0 ? args[0] : Path.Combine("examples", "profile.json");
string inputPath = args.Length > 1 ? args[1] : Path.Combine("examples", "input.txt");

// File Validation
if (!File.Exists(profilePath))
{
    Console.Error.WriteLine($"[Error] Profile file not found at: {profilePath}");
    Console.Error.WriteLine("Please create the file or check the path.");
    Environment.Exit(1);
}

if (!File.Exists(inputPath))
{
    Console.Error.WriteLine($"[Error] Input file not found at: {inputPath}");
    Environment.Exit(1);
}

try
{
    Console.WriteLine($"-> Loading Profile: {profilePath}");
    Console.WriteLine($"-> Loading Content: {inputPath}");

    // Reading the Files
    string jsonProfileContent = File.ReadAllText(profilePath);
    string documentContent = File.ReadAllText(inputPath);

    // JSON configuration
    var jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() },
        WriteIndented = true
    };

    // Profile Deserialization
    var profile = JsonSerializer.Deserialize<ExtractionProfile>(jsonProfileContent, jsonOptions);

    if (profile == null || profile.Rules == null || !profile.Rules.Any())
    {
        Console.Error.WriteLine("[Error] The profile is empty or invalid JSON.");
        Environment.Exit(1);
    }

    // Engine Execution
    var engine = new ParserEngine();
    Console.WriteLine($"-> Starting extraction for '{profile.ProfileName}'");
    var resultJson = engine.Execute(documentContent, profile);


    // Final Result
    Console.WriteLine("----------------------------------------");
    Console.WriteLine("EXTRACTION RESULT:");
    Console.WriteLine(resultJson.ToJsonString(jsonOptions));
    Console.WriteLine("----------------------------------------");

}
catch (JsonException ex)
{
    Console.Error.WriteLine($"[Fatal] JSON Error in configuration: {ex.Message}");
}
catch (Exception ex)
{
    Console.Error.WriteLine($"[Fatal] Unexpected error: {ex.Message}");
    Console.Error.WriteLine(ex.StackTrace);
}