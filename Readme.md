# Lombiq .NET Analyzers



## About

.NET code analyzers and code convention settings for [Lombiq](https://lombiq.com) projects. We use these to enforce common standards across all our .NET projects, including e.g. in all of our [open-source Orchard Core extensions](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions). If you contribute to our open-source projects while using that solution you'll be guided by these rules too. You can check out a demo video of the project [here](https://www.youtube.com/watch?v=dtbGRi3Cezs).


## Analyzer packages used

We added and configured analyzers which are widely used and complement each other.

- [.NET code style analysis](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview#code-style-analysis)
- [AsyncFixer](https://www.nuget.org/packages/AsyncFixer)
- [DotNetAnalyzers.DocumentationAnalyzers](https://www.nuget.org/packages/DotNetAnalyzers.DocumentationAnalyzers/)
- [Microsoft.CodeAnalysis.FxCopAnalyzers](https://www.nuget.org/packages/Microsoft.CodeAnalysis.FxCopAnalyzers/)
- [Microsoft.VisualStudio.Threading.Analyzers](https://www.nuget.org/packages/microsoft.visualstudio.threading.analyzers)
- [SecurityCodeScan](https://www.nuget.org/packages/SecurityCodeScan/)
- [StyleCop.Analyzers](https://www.nuget.org/packages/StyleCop.Analyzers/)
- [SonarAnalyzer.CSharp](https://www.nuget.org/packages/SonarAnalyzer.CSharp/)

Furthermore, the project also includes an *.editorconfig* file with additional configuration for compatible editors.


## Guides

- [Adding analyzers to your project](Docs/AddingAnalyzers.md)
- [Using the analyzers during development](Docs/UsingAnalyzersDuringDevelopment.md)
- [Using the analyzers during command line builds](Docs/UsingAnalyzersDuringCommandLineBuilds.md)
- [Configuring analyzers](Docs/ConfiguringAnalyzers.md)


## Upgrading to a new version of the .NET SDK

When a new version of the .NET SDK comes out then you need to change the `LangVersion` and `AnalysisLevel` elements in the *Build.props* file to opt in to new language features and analyzers.


## Contributing and support

Bug reports, feature requests, comments, questions, code contributions, and love letters are warmly welcome, please do so via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.
