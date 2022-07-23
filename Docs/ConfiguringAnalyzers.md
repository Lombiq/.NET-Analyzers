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

## How to disable analyzers during `dotnet build`

By default the `dotnet build` command runs analyzers and produces code analysis warnings if there are any but it makes the build slower. Pass the `/p:RunCodeAnalysis=false` parameter to disable analyzers during build, like:

```ps
dotnet build MySolution.sln /p:RunCodeAnalysis=false
```

## How to override analyzer configuration globally

If not all the configuration in this project is suitable for your solution then you can also override them globally. This way, the default configuration will be merged with your custom configuration and you can override any number of rules conveniently at one place for all projects in your solution.

### Overriding analyzer configuration from a ruleset file

1. Create your own ruleset file, similar to this project's _general.ruleset_ file. Make sure the file name is all lower-case, because if you edit it in Visual Studio it will be converted anyway and that could cause problems on Unix-like systems. Add any rule configurations there that you want to override. Also, include this project's _general.ruleset_ file as a child, allowing its rules to be available by default. In the end you should have something like this (the included rules only serve as an example):

    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <RuleSet Name="General C# rules" ToolsVersion="16.0">
      <Include Path="tools\Lombiq.Analyzers\general.ruleset" Action="Default" />
      <Rules AnalyzerId="SecurityCodeScan" RuleNamespace="SecurityCodeScan">
        <Rule Id="SCS0005" Action="None" />
      </Rules>
      <Rules AnalyzerId="StyleCop.Analyzers" RuleNamespace="StyleCop.Analyzers">
        <Rule Id="SA1312" Action="Warning" />
      </Rules>
    </RuleSet>
    ```

2. In the _Directory.Build.props_ file of your solution add a reference to your own ruleset file, overriding the default:

    ```xml
    <PropertyGroup>
      <CodeAnalysisRuleSet>$(MSBuildThisFileDirectory)my.ruleset</CodeAnalysisRuleSet>
    </PropertyGroup>
    ```

3. Now every rule you've defined in _my.ruleset_ will take precedence over the default ones. For everything else, the default ones will be applied.

Note that if you add your ruleset file to the solution you'll get GUI support for it in Visual Studio and you'll be able to configure rules without manually editing the XML.

You can similarly add such `ruleset` files to subfolders, to just override rules for projects in that folder.

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
# C# files
[*.cs]

# MA0016: Prefer return collection abstraction instead of implementation
dotnet_diagnostic.MA0016.severity = none
```

While eventually all analyzer rules in the .NET ecosystem will live in _.editorconfig_ this is not the case yet. However, you can override _.editorconfig_ rules from a ruleset file: You can open the ruleset file in Visual Studio and under the `Microsoft.CodeAnalysis.CSharp.Features` section you'll also be able to configure each IDE\* rule.

### Overriding _stylecop.json_ and _SonarLint.xml_ configuration

The project includes further configuration for `StyleCop.Analyzers` and `SonarAnalyzer.CSharp` in their own formats. These files are referenced as `<AdditionalFiles>` elements in the _props_ files corresponding to .NET Core or later and .NET Framework. From your own _Directory.Build.props_ file you need to recreate the contents of the original _props_ file but reference your custom configuration file instead.
