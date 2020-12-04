# Using the analyzers during command line builds




The following notes are useful if you're building not from within and IDE but from the command line, like in a CI environment.


## Showing analyzer warnings during `dotnet build`

Analyzer warnings will show up in the build output of `dotnet build` regardless of the `RunAnalyzersDuringBuild` config in *Build.props*. Note though, that this will only happen if it's a fresh, clean build; otherwise if you're building an already built solution use the `--no-incremental` switch to make analyzer warnings appear:

```ps
dotnet build MySolution.sln --no-incremental
```

If you want analyzer violations to fail the build (recommended) then also use `TreatWarningsAsErrors`:

```ps
dotnet build MySolution.sln --no-incremental /p:TreatWarningsAsErrors=true
```


## .NET 5 code style analysis

If you want code style analysis configured in *.editorconfig* (i.e. IDE* rules, this is not applicable to the others) to be checked during build too (it's already checked during editing) then you'll need to use the .NET 5 SDK or later [and configure `EnforceCodeStyleInBuild`](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview#code-style-analysis). This is not enabled in this project globally, not to slow down Visual Studio builds. However, you can enable it during `dotnet build` by using two switches like following:

```ps
dotnet build MySolution.sln --no-incremental /p:EnforceCodeStyleInBuild=true /p:RunAnalyzersDuringBuild=true
```

Our recommendation is to use it together with `TreatWarningsAsErrors` but do note that for code style analysis warnings you also have to specify `-warnaserror` (this is not needed for the other analyzers):

```ps
dotnet build MySolution.sln --no-incremental -warnaserror /p:TreatWarningsAsErrors=true /p:EnforceCodeStyleInBuild=true /p:RunAnalyzersDuringBuild=true
```

Or if you only want to see the errors and not the full build output (including e.g. NuGet restores, build messages):

```ps
dotnet build MySolution.sln --no-incremental -warnaserror /p:TreatWarningsAsErrors=true /p:EnforceCodeStyleInBuild=true /p:RunAnalyzersDuringBuild=true -nologo -consoleLoggerParameters:NoSummary -verbosity:quiet
```

Note that code style analysis is experimental in the .NET 5 SDK and [may change in later versions](https://github.com/dotnet/roslyn/issues/49044).
