# DocParser

![Build Status](https://github.com/MatheusBenestorff/DocParser/actions/workflows/dotnet.yml/badge.svg)

**A declarative, rule-based engine for extracting structured data from unstructured documents.**

DocParser allows developers to transform unstructured text (from PDFs, Text files, Logs) into clean JSON objects without writing hardcoded parsing logic. Instead of writing complex `if/else` chains in your code, you simply define an **Extraction Profile** in JSON.

## Project Structure

To use DocParser, you must organize your files within the mapped volume (default: `examples/`):

```text
examples/
├── config/          <-- Place your profile.json here
│   └── profile.json
├── input/           <-- Place all PDFs or TXTs you want to process
│   ├── invoice_001.pdf
│   └── invoice_002.pdf
└── output/          <-- The engine will generate the JSON results here
    ├── invoice_001_profile.json
    └── invoice_002_profile.json
```
### Configuration Guide
The extraction logic is defined in a JSON profile. You can choose between two methods:

1. Regex Strategy

Best for structured data like IDs, Dates, and Codes.
```json
{
  "targetField": "ProjectCode",
  "method": "Regex",
  "regexPattern": "Project Number:\\s*(\\d+)",
}
```
2. Text Range Strategy

Best for extracting blocks of text or descriptions where Regex is too complex.
```json
{
  "targetField": "DescriptionBlock",
  "method": "TextRegion",
  "startAnchor": "DESCRIPTION:",
  "endAnchor": "TECHNICAL DATA:",
  "trimWhitespace": true
}
```

### Full Profile Example (`examples/config/profile.json`)
```json
{
  "profileName": "Engineering_Spec_V1",
  "rules": [
    {
      "targetField": "ProjectCode",
      "method": "Regex",
      "regexPattern": "Project Number:\\s*(\\d+)",
    },
    {
      "targetField": "DescriptionBlock",
      "method": "TextRange",
      "startAnchor": "DESCRIPTION:",
      "endAnchor": "TECHNICAL DATA:",
      "trimWhitespace": true
    },
  ]
}
```

### Architecture

- **DocParser.Core:** The extraction engine logic (Standard .NET 8 Library).
- **DocParser.CLI:** A command-line interface that implements the Batch Processing logic and File I/O.

### Quick Start (Docker)

You don't need the .NET SDK installed. You can run the engine using Docker Compose.

1.  Setup: Ensure your examples/config has a profile and examples/input has your documents.

2.  Run the engine:

    ```bash
    docker compose up
    ```


