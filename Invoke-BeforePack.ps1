# This script tries to work around NuGet's validation caused by the presence of these files:
# - bin/Release/netstandard2.0/Lombiq.Analyzers.dll
# - obj/Release/netstandard2.0/Lombiq.Analyzers.dll
# The cause is probably that we don't actually do anything in the corresponding csproj, it only exists because we need
# an entry point for `dotnet pack` (unlike the older `nuget pack` tooling, which worked with just the .nuspec alone).

$framework = "netstandard2.0"
New-Item -Type Directory -Force Lombiq.Analyzers/lib/$framework
Copy-Item Lombiq.Analyzers/bin/Release/$framework/Lombiq.Analyzers.dll -Destination Lombiq.Analyzers/lib/$framework
Copy-Item Lombiq.Analyzers/bin/Release/$framework/Lombiq.Analyzers.dll -Destination Lombiq.Analyzers/lib/$framework