param($Version)

$projectPath = Join-Path $PWD Lombiq.Analyzers
function Read-Xml([string]$File) { [xml](Get-Content (Join-Path $projectPath $File)) }

$nuspec = Read-Xml Lombiq.Analyzers.nuspec.template
$dependencies = $nuspec.package.metadata.dependencies

$nuspec.package.metadata.GetElementsByTagName('version')[0].InnerXml = $Version

foreach($dependency in (Read-Xml CommonPackages.props).Project.ItemGroup.AnalyzerPackage)
{
    $id = $dependency.Include
    if (-not $id) { continue }

    $node = $nuspec.CreateElement('dependency')
    $node.SetAttribute('id', $id)
    $node.SetAttribute('version', $dependency.Version)

    $dependencies.AppendChild($node)
}

$outputPath = Join-Path $projectPath Lombiq.Analyzers.nuspec
$nuspec.Save($outputPath)
