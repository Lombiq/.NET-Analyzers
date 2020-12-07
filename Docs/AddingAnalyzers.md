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
5. Introduce analyzers gradually unless it's a small project and you can fix every analyzer violation at once. To do this, only enable a handful of analyzers first (or enable them just for a couple of projects in a given solution), fix the violations, get used to them, then enable more later. See [the docs on configuring analyzers](ConfiguringAnalyzers.md) for how to do disable certain analyzers.
