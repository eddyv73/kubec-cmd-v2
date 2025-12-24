using System;
using System.IO;
using Xunit;
using kubec_cmd;

namespace kubec_cmd.Tests;

public class ArgsControllerTests
{
    private readonly ArgsController _controller;
    private readonly TextWriter _originalOut;

    public ArgsControllerTests()
    {
        _controller = new ArgsController();
        _originalOut = Console.Out;
    }

    [Fact]
    public void ArgsControl_WithNoArgs_ReturnsEmptyArgs()
    {
        // Arrange
        var args = Array.Empty<string>();
        using var output = new StringWriter();
        Console.SetOut(output);

        try
        {
            // Act
            var result = _controller.ArgsControl(args);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.target);
            Assert.Null(result.context);
        }
        finally
        {
            Console.SetOut(_originalOut);
        }
    }

    [Fact]
    public void ArgsControl_WithSingleArg_PrintsInstructions()
    {
        // Arrange
        var args = new string[] { "invalid" };
        using var output = new StringWriter();
        Console.SetOut(output);

        try
        {
            // Act
            var result = _controller.ArgsControl(args);

            // Assert
            Assert.NotNull(result);
            var outputText = output.ToString();
            Assert.Contains("Kubec-cmd", outputText);
        }
        finally
        {
            Console.SetOut(_originalOut);
        }
    }

    [Fact]
    public void ArgsControl_WithTargetFlag_SetsTarget()
    {
        // Arrange
        var args = new string[] { "-t", "test-config" };
        using var output = new StringWriter();
        Console.SetOut(output);

        try
        {
            // Act
            var result = _controller.ArgsControl(args);

            // Assert
            Assert.Equal("test-config", result.target);
        }
        finally
        {
            Console.SetOut(_originalOut);
        }
    }

    [Fact]
    public void ArgsControl_WithListFlag_ListsFiles()
    {
        // Arrange
        var args = new string[] { "--list" };
        using var output = new StringWriter();
        Console.SetOut(output);

        try
        {
            // Act
            var result = _controller.ArgsControl(args);

            // Assert
            Assert.NotNull(result);
        }
        finally
        {
            Console.SetOut(_originalOut);
        }
    }

    [Fact]
    public void ArgsControl_WithTargetFlagButNoValue_PrintsNoTargetFound()
    {
        // Arrange
        var args = new string[] { "-t" };
        using var output = new StringWriter();
        Console.SetOut(output);

        try
        {
            // Act
            var result = _controller.ArgsControl(args);

            // Assert
            var outputText = output.ToString();
            Assert.Contains("No target file found", outputText);
        }
        finally
        {
            Console.SetOut(_originalOut);
        }
    }
}
