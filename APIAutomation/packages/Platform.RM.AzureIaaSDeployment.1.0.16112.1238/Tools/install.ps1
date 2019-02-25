param ($installPath, $toolsPath, $package, $project)

$files = @("DeploymentMetaData.psd1", "NonProd.IN2TEST.MTM.ConfigData.psd1","NonProd.IN1TEST.Server.ConfigData.psd1","NonProd.LT1TEST.Server.ConfigData.psd1", "NonProd.INTG.Server.ConfigData.psd1", "NonProd.RMConfiguration.ps1", "NonProd.TestConfiguration.ps1" ,"SplunkForwarder.conf", "AfterBuild.ps1")

Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'

$ProjectCollection = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection

$BuildProject = $ProjectCollection.GetLoadedProjects($Project.FullName) | Select-Object -First 1

$files | foreach {
	$TargetFileName = $_
	$prjBuildActionContent = 2
	$ProjectItem = $Project.ProjectItems.Item($TargetFileName)
	$Property = $ProjectItem.Properties.Item('BuildAction')
	$Property.Value = $prjBuildActionContent
	
	$prjCopytoOutputDir = 1
	$Property = $ProjectItem.Properties.Item('CopyToOutputDirectory')
	$Property.Value = $prjCopytoOutputDir
}

$Project.Save()

