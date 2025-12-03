using System.Text.Json;
using System.Text.Json.Serialization;
using DocParser.Core;

Console.WriteLine("========================================");
Console.WriteLine("   DocParser CLI - Extraction Engine    ");
Console.WriteLine("========================================");

string rootFolder = Environment.GetEnvironmentVariable("INPUT_FOLDER") ?? "examples";
string configFolder = Path.Combine(rootFolder, "config");
string inputsFolder = Path.Combine(rootFolder, "input");
string outputsFolder = Path.Combine(rootFolder, "output");


// File Validation
if (!Directory.Exists(configFolder) || !Directory.Exists(inputsFolder))
{
    Console.Error.WriteLine($"[Error] Missing required folders in root: {rootFolder}");
    Console.Error.WriteLine("Ensure you have '/config' and '/input' folders.");
    Environment.Exit(1);
}


string profilePath = args.Length > 0 ? args[0] : Path.Combine(configFolder, "profile.json");

if (!File.Exists(profilePath))
{
    Console.Error.WriteLine($"[Error] Profile file not found at: {profilePath}");
    Console.Error.WriteLine("Please create the file or check the path.");
    Environment.Exit(1);
}

ExtractionProfile? profile = null;

var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    Converters = { new JsonStringEnumConverter() },
    WriteIndented = true
};

try 
{
    // Profile Deserialization
    Console.WriteLine($"[1/3] Loading Configuration Profile...");
    string jsonContent = File.ReadAllText(profilePath);
    profile = JsonSerializer.Deserialize<ExtractionProfile>(jsonContent, jsonOptions);

    if (profile == null || string.IsNullOrWhiteSpace(profile.ProfileName))
        throw new Exception("Invalid Profile JSON.");

    Console.WriteLine($"      -> Profile Loaded: {profile.ProfileName} ({profile.Rules.Count} rules)");
}
catch (Exception ex)
{
    Console.Error.WriteLine($"[Fatal] Failed to load profile: {ex.Message}");
    Environment.Exit(1);
}

// Batch Processing
var engine = new ParserEngine();
var inputFiles = Directory.GetFiles(inputsFolder); 
var supportedExtensions = new[] { ".pdf", ".txt", ".json" };

Console.WriteLine($"[2/3] Found {inputFiles.Length} files in input folder.");

if (!Directory.Exists(outputsFolder)) Directory.CreateDirectory(outputsFolder);

int successCount = 0;
int errorCount = 0;

Console.WriteLine($"[3/3] Starting Batch Processing...");
Console.WriteLine("--------------------------------------------------");


foreach (var filePath in inputFiles)
{
    string fileName = Path.GetFileName(filePath);
    string extension = Path.GetExtension(filePath).ToLower();

    if (!supportedExtensions.Contains(extension))
    {
        Console.WriteLine($"[SKIP] Ignoring unsupported file: {fileName}");
        continue;
    }

    try
    {
        Console.Write($" -> Processing: {fileName}... ");
        
        // Reading the Files
        string content = DocumentReader.ReadContent(filePath);

        // Engine Execution
        var resultJson = engine.Execute(content, profile);

        // Final Result
        string safeFileName = Path.GetFileNameWithoutExtension(fileName);
        string outputFileName = $"{safeFileName}_{profile.ProfileName}.json";
        string outputPath = Path.Combine(outputsFolder, outputFileName);

        File.WriteAllText(outputPath, resultJson.ToJsonString(jsonOptions));
        
        Console.WriteLine("OK");
        successCount++;
    }
    catch (Exception ex)
    {
        Console.WriteLine("ERROR");
        Console.Error.WriteLine($"    -> [Fail] {ex.Message}");
        errorCount++;
    }
}

Console.WriteLine("--------------------------------------------------");
Console.WriteLine($"Batch Completed.");
Console.WriteLine($"Success: {successCount} | Failed: {errorCount}");
Console.WriteLine($"Outputs saved to: {outputsFolder}");

