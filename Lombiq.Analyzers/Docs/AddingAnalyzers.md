# Adding analyzers to your project

## Selecting which analyzer project/package to use

Lombiq .NET Analyzers is split into multiple projects/packages so you can select the one most suitable for your application. Depending on your use case, select one of the following:

- [`Lombiq.Analyzers`](../Readme.md): General .NET projects.
- [`Lombiq.Analyzers.NetFx`](../../Lombiq.Analyzers.NetFx/Readme.md): General .NET Framework projects.
- [`Lombiq.Analyzers.OrchardCore`](../../Lombiq.Analyzers.OrchardCore/Readme.md): [Orchard Core](https://orchardcore.net/) projects.
- [`Lombiq.Analyzers.Orchard1`](../../Lombiq.Analyzers.Orchard1/Readme.md): [Orchard 1](https://orchardcore.net/orchardcms) projects.
- [`Lombiq.Analyzers.VisualStudioExtension`](../../Lombiq.Analyzers.VisualStudioExtension/Readme.md): Visual Studio extension projects.

You only need to reference a single project; e.g., even though Orchard Core apps are also .NET apps, you only need to use `Lombiq.Analyzers.OrchardCore` for them.

## How to add the analyzers to SDK-style projects from NuGet

If you don't want to stay on the cutting-edge version, nor do you intend to contribute to Lombiq .NET Analyzers, you can use one of the NuGet packages. Install the package suitable for your project, as selected above. Check for the latest version number [on NuGet](https://www.nuget.org/packages/Lombiq.Analyzers/).

```csproj
    <PackageReference Include="Lombiq.Analyzers" Version="<latest version>">
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
```

The `<PrivateAssets>all</PrivateAssets>` is necessary to prevent the analyzers "leaking" into other projects that may consume yours.

You can also add the package to all projects inside a folder at once [from a _Directory.Build.props_ file](https://docs.microsoft.com/en-us/visualstudio/msbuild/customize-your-build).

## How to add the analyzers to SDK-style projects from a Git submodule

Use this approach if you always want to use the very latest version, or if you'd like to contribute to Lombiq .NET Analyzers.

1. Add to the following to the _.gitmodules_ file (we use the _tools_ subfolder for the submodule's folder here but feel free to use something else):

   ```gitmodules
   [submodule "Lombiq.Analyzers"]
       path = tools/Lombiq.Analyzers
       url = https://github.com/Lombiq/.NET-Analyzers.git
       branch = dev
   ```

   _`path` can target anything but we suggest either the folder where the solution `.sln` file is located (mostly the repository root) or a "tools" subfolder therein._
2. Create a _Directory.Build.props_ file in the folder where the solution _.sln_ file is located (mostly the repository root) with the following content, referencing the analyzer project you selected above (if you've put the submodule into a different folder then change the path):

   ```xml
   <Project>
     <Import Project="tools/Lombiq.Analyzers/Build.props" />
   </Project>
   ```

3. Since the project's _.editorconfig_ file will be copied into your solution's root you may want to gitignore it:

    ```gitignore
    /.editorconfig
    ```

For at least Visual Studio and JetBrains Rider you don't need any further setup for IDE support. For [OmniSharp-integrated editors](http://www.omnisharp.net/#integrations) like Visual Studio Code you'll also need to add an _omnisharp.json_ file to the root of the solution:

```json
{
    "RoslynExtensionsOptions": {
        "enableAnalyzersSupport": true
    },
    "FormattingOptions": {
        "enableEditorConfigSupport": true
    }
}
```

## How to add the analyzers to individual non-SDK-style .NET Framework projects (not solutions)

The key difference compared to .NET projects is that for non-SDK-style .NET Framework projects, you need to add Lombiq .NET Analyzers to each project.

1. Same as above - add the .NET-Analyzers repository as a Git submodule to your repository or add the NuGet package in the next step.
2. Create a _Directory.Build.props_ file in every project folder you want to target, next to the _.csproj_ file with the following content, either referencing `Lombiq.Analyzers.NetFx` or `Lombiq.Analyzers.Orchard1`:

   ```csproj
   <PackageReference Include="Lombiq.Analyzers.NetFx" Version="<latest version>">
       <PrivateAssets>all</PrivateAssets>
   </PackageReference>
   ```

   Or if you're using a Git submodule (adjust the relative path as suitable):

   ```xml
   <Project>
     <Import Project="../../../tools/Lombiq.Analyzers.NetFx/Build.props" />
   </Project>
   ```

   If you do this for multiple projects, it's recommended to add your own central props file (like _Analyzers.Build.props_) where you do the above and import that in each project instead.

3. _Build.props_ will copy this project's _.editorconfig_ file into every project folder that you've created a _Directory.Build.props_ in, so you might want to gitignore those:

    ```gitignore
    .editorconfig
    ```

## Introducing analyzers to an existing project

What to do if you're not starting a green-field project but want to add analyzers to an existing (large) project?

1. Fix any existing build warnings. Analyzer violations will be surfaced as warnings so it's best to fix build warnings first.
2. Enable `TreatWarningsAsErrors` [in CI/CD builds](UsingAnalyzersDuringCommandLineBuilds.md) to make sure you don't introduce more warnings. Or alternatively, at least you can enable `TreatWarningsAsErrors` for all projects so during local build it'll fail on any violation. You can do this from the same _Directory.Build.props_ file by adding the following:

    ```xml
    <PropertyGroup>
        <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    </PropertyGroup>
    ```

3. .NET compatibility:
    - .NET Core and .NET 5 or later projects can use this without anything special.
    - .NET Framework projects:
      - Projects using the new SDK-style csproj format don't need anything special. You [can convert projects manually](https://docs.microsoft.com/en-us/dotnet/core/porting/#per-project-steps) from the non-SDK-style csproj format or automatically with the [try-convert](https://github.com/dotnet/try-convert) utility. You'll also need to manually remove any leftover _packages.config_ files and adjust _NuGet.config_ files.
      - Projects using the legacy, non-SDK-style csproj format should use `Lombiq.Analyzers.NetFx` as described above, but the including project needs to use `PackageReference`s. You can convert your _packages.config_-based project to `PackageReference`s using [this walkthrough](https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference).  
4. Now you can add this project to yours as explained above.
5. Introduce analyzers gradually unless it's a small project and you can fix every analyzer violation at once. To do this, only enable a handful of analyzers first (or enable them just for a couple of projects in a given solution), fix the violations, get used to them, then enable more later. See [the docs on configuring analyzers](ConfiguringAnalyzers.md) for how to do disable certain analyzers of the default configuration and thus activating analyzers in your code base gradually. We recommend enabling analyzers and fixing violations in at least three stages:
    1. All the simpler rules, i.e. all rules except the ones in the next steps (that means, if you're working with the default configuration, to disable the rules mentioned in the next steps). These are quite straightforward to fix, to an extent can be done even automatically. It's the best if you group warnings by code in the Error List and fix them one by one, committing to the repository after completing each. It's better to batch this work in a way that you fix a particular type of warning for all projects of a solution at once, as opposed to fixing multiple type of warnings for just selected projects. This is because it's more efficient to just repeat the same kind of fix for all projects (can sometimes even be done automatically) in one go instead of revisiting it in multiple iterations. For tips on how to make fixing violations easier see [this page](UsingAnalyzersDuringDevelopment.md).
    2. "[SA1600 Elements should be documented](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1600.md)": This will require adding documentation to all interfaces. This and the next step can also be done individually for specific projects, so just have them enabled for certain projects, since these benefit much less from being done in bulk for the whole solution.
    3. "[S107 Methods should not have too many parameters](https://rules.sonarsource.com/csharp/RSPEC-107/)", "[S134 Control flow statements "if", "for", "while", "switch" and "try" should not be nested too deeply](https://rules.sonarsource.com/csharp/RSPEC-107/)" and "[S3776 Cognitive Complexity of functions should not be too high](https://rules.sonarsource.com/csharp/RSPEC-3776/)": These analyzer rules will require deeper refactoring of the code base. They're all worth it though.
