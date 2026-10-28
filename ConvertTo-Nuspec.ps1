param($Version)

function Read-Xml([string]$File) { [xml](Get-Content $File) }

function Add-Dependency($Xml, $Id, $Version)
{
    if (-not $Id -or -not $Version) { return }

    $node = $Xml.CreateElement('dependency', $Xml.package.NamespaceURI)
    $node.SetAttribute('id', $Id)
    $node.SetAttribute('version', $Version)

    $dependencies = $Xml.package.metadata.dependencies
    $dependencies.AppendChild($node)
}

$packages = @(
    'Lombiq.Analyzers',
    'Lombiq.Analyzers.NetFx',
    'Lombiq.Analyzers.OrchardCore',
    'Lombiq.Analyzers.Orchard1',
    'Lombiq.Analyzers.VisualStudioExtension'
)

# Iterate through the packages to replace
foreach ($package in $packages)
{
    $projectPath = Join-Path $PWD $package
    $nuspec = Read-Xml (Join-Path $projectPath "$package.nuspec.template")

    $nuspec.package.metadata.GetElementsByTagName('version')[0].InnerXml = $Version

    # AnalyzerPackages.props is optional.
    $analyzerPackagesPropsFile = Join-Path $projectPath 'AnalyzerPackages.props'
    if (Test-Path $analyzerPackagesPropsFile)
    {
        foreach ($dependency in (Read-Xml $analyzerPackagesPropsFile).Project.ItemGroup.AnalyzerPackage)
        {
            Add-Dependency -Xml $nuspec -Id $dependency.Include -Version $dependency.Version
        }
    }

    $outputPath = Join-Path $projectPath "$package.nuspec"
    $nuspec.Save($outputPath)
}
