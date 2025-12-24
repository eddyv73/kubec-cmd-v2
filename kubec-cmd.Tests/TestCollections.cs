using Xunit;

namespace kubec_cmd.Tests;

/// <summary>
/// Collection definition to disable parallel execution for tests that modify Console.Out
/// </summary>
[CollectionDefinition("ConsoleTests", DisableParallelization = true)]
public class ConsoleTestsCollection : ICollectionFixture<ConsoleTestsFixture>
{
}

/// <summary>
/// Shared fixture for console tests
/// </summary>
public class ConsoleTestsFixture
{
    // No shared state needed, just used to group tests
}
