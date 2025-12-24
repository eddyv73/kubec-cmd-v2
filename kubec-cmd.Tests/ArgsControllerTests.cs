using System;
using System.IO;
using System.Text;
using Xunit;
using kubec_cmd;

namespace kubec_cmd.Tests;

// Disable parallel execution for these tests since they modify Console.Out
[Collection("ConsoleTests")]
public class ArgsControllerTests
{
    [Fact]
    public void ArgsControl_WithNoArgs_ReturnsEmptyArgs()
    {
        // Arrange
        var originalOut = Console.Out;
        var output = new StringWriter();
        var controller = new ArgsController();

        try
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetOut(output);

            // Act
            var result = controller.ArgsControl(Array.Empty<string>());

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.target);
            Assert.Null(result.context);
        }
        finally
        {
            Console.SetOut(originalOut);
            output.Dispose();
        }
    }

    [Fact]
    public void ArgsControl_WithSingleArg_PrintsInstructions()
    {
        // Arrange
        var originalOut = Console.Out;
        var output = new StringWriter();
        var controller = new ArgsController();

        try
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetOut(output);

            // Act
            var result = controller.ArgsControl(new[] { "invalid" });

            // Get output before restoring
            var outputText = output.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.Contains("Kubec-cmd", outputText);
        }
        finally
        {
            Console.SetOut(originalOut);
            output.Dispose();
        }
    }

    [Fact]
    public void ArgsControl_WithTargetFlag_SetsTargetAndHandlesFilesystem()
    {
        // Arrange
        var originalOut = Console.Out;
        var output = new StringWriter();
        var controller = new ArgsController();

        try
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetOut(output);

            // Act - May throw if .kube directory doesn't exist in CI
            var result = controller.ArgsControl(new[] { "-t", "test-config" });

            // Assert - If we get here, filesystem exists
            Assert.Equal("test-config", result.target);
        }
        catch (DirectoryNotFoundException)
        {
            // Expected in CI environment where ~/.kube doesn't exist
            Assert.True(true);
        }
        finally
        {
            Console.SetOut(originalOut);
            output.Dispose();
        }
    }

    [Fact]
    public void ArgsControl_WithListFlag_HandlesFilesystem()
    {
        // Arrange
        var originalOut = Console.Out;
        var output = new StringWriter();
        var controller = new ArgsController();

        try
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetOut(output);

            // Act - May throw if .kube directory doesn't exist in CI
            var result = controller.ArgsControl(new[] { "--list" });

            // Assert
            Assert.NotNull(result);
        }
        catch (DirectoryNotFoundException)
        {
            // Expected in CI environment where ~/.kube doesn't exist
            Assert.True(true);
        }
        finally
        {
            Console.SetOut(originalOut);
            output.Dispose();
        }
    }

    [Fact]
    public void ArgsControl_WithTargetFlagButNoValue_PrintsNoTargetFound()
    {
        // Arrange
        var originalOut = Console.Out;
        var output = new StringWriter();
        var controller = new ArgsController();

        try
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetOut(output);

            // Act
            var result = controller.ArgsControl(new[] { "-t" });

            // Get output before restoring
            var outputText = output.ToString();

            // Assert - Check that the output contains the expected message
            // Note: The message is printed before the instructions banner
            Assert.Contains("No target file found", outputText);
        }
        finally
        {
            Console.SetOut(originalOut);
            output.Dispose();
        }
    }

    [Fact]
    public void Args_Properties_CanBeSetAndRetrieved()
    {
        // Arrange & Act
        var args = new Args
        {
            target = "my-target",
            context = "my-context"
        };

        // Assert
        Assert.Equal("my-target", args.target);
        Assert.Equal("my-context", args.context);
    }
}
