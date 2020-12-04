# Configuring analyzers



## How to disable all analyzers for particular projects

Place a *Directory.Build.props* file into the project's folder (or folder with set of projects) with the following contents:

```xml
<Project>
  <ItemGroup> 
      <Analyzer Remove="@(Analyzer)" /> 
  </ItemGroup>
</Project>
```

This will completely disable code analysis.


## How to disable analyzers during `dotnet build`

By default the `dotnet build` command runs analyzers and produces code analysis warnings if there are any but it makes the build slower. Pass the `-p:RunCodeAnalysis=false` parameter to disable analyzers during build, like:

```ps
dotnet build MySolution.sln -p:RunCodeAnalysis=false
```


## How to override analyzer configuration globally

If not all the configuration in this project is suitable for your solution then you can also override them globally. This way, the default configuration will be merged with your custom configuration and you can override any number of rules conveniently at one place for all projects in your solution.

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
