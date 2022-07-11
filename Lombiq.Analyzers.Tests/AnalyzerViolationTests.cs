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
        await CliProgram.DotNet.ExecuteAsync(CancellationToken.None, "restore", solutionRelativePath);
        var exception = (InvalidOperationException)await Should.ThrowAsync(
            () => ExecuteStaticCodeAnalysisAsync(solutionRelativePath),
            typeof(InvalidOperationException));

        // Excepted exception codes:
        // error IDE0021: Use expression body for constructors.
        // error IDE0044: Make field readonly.
        // error S2933: Make 'Number' 'readonly'.
        // error S4487: Remove this unread private field 'Number' or refactor the code to use its value.

        var exceptionCodes = new[] { "IDE0021", "IDE0044", "S2933", "S4487" };

        SelectErrorCodes(exception).ShouldBe(exceptionCodes, $"Exception message: {exception}");
    }

    [Fact]
    public async Task AnalyzerShouldNotSpreadToDependentProjects()
    {
        var solutionRelativePath = "Lombiq.Analyzers.PackageReference";
        var exception = (InvalidOperationException)await Should.ThrowAsync(
            () => ExecuteStaticCodeAnalysisAsync(solutionRelativePath),
            typeof(InvalidOperationException));

        var violationCount = SelectErrorCodes(exception).Count();
        violationCount.ShouldBeGreaterThan(0); // Just to be sure.

        exception = (InvalidOperationException)await Should.ThrowAsync(
            () => ExecuteStaticCodeAnalysisAsync(solutionRelativePath, "-p:ImportPackageAgain=true"),
            typeof(InvalidOperationException));

        // The solution contains two projects, both copy the same file. One project also has a ProjectReference to the
        // other. The first run above will only emit warnings for the main project that references the NuGet package.
        // The second run defines a symbol which enables a PackageReference in the other project too, so there will be
        // twice as many warnings, one set for each copy of the same source file.
        SelectErrorCodes(exception).Count().ShouldBe(2 * violationCount, $"Exception message: {exception}");
    }

    // Runs `dotnet build $SolutionFileName$ --no-incremental --nologo --warnaserror --consoleLoggerParameters:NoSummary
    // --verbosity:quiet -p:TreatWarningsAsErrors=true -p:RunAnalyzersDuringBuild=true` command. See
    // https://github.com/Lombiq/.NET-Analyzers/blob/dev/Docs/UsingAnalyzersDuringCommandLineBuilds.md#net-code-style-analysis
    // for more information.
    private static Task ExecuteStaticCodeAnalysisAsync(string solutionPath, params string[] additionalArguments)
    {
        var relativeSolutionPath = Path.Combine("..", "..", "..", "..", "TestSolutions", solutionPath);

        var arguments = new List<object>
        {
            "build",
            relativeSolutionPath,
            "--no-incremental",
            "--nologo",
            "--warnaserror",
            "--consoleLoggerParameters:NoSummary",
            "--verbosity:quiet",
            "-p:TreatWarningsAsErrors=true",
            "-p:RunAnalyzersDuringBuild=true",
        };
        arguments.AddRange(additionalArguments);

        return CliProgram.DotNet.ExecuteAsync(arguments, additionalExceptionText: null, CancellationToken.None);
    }

    private static IEnumerable<string> SelectErrorCodes(Exception exception) =>
        exception
            .Message
            .Split(new[] { '\n', '\r' }, StringSplitOptions.None)
            .Where(line => line.Contains(" error "))
            .Select(line => line.RegexReplace(@"^.* error ([^:]+):.*$", "$1"))
            .OrderBy(line => line);

    public static IEnumerable<object[]> Data()
    {
        static object[] FromProjectName(string projectName) =>
            new object[] { Path.Combine(projectName, projectName + ".csproj") };

        yield return FromProjectName("Lombiq.Analyzers.PackageReference");
        yield return FromProjectName("Lombiq.Analyzers.ProjectReference");
    }
}
