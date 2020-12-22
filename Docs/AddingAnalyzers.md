# Adding analyzers to your project



## How to add the analyzers to your project repository

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

This will use the analyzer configuration suitable for Orchard Core projects. If you want to use the this in a non-Orchard .NET app then switch over to the general configuration by adding the following to the *Directory.Build.props* file:

```xml
<PropertyGroup>
    <CodeAnalysisRuleSet>$(MSBuildThisFileDirectory)General.ruleset</CodeAnalysisRuleSet>
</PropertyGroup>
```

Note that the analyzers support both .NET Core and .NET Framework projects. However, to get full support you'll need to use the new SDK-style csproj format (this is also possible with .NET Framework). Most possibly you can automatically convert your projects with the [try-convert utility](https://github.com/dotnet/try-convert).


## Introducing analyzers to an existing project

What to do if you're not starting a green-field project but want to add analyzers to an existing (large) project?

1. Fix any existing build warnings. Analyzer violations will be surfaced as warnings so it's best to fix build warnings first.
2. Enable `TreatWarningsAsErrors` [in CI/CD builds](UsingAnalyzersDuringCommandLineBuilds.md) to make sure you don't introduce more warnings. Or alternatively, at least you can enable `TreatWarningsAsErrors` for all projects so during local build it'll fail on any violation. You can do this from the same *Directory.Build.props* file by adding the following:
    ```xml
    <PropertyGroup>
        <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    </PropertyGroup>
    ```
3. While this project can be used with .NET Framework, .NET Core and .NET 5 or later projects alike, it needs all projects to use the new SDK-style csproj format. You [can convert projects manually](https://docs.microsoft.com/en-us/dotnet/core/porting/#per-project-steps) or automatically with the [try-convert](https://github.com/dotnet/try-convert) utility. You'll also need to manually remove any leftover `packages.config` files and adjust `NuGet.config` files. 
4. Now you can add this project to yours as explained above.
5. Introduce analyzers gradually unless it's a small project and you can fix every analyzer violation at once. To do this, only enable a handful of analyzers first (or enable them just for a couple of projects in a given solution), fix the violations, get used to them, then enable more later. See [the docs on configuring analyzers](ConfiguringAnalyzers.md) for how to do disable certain analyzers of the default configuration. We recommend enabling analyzers and fixing violations in at least three stages:
    1. All the simpler rules, i.e. all rules except the ones in the next steps. These are quite straightforward to fix, to an extent can be done even automatically. It's the best if you group warnings by code in the Error List and fix them one by one, committing to the repository after completing each. It's better to batch this work in a way that you fix a particular type of warning for all projects of a solution at once, as opposed to fixing multiple type of warnings for just selected projects. This is because it's more efficient to just repeat the same kind of fix for all projects (can sometimes even be done automatically) in one go instead of revisiting it in multiple iterations. For tips on how to make fixing violations easier see [this page](UsingAnalyzersDuringDevelopment.md).
    2. "[SA1600 Elements should be documented](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1600.md)": This will require adding documentation to all interfaces. This and the next step can also be done individually for specific projects, so just have them enabled for certain projects, since these benefit much less from being done in bulk for the whole solution.
    3. "[S107 Methods should not have too many parameters](https://rules.sonarsource.com/csharp/RSPEC-107)", "[S134 Control flow statements "if", "for", "while", "switch" and "try" should not be nested too deeply](https://rules.sonarsource.com/csharp/RSPEC-107)", and "[S3776 Cognitive Complexity of functions should not be too high](https://rules.sonarsource.com/csharp/RSPEC-3776)": These analyzer rules will require deeper refactoring of the code base. They're all worth it though.
