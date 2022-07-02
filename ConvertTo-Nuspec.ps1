param($Version)

$templateFileName = [System.IO.Path]::Combine($PWD, 'Lombiq.Analyzers', 'Lombiq.Analyzers.nuspec.template')
$nuspec = [xml](Get-Content $templateFileName)
$group = $nuspec.package.metadata.dependencies.group

foreach($dependency in ([xml](Get-Content "CommonPackages.props")).Project.ItemGroup.AnalyzerPackage)
{
    $id = $dependency.Include
    if (-not $id) { continue }

    $node = $nuspec.CreateElement('dependency')
    $node.SetAttribute('id', $id)
    $node.SetAttribute('version', $dependency.Version)

    $group.AppendChild($node)
}

$nuspec.Save([System.IO.Path]::Combine($PWD, 'Lombiq.Analyzers.nuspec'))
