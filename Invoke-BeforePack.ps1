# This script prevents NU5100 warning caused by the presence of these files
# - bin/Release/netstandard2.0/Lombiq.Analyzers.dll
# - obj/Release/netstandard2.0/Lombiq.Analyzers.dll
# The cause is probably that we don't actually do anything in the corresponding csproj, it only exists because we need
# an entry point for `dotnet pack` (unlike the older `nuget pack` tooling, which worked with just the .nuspec alone).

Get-ChildItem Lombiq.Analyzers/obj -Recurse -Include Lombiq.Analyzers.dll | Remove-Item

$release = Get-ChildItem Lombiq.Analyzers/bin/Release -Recurse -Include Lombiq.Analyzers.dll
$releasePdb = $release.FullName -replace 'dll$','pdb'
New-Item -Type Directory -Force lib
Move-Item $release lib

if (Test-Path $releasePdb)
{
    Move-Item $releasePdb lib
}
else
{
    Write-Warning "There is no $releasePdb file!"
}
