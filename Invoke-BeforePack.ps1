# This script tries to work around NuGet's validation caused by the presence of these files:
# - bin/Release/netstandard2.0/Lombiq.Analyzers.dll
# - obj/Release/netstandard2.0/Lombiq.Analyzers.dll
# The cause is probably that we don't actually do anything in the corresponding csproj, it only exists because we need
# an entry point for `dotnet pack` (unlike the older `nuget pack` tooling, which worked with just the .nuspec alone).

# Remove all .dll and other build output files from the directory to be packed.
Remove-Item Lombiq.Analyzers/bin -Recurse
Remove-Item Lombiq.Analyzers/obj/Release -Recurse
