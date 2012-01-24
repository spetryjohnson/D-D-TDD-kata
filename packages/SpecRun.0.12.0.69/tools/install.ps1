param($installPath, $toolsPath, $package, $project)

$project.ProjectItems.Item("Default.srprofile").Properties.Item("CopyToOutputDirectory").Value = 2

$specflowPackage = Get-Package -Filter SpecFlow -First 1

$specflowPackageToolsFolder = $installPath + "\..\SpecFlow." + $specflowPackage.Version.ToString() + "\tools"

$specFlowPlugin = $toolsPath + "\SpecFlowPlugin\TechTalk.SpecRun.SpecFlowPlugin.Generator.dll"

Copy-Item $specFlowPlugin -destination $specflowPackageToolsFolder
