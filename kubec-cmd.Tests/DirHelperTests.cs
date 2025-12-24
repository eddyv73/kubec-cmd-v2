using System;
using System.IO;
using System.Text;
using Xunit;

namespace kubec_cmd.Tests;

// Disable parallel execution for these tests since they modify Console.Out
[Collection("ConsoleTests")]
public class DirHelperTests
{
    [Fact]
    public void PrintInstructions_OutputsAsciiArt()
    {
        // Arrange
        var originalOut = Console.Out;
        var output = new StringWriter();

        try
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetOut(output);

            // Act
            DirHelper.PrintInstructions();

            // Get output before restoring
            var outputText = output.ToString();

            // Assert
            Assert.Contains("Kubec-cmd", outputText);
            Assert.Contains("===", outputText);
        }
        finally
        {
            Console.SetOut(originalOut);
            output.Dispose();
        }
    }

    [Fact]
    public void PrintInstructions_ContainsVersionInfo()
    {
        // Arrange
        var originalOut = Console.Out;
        var output = new StringWriter();

        try
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetOut(output);

            // Act
            DirHelper.PrintInstructions();

            // Get output before restoring
            var outputText = output.ToString();

            // Assert
            Assert.Contains("Formula", outputText);
        }
        finally
        {
            Console.SetOut(originalOut);
            output.Dispose();
        }
    }

    [Fact]
    public void PrintInstructions_ContainsUsageInstructions()
    {
        // Arrange
        var originalOut = Console.Out;
        var output = new StringWriter();

        try
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetOut(output);

            // Act
            DirHelper.PrintInstructions();

            // Get output before restoring
            var outputText = output.ToString();

            // Assert
            Assert.Contains("Target file", outputText);
            Assert.Contains("kubec-cmd -t", outputText);
            Assert.Contains("--list", outputText);
            Assert.Contains("--clean", outputText);
        }
        finally
        {
            Console.SetOut(originalOut);
            output.Dispose();
        }
    }

    [Fact]
    public void PrintInstructions_ContainsGithubLink()
    {
        // Arrange
        var originalOut = Console.Out;
        var output = new StringWriter();

        try
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetOut(output);

            // Act
            DirHelper.PrintInstructions();

            // Get output before restoring
            var outputText = output.ToString();

            // Assert
            Assert.Contains("github.com", outputText);
        }
        finally
        {
            Console.SetOut(originalOut);
            output.Dispose();
        }
    }

    [Fact]
    public void PrintInstructions_ContainsAuthorInfo()
    {
        // Arrange
        var originalOut = Console.Out;
        var output = new StringWriter();

        try
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetOut(output);

            // Act
            DirHelper.PrintInstructions();

            // Get output before restoring
            var outputText = output.ToString();

            // Assert
            Assert.Contains("Eddy Wister", outputText);
        }
        finally
        {
            Console.SetOut(originalOut);
            output.Dispose();
        }
    }

    [Fact]
    public void PrintInstructions_ContainsUnicodeSymbols()
    {
        // Arrange
        var originalOut = Console.Out;
        var output = new StringWriter();

        try
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetOut(output);

            // Act
            DirHelper.PrintInstructions();

            // Get output before restoring
            var outputText = output.ToString();

            // Assert - Check for Unicode symbols
            Assert.Contains("\u2693", outputText); // Anchor
            Assert.Contains("\u2699", outputText); // Gear
            Assert.Contains("\u279C", outputText); // Arrow
        }
        finally
        {
            Console.SetOut(originalOut);
            output.Dispose();
        }
    }
}
