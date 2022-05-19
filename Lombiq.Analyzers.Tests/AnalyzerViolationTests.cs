using Lombiq.HelpfulLibraries.Cli;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.Analyzers.Tests;

public class AnalyzerViolationTests
{
    [Theory]
    [MemberData(nameof(Data))]
    public async Task AnalyzerViolationShouldBeReported(string solutionRelativePath)
    {
        var exception = (InvalidOperationException)await Should.ThrowAsync(
            () => ExecuteStaticCodeAnalysisAsync(solutionRelativePath),
            typeof(InvalidOperationException));

        // Excepted exception codes:
        // error IDE0021: Use expression body for constructors.
        // error IDE0044: Make field readonly.
        // error S2933: Make 'Number' 'readonly'.
        // error S4487: Remove this unread private field 'Number' or refactor the code to use its value.

        var exceptionCodes = new[] { "IDE0021", "IDE0044", "S2933", "S4487" };

        exception
            .Message
            .Split(new[] { '\n', '\r' }, StringSplitOptions.None)
            .Where(line => line.Contains(" error "))
            .Select(line => line.RegexReplace(@"^.* error ([^:]+):.*$", "$1"))
            .OrderBy(line => line)
            .ShouldBe(exceptionCodes);
    }

    // Runs dotnet msbuild {solutionPath}.sln -t:Clean,Build -v:quiet -p:RunAnalyzersDuringBuild=true -p:TreatWarningsAsErrors=true -warnAsError
    // See https://github.com/Lombiq/.NET-Analyzers/blob/dev/Docs/UsingAnalyzersDuringCommandLineBuilds.md#net-code-style-analysis
    private static Task ExecuteStaticCodeAnalysisAsync(string solutionPath) =>
        CliProgram.DotNet.ExecuteAsync(
            CancellationToken.None,
            "msbuild",
            Path.Combine("..", "..", "..", "..", "TestSolutions", solutionPath),
            "-t:Clean,Build",
            "-v:quiet",
            "-p:RunAnalyzersDuringBuild=true",
            "-p:TreatWarningsAsErrors=true",
            "-warnAsError");

    public static IEnumerable<object[]> Data()
    {
        static object[] FromProjectName(string projectName) =>
            new object[] { Path.Combine(projectName, projectName + ".csproj") };

        yield return FromProjectName("Lombiq.Analyzers.PackageReference");
        yield return FromProjectName("Lombiq.Analyzers.ProjectReference");
    }
}
