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

### How to add analyzers to project repository

1. Add to `.gitmodules` file (we use the *tools* subfolder for the submodule's folder here but feel free to use something else):
   ```
   [submodule "Lombiq.Analyzers"]
       path = tools/Lombiq.Analyzers
       url = https://github.com/Lombiq/.NET-Analyzers.git
       branch = dev
   ```
   *`path` can target anything but we suggest either the folder where the solution `.sln` file is located (mostly the repository root) or a "tools" subfolder therein.*
1. Create a `Directory.Build.props` file in the folder where the solution `.sln` file is located (mostly the repository root) with the following content (if you've put the submodule in to a different folder then change the path):
   ```xml
   <Project>
     <Import Project="tools/Lombiq.Analyzers/Build.props"/>
   </Project>
   ```

### How to disable analyzers for particular projects

Place a `Directory.Build.props` file into the project's folder (or folder with set of projects) with the following contents:

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
dotnet build ./SomeSolution.sln -c Debug --no-incremental -p:RunCodeAnalysis=false
```

### Practices on suppressing a rule for a given piece of code

Analyzers are not perfect so they can give false positives, and there can always be justified exceptions to every rule, so suppressing analyzer warnings is fine if done in moderation (if you have to do it a lot for a given rule then the rule is not suitable for your coding style). When doing so adhere to the following:

- Always suppress a warning in the smallest scope possible.
- Use the `#pragma` suppress for specific lines of code.
- Only use the `SuppressMessage` attribute on a member (or even a whole project) if the suppress absolutely must cover the whole member or if it's for a method argument.
- Always add a justification.

### Using the analyzers during development

Output of the analyzers will show up as entries of various levels (i.e. Errors, Warnings, Messages) in the Error List window of Visual Studio for the currently open files. You'll also see squiggly lines in the code editor as it is usual for any code issues. For a lot of issues you'll be able to use automatic code fixes, or suppress them if they're wrong in the given context from the Quick Actions menu (Ctrl+. by default).

The `Build.props` file disables analyzers during Visual Studio build, not to slow down development; you can enable them by setting `RunAnalyzersDuringBuild` to `true`. After this, they'll show for the whole solution after a rebuild (but not when you build an already built solution, just with a rebuild or a fresh build).

Similarly, they'll show up in the build output of `dotnet build` (regardless of `RunAnalyzersDuringBuild`). 

Note that if you have the [Microsoft Code Analysis Visual Studio extension](https://docs.microsoft.com/en-us/visualstudio/code-quality/install-fxcop-analyzers#vsix) installed then it'll clash with the analyzer packages and you'll see warnings in Visual Studio of the like of "An instance of analyzer Microsoft.NetCore.CSharp.Analyzers.Runtime.CSharpDoNotRaiseReservedExceptionTypesAnalyzer cannot be created from..." To fix this, disable or uninstall the extension.


## Contributing and support

Bug reports, feature requests, comments, questions, code contributions, and love letters are warmly welcome, please do so via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.