# Lombiq .NET Analyzers



## About

.NET code analyzers and code convention settings for [Lombiq](https://lombiq.com) projects. We use these to enforce common standards across all our .NET projects, including e.g. in all of our [open-source Orchard Core extensions](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions). If you contribute to our open-source projects while using that solution you'll be guided by these rules too. You can check out a demo video of the project [here](https://www.youtube.com/watch?v=dtbGRi3Cezs).


## Documentation

### Analyzer packages used

- [AsyncFixer](https://www.nuget.org/packages/AsyncFixer)
- [DotNetAnalyzers.DocumentationAnalyzers](https://www.nuget.org/packages/DotNetAnalyzers.DocumentationAnalyzers/)
- [Microsoft.CodeAnalysis.FxCopAnalyzers](https://www.nuget.org/packages/Microsoft.CodeAnalysis.FxCopAnalyzers/)
- [Microsoft.VisualStudio.Threading.Analyzers](https://www.nuget.org/packages/microsoft.visualstudio.threading.analyzers)
- [SecurityCodeScan](https://www.nuget.org/packages/SecurityCodeScan/)
- [StyleCop.Analyzers](https://www.nuget.org/packages/StyleCop.Analyzers/)
- [SonarAnalyzer.CSharp](https://www.nuget.org/packages/SonarAnalyzer.CSharp/)

Furthermore, the project also includes an *.editorconfig* file with additional configuration for compatible editors.

### How to add the analyzers to you project repository

1. Add to *.gitmodules* file (we use the *tools* subfolder for the submodule's folder here but feel free to use something else):
   ```
   [submodule "Lombiq.Analyzers"]
       path = tools/Lombiq.Analyzers
       url = https://github.com/Lombiq/.NET-Analyzers.git
       branch = dev
   ```
   *`path` can target anything but we suggest either the folder where the solution `.sln` file is located (mostly the repository root) or a "tools" subfolder therein.*
2. Create a *Directory.Build.props* file in the folder where the solution *.sln* file is located (mostly the repository root) with the following content (if you've put the submodule in to a different folder then change the path):
   ```xml
   <Project>
     <Import Project="tools/Lombiq.Analyzers/Build.props"/>
   </Project>
   ```

Note that the analyzers support both .NET Core and .NET Framework projects. However, to get full support you'll need to use the new SDK-style csproj format (this is also possible with .NET Framework). Most possibly you can automatically convert your projects with the [try-convert utility](https://github.com/dotnet/try-convert).

### Using the analyzers during development

#### Working with analyzers in Visual Studio or another IDE

Output of the analyzers will show up as entries of various levels (i.e. Errors, Warnings, Messages) in the Error List window of Visual Studio for the currently open files. You'll also see squiggly lines in the code editor as it is usual for any code issues. For a lot of issues you'll be able to use automatic code fixes, or suppress them if they're wrong in the given context from the Quick Actions menu (<kbd>Ctrl</kbd> + <kbd>.</kbd> by default).

The *Build.props* file disables analyzers during Visual Studio build, not to slow down development; you can enable them by setting `RunAnalyzersDuringBuild` to `true`. After this, they'll show for the whole solution after a rebuild (but not when you build an already built solution, just with a rebuild or a fresh build).

Note that if you have the [Microsoft Code Analysis Visual Studio extension](https://docs.microsoft.com/en-us/visualstudio/code-quality/install-fxcop-analyzers#vsix) installed then it'll clash with the analyzer packages and you'll see warnings in Visual Studio of the like of "An instance of analyzer Microsoft.NetCore.CSharp.Analyzers.Runtime.CSharpDoNotRaiseReservedExceptionTypesAnalyzer cannot be created from..." To fix this, disable or uninstall the extension.

When working on reducing cognitive complexity to pass the [S3776 rule](https://rules.sonarsource.com/csharp/RSPEC-3776) you can make use of the [CognitiveComplexity plugin for JetBrains Rider](https://plugins.jetbrains.com/plugin/12024-cognitivecomplexity) or the [ReSharper plugin of the same name](https://plugins.jetbrains.com/plugin/12391-cognitivecomplexity) for Visual Studio. It annotates the individual contributors to the cognitive complexity score so you can quickly identify trouble spots with the least effort. The scoring algorithm is not 100% identical to the one used in Sonar but it's similar enough to greatly speed up the task of refactoring complex methods.

#### Practices on suppressing a rule for a given piece of code

Analyzers are not perfect so they can give false positives, and there can always be justified exceptions to every rule, so suppressing analyzer warnings is fine if done in moderation (if you have to do it a lot for a given rule then the rule is not suitable for your coding style). When doing so adhere to the following:

- Always suppress a warning in the smallest scope possible.
- Use the `#pragma` suppress for specific lines of code.
- Only use the `SuppressMessage` attribute on a member (or even a whole project) if the suppress absolutely must cover the whole member or if it's for a method argument.
- Always add a justification.

### Using the analyzers during command line builds

The following notes are useful if you're building not from within and IDE but from the command line, like in a CI environment.

#### Showing analyzer warnings during `dotnet build`

Analyzer warnings will show up in the build output of `dotnet build` regardless of the `RunAnalyzersDuringBuild` config in *Build.props*. Note though, that this will only happen if it's a fresh, clean build; otherwise if you're building an already built solution use the `--no-incremental` switch to make analyzer warnings appear:

```ps
dotnet build MySolution.sln --no-incremental
```

If you want analyzer violations to fail the build (recommended) then also use `TreatWarningsAsErrors`:

```ps
dotnet build MySolution.sln --no-incremental /p:TreatWarningsAsErrors=true
```

#### .NET 5 code style analysis

If you want code style analysis configured in *.editorconfig* (i.e. IDE* rules, this is not applicable to the others) to be checked during build too (it's already checked during editing) then you'll need to use the .NET 5 SDK or later [and configure `EnforceCodeStyleInBuild`](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview#code-style-analysis). This is not enabled in this project globally, not to slow down Visual Studio builds. However, you can enable it during `dotnet build` by using two switches like following:

```ps
dotnet build MySolution.sln --no-incremental /p:EnforceCodeStyleInBuild=true /p:RunAnalyzersDuringBuild=true
```

Our recommendation is to use it together with `TreatWarningsAsErrors` but do note that for code style analysis warnings you also have to specify `-warnaserror` (this is not needed for the other analyzers):

```ps
dotnet build MySolution.sln --no-incremental -warnaserror /p:TreatWarningsAsErrors=true /p:EnforceCodeStyleInBuild=true /p:RunAnalyzersDuringBuild=true
```

Or if you only want to see the errors and not the full build output (including e.g. NuGet restores, build messages):

```ps
dotnet build MySolution.sln --no-incremental -warnaserror /p:TreatWarningsAsErrors=true /p:EnforceCodeStyleInBuild=true /p:RunAnalyzersDuringBuild=true -nologo -consoleLoggerParameters:NoSummary -verbosity:quiet
```

Note that code style analysis is experimental in the .NET 5 SDK and [may change in later versions](https://github.com/dotnet/roslyn/issues/49044).

### How to disable all analyzers for particular projects

Place a *Directory.Build.props* file into the project's folder (or folder with set of projects) with the following contents:

```xml
<Project>
  <ItemGroup> 
      <Analyzer Remove="@(Analyzer)" /> 
  </ItemGroup>
</Project>
```

This will completely disable code analysis.

### How to disable analyzers during `dotnet build`

By default the `dotnet build` command runs analyzers and produces code analysis warnings if there are any but it makes the build slower. Pass the `-p:RunCodeAnalysis=false` parameter to disable analyzers during build, like:

```ps
dotnet build MySolution.sln -p:RunCodeAnalysis=false
```

### How to override analyzer configuration globally

If not all the configuration in this project is suitable for your solution then you can also override them globally. This way, the default configuration will be merged with your custom configuration and you can override any number of rules conveniently at one place for all projects in your solution.

1. Create your own ruleset file, similar to this project's *General.ruleset* file, and add any rule configurations there that you want to override. Also, include this project's *General.ruleset* file as a child, allowing its rules to be available by default. So you'll have something like this (rules included only as examples):
    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <RuleSet Name="General C# rules" ToolsVersion="16.0">
      <Include Path="tools\Lombiq.Analyzers\General.ruleset" Action="Default" />
      <Rules AnalyzerId="SecurityCodeScan" RuleNamespace="SecurityCodeScan">
        <Rule Id="SCS0005" Action="None" />
      </Rules>
      <Rules AnalyzerId="StyleCop.Analyzers" RuleNamespace="StyleCop.Analyzers">
        <Rule Id="SA1312" Action="Warning" />
      </Rules>
    </RuleSet>
    ```
2. In the `Directory.Build.props` file of your solution add a reference to your own ruleset file, overriding the default:
    ```xml
    <PropertyGroup>
      <CodeAnalysisRuleSet>$(MSBuildThisFileDirectory)My.ruleset</CodeAnalysisRuleSet>
    </PropertyGroup>
    ```
3. Now every rule you defined in *My.ruleset* will take precedence over the default ones. For everything else the default ones will be applied.


## Upgrading to a new version of the .NET SDK

When a new version of the .NET SDK comes out then you need to change the `LangVersion` and `AnalysisLevel` elements in the *Build.props* file to opt in to new language features and analyzers.


## Contributing and support

Bug reports, feature requests, comments, questions, code contributions, and love letters are warmly welcome, please do so via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.
