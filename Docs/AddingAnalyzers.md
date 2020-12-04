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
