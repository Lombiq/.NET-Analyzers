# Configuring analyzers



This page is about configuring analyzers for at least a whole folder or project. On suppressing just single analyzer violations see the ["Using the analyzers during development" page](UsingAnalyzersDuringDevelopment.md). For further details see the [official docs](https://docs.microsoft.com/en-us/visualstudio/code-quality/use-roslyn-analyzers).



## How to disable all analyzers for particular projects

Place a *Directory.Build.props* file into the project's folder (or folder with set of projects) with the following contents:

```xml
<Project>
  <ItemGroup> 
      <Analyzer Remove="@(Analyzer)" /> 
  </ItemGroup>
</Project>
```

This will completely disable code analysis packages. To also disable .NET SDK analysis override them from an *.editorconfig* file placed into the given project's folder. There you can disable any unwanted rules, like disabling .NET code style analysis completely:

```
[*.cs]
dotnet_analyzer_diagnostic.category-Style.severity = none
```


## How to disable analyzers during `dotnet build`

By default the `dotnet build` command runs analyzers and produces code analysis warnings if there are any but it makes the build slower. Pass the `/p:RunCodeAnalysis=false` parameter to disable analyzers during build, like:

```ps
dotnet build MySolution.sln /p:RunCodeAnalysis=false
```


## How to override analyzer configuration globally

If not all the configuration in this project is suitable for your solution then you can also override them globally. This way, the default configuration will be merged with your custom configuration and you can override any number of rules conveniently at one place for all projects in your solution.

### Overriding analyzer configuration from a ruleset file

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

Note that if you add your ruleset file to the solution you'll get GUI support for it in Visual Studio and you'll be able to configure rules without manually editing the XML.

You can similarly add such `ruleset` files to subfolders, to just override rules for projects in that folder.

### Suppressing analyzers from code for a whole project

You can suppress specific analyzer rules for a whole project from code as well. Add a *GlobalSuppressions.cs* file to your project, add `SuppressMessage` attributes to it like below:

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

### Overriding *.editorconfig* rules

You can't as easily do the same as with ruleset files with *.editorconfig* rules. [It's not possible to define explicit inheritance between *.editorconfig* files](https://github.com/editorconfig/editorconfig/issues/236) so [the only option is to use the folder hierarchy](https://stackoverflow.com/a/58556556/220230): The *Build.props* file of this project copies the default *.editorconfig* file into the solution root. If you put your projects below that in the folder hierarchy and use your own *.editorconfig* there then the latter will take precedence and you can override the default rules. E.g. you can override certain analyzer rules for a whole folder (even a folder within a project) like this:

```editor-config
# C# files
[*.cs]

# MA0016: Prefer return collection abstraction instead of implementation
dotnet_diagnostic.MA0016.severity = none
```

While eventually all analyzer rules in the .NET ecosystem will live in *.editorconfig* this is not the case yet. However, you can override *.editorconfig* rules from a ruleset file: You can open the ruleset file in Visual Studio and under the `Microsoft.CodeAnalysis.CSharp.Features` section you'll also be able to configure each IDE\* rule.

### Overriding *stylecop.json* and *SonarLint.xml* configuration

The project includes further configuration for `StyleCop.Analyzers` and `SonarAnalyzer.CSharp` in their own formats. These files are referenced as `<AdditionalFiles>` elements in the *props* files corresponding to .NET Core or later and .NET Framework. From your own *Directory.Build.props* file you need to recreate the contents of the original *props* file but reference your custom configuration file instead.
