# DocParser

**A declarative, rule-based engine for extracting structured data from unstructured documents.**

DocParser allows developers to transform unstructured text (from PDFs, Text files, Logs) into clean JSON objects without writing hardcoded parsing logic. Instead of writing complex `if/else` chains in your code, you simply define an **Extraction Profile** in JSON.

## Concept

The core philosophy is **Infrastructure as Code** applied to **Data Extraction**. 
By decoupling the extraction rules from the core logic, DocParser enables:
- **Portability:** Rules can be versioned and shared.
- **Maintainability:** Support new document formats without recompiling the application.
- **Scalability:** Stateless engine, ready for Cloud Native environments.

## How it Works

1. **Input:** Raw text from a document (e.g., an Engineering Spec PDF or an Invoice).
2. **Configuration:** A JSON file defining Regex patterns and data types.
3. **Output:** A sanitized, typed JSON object ready for your Database or API.

### Example Configuration (`profile.json`)
```json
{
  "profileName": "Engineering_Spec_V1",
  "rules": [
    {
      "targetField": "ProjectCode",
      "regexPattern": "Project Number:\\s*(\\d+)",
      "type": "Integer"
    },
    {
      "targetField": "TotalValue",
      "regexPattern": "Total:\\s*\\$([\\d\\.]+)",
      "type": "Decimal"
    }
  ]
}
```

### Architecture

- **DocParser.Core:** The regex processing engine (Standard .NET 8 Library).
- **DocParser.CLI:** A command-line interface to demonstrate the engine capabilities.

### Running with Docker (No .NET SDK required)

You can run DocParser immediately using Docker Compose. The project maps the ./examples folder to the container, allowing you to test your own rules instantly.

1.  Place your text files and JSON profiles in the examples/ folder.

2.  Run the engine:

    ```bash
    docker compose up --build
    ```


