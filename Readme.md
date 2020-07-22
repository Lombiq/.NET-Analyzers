# Lombiq .NET Analyzers



## About

.NET code analyzers and code convention settings for [Lombiq](https://lombiq.com) projects. We use these to enforce common standards across all our .NET projects, including e.g. in all of our [open-source Orchard Core extensions](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions). If you contribute to our open-source projects you'll be guides by these rules too.


## Documentation

### Analyzer packages used

- [Microsoft.CodeAnalysis.FxCopAnalyzers](https://www.nuget.org/packages/Microsoft.CodeAnalysis.FxCopAnalyzers/)
- [StyleCop.Analyzers](https://www.nuget.org/packages/StyleCop.Analyzers/)
- [SonarAnalyzer.CSharp](https://www.nuget.org/packages/SonarAnalyzer.CSharp/)

### How to add analyzers to project repository

1. Add to `.gitmodules` file:
   ```
   [submodule "Lombiq.Analyzers"]
       path = Lombiq.Analyzers
       url = https://github.com/Lombiq/.NET-Analyzers.git
       branch = dev
   ```
1. Create `Directory.Build.props` file in the repository root folder with the following content:
   ```xml
   <Project>
     <Import Project="Lombiq.Analyzers/Build.props"/>
   </Project>
   ```


## Contributing and support

Bug reports, feature requests, comments, questions, code contributions, and love letters are warmly welcome, please do so via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.