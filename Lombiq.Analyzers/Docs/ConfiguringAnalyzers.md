# Configuring analyzers

This page is about configuring analyzers for at least a whole folder or project. On suppressing just single analyzer violations see the ["Using the analyzers during development" page](UsingAnalyzersDuringDevelopment.md). For further details see the [official docs](https://docs.microsoft.com/en-us/visualstudio/code-quality/use-roslyn-analyzers).

## How to disable all analyzers for particular projects

Place a _Directory.Build.props_ file into the project's folder (or folder with set of projects) with the following contents:

```xml
<Project>
  <ItemGroup> 
      <Analyzer Remove="@(Analyzer)" /> 
  </ItemGroup>
</Project>
```

This will completely disable code analysis packages. To also disable .NET SDK analysis override them from an _.editorconfig_ file placed into the given project's folder. There you can disable any unwanted rules, like disabling .NET code style analysis completely:

```editorconfig
[*.cs]
dotnet_analyzer_diagnostic.category-Style.severity = none
```

## How to disable all analyzers during `dotnet build`

By default the `dotnet build` command runs analyzers and produces code analysis warnings if there are any but it makes the build slower. Pass the `/p:RunCodeAnalysis=false` parameter to disable analyzers during build, like:

```ps
dotnet build MySolution.sln /p:RunCodeAnalysis=false
```

## How to override analyzer configuration globally

If not all the configuration in this project is suitable for your solution then you can also override them globally. This way, the default configuration will be merged with your custom configuration and you can override any number of rules conveniently at one place for all projects in your solution.

### Overriding analyzer configuration from a _.globalconfig_ file

1. Create your own _.globalconfig_ file with your own rule configuration, similar to this project's _general.ruleset_ file (see [the official docs on this file format](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/configuration-files#global-analyzerconfig)).
2. Place the file into the folder under which you want the configuration to apply to all projects.
3. Now every rule you've defined in your _.globalconfig_ file will take precedence over the default ones. For everything else, the default ones will be applied.

Note that if you add your _.globalconfig_ file to the solution you'll get GUI support for it in Visual Studio and you'll be able to configure rules without manually editing the configuration text.

More complex configuration is also available:

- You can similarly add such _.globalconfig_ files to subfolders, to just override rules for projects in that folder.
- Such files can be placed outside of the folder hierarchy too and referenced from csproj or props files with the `<GlobalAnalyzerConfigFiles>` element, see [the docs](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/configuration-files#naming).
- You can further adjust the precedence of _.globalconfig_ files with their names and the `global_level` entry, see [the docs](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/configuration-files#precedence).

### Suppressing analyzers from code for a whole project

You can suppress specific analyzer rules for a whole project from code as well. Add a _GlobalSuppressions.cs_ file to your project, add `SuppressMessage` attributes to it like below:

```csharp
// This file is used by Code Analysis to maintain SuppressMessage attributes that are applied to this project.
// Project-level suppressions either have no target or are given a specific target and scoped to a namespace, type,
// member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Minor Code Smell",
    "S1199:Nested code blocks should not be used",
    Justification = "UI tests commonly have small inner blocks for managing one-off elements like dropdowns.",
    Scope = "module")]
```

### Overriding _.editorconfig_ rules

You can't as easily do the same as with ruleset files with _.editorconfig_ rules. [It's not possible to define explicit inheritance between _.editorconfig_ files](https://github.com/editorconfig/editorconfig/issues/236) so [the only option is to use the folder hierarchy](https://stackoverflow.com/questions/58543855/can-visual-studio-use-an-editorconfig-not-in-the-directory-hierarchy/58556556#58556556): The _Build.props_ file of this project copies the default _.editorconfig_ file into the solution root. If you put your projects below that in the folder hierarchy and use your own _.editorconfig_ there then the latter will take precedence and you can override the default rules. E.g. you can override certain analyzer rules for a whole folder (even a folder within a project) like this:

```editor-config
[*.{config,csproj,json,props,targets}]

indent_size = 4
```

While eventually all analyzer rules in the .NET ecosystem will live in _.editorconfig_ this is not the case yet. However, you can override _.editorconfig_ rules from a ruleset file: You can open the ruleset file in Visual Studio and under the `Microsoft.CodeAnalysis.CSharp.Features` section you'll also be able to configure each IDE\* rule.

### Overriding _stylecop.json_ and _SonarLint.xml_ configuration

The project includes further configuration for `StyleCop.Analyzers` and `SonarAnalyzer.CSharp` in their own formats. These files are referenced as `<AdditionalFiles>` elements in the _props_ files corresponding to .NET Core or later and .NET Framework. From your own _Directory.Build.props_ file you need to recreate the contents of the original _props_ file but reference your custom configuration file instead.
