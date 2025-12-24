using System;
using System.IO;
using System.Text;
using Xunit;

namespace kubec_cmd.Tests;

public class DirHelperTests
{
    private readonly TextWriter _originalOut;

    public DirHelperTests()
    {
        _originalOut = Console.Out;
    }

    [Fact]
    public void PrintInstructions_OutputsAsciiArt()
    {
        // Arrange
        Console.OutputEncoding = Encoding.UTF8;
        using var output = new StringWriter();
        Console.SetOut(output);

        try
        {
            // Act
            DirHelper.PrintInstructions();

            // Assert
            var outputText = output.ToString();
            Assert.Contains("Kubec-cmd", outputText);
            Assert.Contains("===", outputText);
        }
        finally
        {
            Console.SetOut(_originalOut);
        }
    }

    [Fact]
    public void PrintInstructions_ContainsVersionInfo()
    {
        // Arrange
        Console.OutputEncoding = Encoding.UTF8;
        using var output = new StringWriter();
        Console.SetOut(output);

        try
        {
            // Act
            DirHelper.PrintInstructions();

            // Assert
            var outputText = output.ToString();
            Assert.Contains("Formula", outputText);
        }
        finally
        {
            Console.SetOut(_originalOut);
        }
    }

    [Fact]
    public void PrintInstructions_ContainsUsageInstructions()
    {
        // Arrange
        Console.OutputEncoding = Encoding.UTF8;
        using var output = new StringWriter();
        Console.SetOut(output);

        try
        {
            // Act
            DirHelper.PrintInstructions();

            // Assert
            var outputText = output.ToString();
            Assert.Contains("Target file", outputText);
            Assert.Contains("kubec-cmd -t", outputText);
            Assert.Contains("--list", outputText);
            Assert.Contains("--clean", outputText);
        }
        finally
        {
            Console.SetOut(_originalOut);
        }
    }

    [Fact]
    public void PrintInstructions_ContainsGithubLink()
    {
        // Arrange
        Console.OutputEncoding = Encoding.UTF8;
        using var output = new StringWriter();
        Console.SetOut(output);

        try
        {
            // Act
            DirHelper.PrintInstructions();

            // Assert
            var outputText = output.ToString();
            Assert.Contains("github.com", outputText);
        }
        finally
        {
            Console.SetOut(_originalOut);
        }
    }

    [Fact]
    public void PrintInstructions_ContainsAuthorInfo()
    {
        // Arrange
        Console.OutputEncoding = Encoding.UTF8;
        using var output = new StringWriter();
        Console.SetOut(output);

        try
        {
            // Act
            DirHelper.PrintInstructions();

            // Assert
            var outputText = output.ToString();
            Assert.Contains("Eddy Wister", outputText);
        }
        finally
        {
            Console.SetOut(_originalOut);
        }
    }

    [Fact]
    public void PrintInstructions_ContainsUnicodeSymbols()
    {
        // Arrange
        Console.OutputEncoding = Encoding.UTF8;
        using var output = new StringWriter();
        Console.SetOut(output);

        try
        {
            // Act
            DirHelper.PrintInstructions();

            // Assert
            var outputText = output.ToString();
            // Check for Unicode symbols (anchor, gear, arrow, etc.)
            Assert.Contains("\u2693", outputText); // Anchor
            Assert.Contains("\u2699", outputText); // Gear
            Assert.Contains("\u279C", outputText); // Arrow
        }
        finally
        {
            Console.SetOut(_originalOut);
        }
    }
}
