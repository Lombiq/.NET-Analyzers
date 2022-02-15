# Lombiq .NET Analyzers



[![Lombiq.Analyzers NuGet](https://img.shields.io/nuget/v/Lombiq.Analyzers?label=Lombiq.Analyzers)](https://www.nuget.org/packages/Lombiq.Analyzers/)


## About

.NET code analyzers and code convention settings for [Lombiq](https://lombiq.com) projects, predominantly for [Orchard Core](https://www.orchardcore.net/) apps but also any .NET apps. We use these to enforce common standards across all our .NET projects, including e.g. all of our [open-source Orchard Core extensions](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions). If you contribute to our open-source projects while using that solution you'll be guided by these rules, too. You can check out a demo video of the project [here](https://www.youtube.com/watch?v=dtbGRi3Cezs).

There is also support for non-SDK-style .NET Framework projects, as long as they use `PackageReference` for their dependencies (in contrast to *packages.config*).

Some other projects you may be interested in:

- Looking for something similar for JavaScript and SCSS? Check out the ESLint and Stylelint integrations of our [Gulp Extensions project](https://github.com/Lombiq/Gulp-Extensions).
- Looking not just for static code analysis but also dynamic testing? Check out our [UI Testing Toolbox](https://github.com/Lombiq/UI-Testing-Toolbox) and [Testing Toolbox](https://github.com/Lombiq/Testing-Toolbox) projects.
- Looking for a library that'll help you comply with analyzer rules? Check out our [Helpful Libraries project](https://github.com/Lombiq/Helpful-Libraries).

Do you want to quickly try out this project and see it in action? Check it out in our [Open-Source Orchard Core Extensions](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions) full Orchard Core solution and also see our other useful Orchard Core-related open-source projects!

## Analyzer packages used

We added and configured analyzers which are widely used and complement each other.

- [.NET code style analysis](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview#code-style-analysis)
- [.NET code quality analysis](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview#code-quality-analysis)
- [AsyncFixer](https://www.nuget.org/packages/AsyncFixer)
- [DotNetAnalyzers.DocumentationAnalyzers](https://www.nuget.org/packages/DotNetAnalyzers.DocumentationAnalyzers/)
- [Meziantou.Analyzer](https://www.nuget.org/packages/Meziantou.Analyzer/)
- [Microsoft.CodeAnalysis.NetAnalyzers](https://www.nuget.org/packages/Microsoft.CodeAnalysis.NetAnalyzers)
- [Microsoft.VisualStudio.Threading.Analyzers](https://www.nuget.org/packages/microsoft.visualstudio.threading.analyzers)
- [SecurityCodeScan.VS2019](https://www.nuget.org/packages/SecurityCodeScan.VS2019/)
- [StyleCop.Analyzers](https://www.nuget.org/packages/StyleCop.Analyzers/)
- [SonarAnalyzer.CSharp](https://www.nuget.org/packages/SonarAnalyzer.CSharp/)

Furthermore, the project also includes an *.editorconfig* file with additional configuration for compatible editors.


## Guides

- [Adding analyzers to your project](Docs/AddingAnalyzers.md)
- [Using the analyzers during development](Docs/UsingAnalyzersDuringDevelopment.md)
- [Using the analyzers during command line builds](Docs/UsingAnalyzersDuringCommandLineBuilds.md)
- [Configuring analyzers](Docs/ConfiguringAnalyzers.md)

## Contributing and support

Bug reports, feature requests, comments, questions, code contributions, and love letters are warmly welcome, please do so via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.

### Upgrading to a new version of the .NET SDK
When a new version of the .NET SDK comes out then to the following:
- Change the `LangVersion` and `AnalysisLevel` elements in the *Build.props* file to opt in to new language features and analyzers. This does not concern non-SDK-style .NET Framework projects.
- Wait for all analyzers to support the new SDK (primarily the new language features). Then update all packages to latest.

### Adding a new analyzer
When adding a new analyzer package, do the following:

- Check if the project is actively developed with issues addressed quickly.
- Check if it has any significant impact on build or editing performance.
- Go through all rules in the package and decide one by one whether we need them.
    - Check for rules that are already covered by some other analyzer and disable duplicates.
    - All rules that we need should be surfaced as Warnings. This allows to only break the build on analyzer violations when we need it, i.e. during CI builds but not during development.
- Test it on multiple significant solutions. Using the [Hastlayer SDK](https://github.com/Lombiq/Hastlayer-SDK) is a good example as it's a large C# solution.
