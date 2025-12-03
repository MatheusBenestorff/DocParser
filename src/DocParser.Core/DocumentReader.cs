using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace DocParser.Core;

public static class DocumentReader
{
    public static string ReadContent(string filePath)
    {
        if (!File.Exists(filePath)) 
            throw new FileNotFoundException($"File not found: {filePath}");

        string extension = Path.GetExtension(filePath).ToLower();

        return extension switch
        {
            ".pdf" => ExtractTextFromPdf(filePath),
            ".txt" => File.ReadAllText(filePath),
            ".json" => File.ReadAllText(filePath),
            _ => throw new NotSupportedException($"File extension '{extension}' is not supported.")
        };
    }

    private static string ExtractTextFromPdf(string filePath)
    {
        Console.WriteLine($"[Reader] Extracting text from PDF: {Path.GetFileName(filePath)}");
        
        using var document = PdfDocument.Open(filePath);
        var textContent = new System.Text.StringBuilder();

        foreach (var page in document.GetPages())
        {
            textContent.AppendLine(page.Text);
        }

        return textContent.ToString();
    }
}