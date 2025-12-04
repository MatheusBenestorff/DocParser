using DocParser.Core;
using Xunit;
using System.Collections.Generic;

namespace DocParser.Tests;

public class ParserEngineTests
{
    [Fact]
    public void Should_Extract_Data_Using_Regex_Strategy()
    {
        //ARRANGE
        var inputText = "Project Code: 12345 - Status: Active";
        
        var profile = new ExtractionProfile
        {
            ProfileName = "TestProfile",
            Rules = new List<ExtractionRule>
            {
                new ExtractionRule
                {
                    TargetField = "Code",
                    Method = ExtractionMethod.Regex,
                    RegexPattern = "Project Code:\\s*(\\d+)"
                }
            }
        };

        var engine = new ParserEngine();

        //ACT
        var result = engine.Execute(inputText, profile);

        //ASSERT
        Assert.NotNull(result);
        Assert.True(result.ContainsKey("Code"));
        Assert.Equal("12345", result["Code"]?.ToString());
    }

    [Fact]
    public void Should_Extract_Data_Using_TextRange_Strategy()
    {
        //ARRANGE
        var inputText = @"
        BEGIN HEADER
        System Status: ONLINE
        END HEADER
        ";

        var profile = new ExtractionProfile
        {
            ProfileName = "RegionTest",
            Rules = new List<ExtractionRule>
            {
                new ExtractionRule
                {
                    TargetField = "Status",
                    Method = ExtractionMethod.TextRange,
                    StartAnchor = "System Status:",
                    EndAnchor = "END HEADER",
                    TrimWhitespace = true
                }
            }
        };

        var engine = new ParserEngine();

        //ACT
        var result = engine.Execute(inputText, profile);

        //ASSERT
        Assert.Equal("ONLINE", result["Status"]?.ToString());
    }

    [Fact]
    public void Should_Return_Null_When_Regex_Does_Not_Match()
    {
        //ARRANGE
        var inputText = "No numbers here";
        var profile = new ExtractionProfile
        {
            ProfileName = "FailTest",
            Rules = new List<ExtractionRule>
            {
                new ExtractionRule
                {
                    TargetField = "Number",
                    Method = ExtractionMethod.Regex,
                    RegexPattern = "(\\d+)"
                }
            }
        };

        var engine = new ParserEngine();

        //ACT
        var result = engine.Execute(inputText, profile);

        //ASSERT
        Assert.True(result.ContainsKey("Number"));
        Assert.Null(result["Number"]); 
    }
}