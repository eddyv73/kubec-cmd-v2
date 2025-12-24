using System;
using System.IO;
using Xunit;
using kubec_cmd;

namespace kubec_cmd.Tests;

public class ArgsControllerTests
{
    private readonly ArgsController _controller;

    public ArgsControllerTests()
    {
        _controller = new ArgsController();
    }

    [Fact]
    public void ArgsControl_WithNoArgs_ReturnsEmptyArgs()
    {
        // Arrange
        var args = new string[] { };
        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        var result = _controller.ArgsControl(args);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.target);
        Assert.Null(result.context);
    }

    [Fact]
    public void ArgsControl_WithSingleArg_PrintsInstructions()
    {
        // Arrange
        var args = new string[] { "invalid" };
        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        var result = _controller.ArgsControl(args);

        // Assert
        Assert.NotNull(result);
        var outputText = output.ToString();
        Assert.Contains("Kubec-cmd", outputText);
    }

    [Fact]
    public void ArgsControl_WithTargetFlag_SetsTarget()
    {
        // Arrange
        var args = new string[] { "-t", "test-config" };
        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        var result = _controller.ArgsControl(args);

        // Assert
        Assert.Equal("test-config", result.target);
    }

    [Fact]
    public void ArgsControl_WithListFlag_ListsFiles()
    {
        // Arrange
        var args = new string[] { "--list" };
        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        var result = _controller.ArgsControl(args);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void ArgsControl_WithTargetFlagButNoValue_PrintsNoTargetFound()
    {
        // Arrange
        var args = new string[] { "-t" };
        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        var result = _controller.ArgsControl(args);

        // Assert
        var outputText = output.ToString();
        Assert.Contains("No target file found", outputText);
    }
}
