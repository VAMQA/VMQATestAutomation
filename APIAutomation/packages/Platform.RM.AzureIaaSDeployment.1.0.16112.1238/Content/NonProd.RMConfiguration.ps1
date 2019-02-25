Configuration InitializeTargetServer
{
        Script UnzipPackage
        {
            SetScript = {
                  
                        Add-Type -AssemblyName "System.IO.Compression.FileSystem"
                               
                        [System.Reflection.Assembly]::LoadWithPartialName('System.IO.Compression.FileSystem')
                        [System.IO.Compression.ZipFile]::ExtractToDirectory("$($using:root)\Package.zip", "$($using:root)\DropLocationContent")
                 
              }
                        
            TestScript = { $false }
            GetScript = { <# This must return a hash table #> }          
                        
        }  

        Script UnzipDSCTools
        {
            SetScript = {
                  
                        Add-Type -AssemblyName "System.IO.Compression.FileSystem"
                               
                        [System.Reflection.Assembly]::LoadWithPartialName('System.IO.Compression.FileSystem')
                        [System.IO.Compression.ZipFile]::ExtractToDirectory("$($using:root)\DSCTools.zip", "$($using:root)\DropLocationContent\Tools")
              }
                        
            TestScript = { $false }
            GetScript = { <# This must return a hash table #> }   
            DependsOn = "[Script]UnzipPackage"       
                        
        }
            
}

configuration DeployWebSite{ 
    
	 Param ($Machine)
     
     Node $Machine.NodeName
     {
        
            switch ($Machine.Roles)
			{
                'WebServer'
                {

                    File LogsDir {
                        DestinationPath = "$($root)\DroplocationContent\Logs"
                        Ensure = "Present"
                        Type = 'Directory'
                    }

                    Script WebDeployPackage
                    {
                       SetScript = {
                       
                                $deploymentpath = "$($using:root)\DroplocationContent"
                                $logsPath = join-path $deploymentpath "Logs"
				                								
                                $webdeployPath = join-path $deploymentpath "Tools\Web"
                                # Modify file paths below 
								$webPackagePath = join-path $deploymentpath "$($using:Machine.PackageRootDirectory)\$($using:Machine.PackageName)"
                                $setParamPath = join-path $deploymentpath "$($using:Machine.PackageRootDirectory)\$($using:Machine.WebUISetParamFileName)"
								$name = "$($using:Machine.NodeName)" 
                                $stdOutput = join-path $($logsPath) "WebDeploy$($name)StdOutput.txt"
                                $stdError = join-path $($logsPath) "WebDeploy$($name)StdError.txt"
    							
						        write-verbose "Deploying package $($webPackagePath) with $($setParamPath) configuration file"

                            . $webdeployPath/WebDeploy.ps1 -computerName $name -webPackagePath $webPackagePath -setParamPath $setParamPath -standardOutputPath $stdOutput -standardErrorPath $stdError
                        
                       }
                        TestScript = { $false }
                        GetScript = {  }
                        DependsOn = "[File]LogsDir" 
					}
                    
                    File CopyLogs {
                        DestinationPath = "$($Machine.DeploymentLogsPath)\$($Machine.BuildNumber)\$($Machine.Environment)"
                        Ensure = "Present"
                        Type = 'Directory'
                        Recurse = $true
                        Force = $true
                        SourcePath = "$($root)\DroplocationContent\Logs"
                        #Credential = $Credentials
						Checksum = "modifiedDate" 
                        DependsOn = "[Script]WebDeployPackage" 
                    }
                }
            }
        }
 }


 configuration DeployWindowsService{ 
	
  Param ($Machine, $ServiceName, $ConfigFileName, $ServiceSetParamFileName, $StandardOuputFileName, $ErrorOuputFileName, $InstallationPath, $ServiceDescription, $BinaryPath, $LogonUsername, $LoginPassword, $startUpType)
	 Node $Machine.NodeName
	 {

	Script InstallService
		{
			SetScript = {
				  
						   Write-Verbose "Service Instllation started :: $($using:ServiceName)" 
						   $SourceLocation = "$($using:root)\DropLocationContent"  
						   $deploymentpath = "$($using:root)\DroplocationContent"         
						   $dscToolsPath = "$($using:root)\DroplocationContent"
						   $configTransformPath = join-path "$($dscToolsPath)" "Tools\ConfigurationTransform"
						   $serviceFramworkPath = join-path "$($dscToolsPath)" "Tools\WindowsService"

						   Write-Verbose "Source Location:: $($SourceLocation)"       
						   Write-Verbose "dsc Tools Path :: $($dscToolsPath)"
						   Write-Verbose "Config Transformation Framework Path :: $($configTransformPath)"
						   Write-Verbose "Service FrameWork Path :: $($serviceFramworkPath)"				
		
						 	# Service Stopped
						   if (Get-Service $($using:ServiceName) -ErrorAction SilentlyContinue)
						   {
							. $serviceFramworkPath\ManageWindowsServices.ps1 -Action "stop" -ServiceName $($using:ServiceName) 
							Write-Verbose "Step 1 :: Service Stopped :: $($using:ServiceName)" 
						   }

							# Configuration transformation 
       
						    $publishProfilePath = join-path "$($deploymentpath)" "$($using:ConfigFileName)"
						    $setParamPath = join-path "$($deploymentpath)" "$($using:ServiceSetParamFileName)"        
						    $stdOutput = join-path  "$($deploymentpath)" "$($using:StandardOuputFileName)"
						    $stdError = join-path  "$($deploymentpath)" "$($using:ErrorOuputFileName)"

						    Write-Verbose "Source XML Config file Path :: $($publishProfilePath)"
						    Write-Verbose "Parameter file Path :: $($setParamPath)"
						    Write-Verbose "Error file Path :: $($stdError)"
						    Write-Verbose "Output file Path :: $($stdOutput)"

						    . $configTransformPath/ConfigurationTransform.ps1 -sourceXMLPath "$($publishProfilePath)" -destinationXMLPath "$($publishProfilePath)" -setParmXMlPath "$($setParamPath)" -standardOutputPath "$($stdOutput)" -standardErrorPath "$($stdError)"  

						    Write-Verbose "Step 2 :: Config Transformation completed"
                          

							# Source Code copy operation			  
                          
						    If(!(Test-Path $($using:InstallationPath)))
							{
								New-Item -ItemType Directory -Path $($using:InstallationPath) -Force | Out-Null
							}

							$Path = join-path "$($SourceLocation)" "*"

						    Copy-Item  -Path  $Path  -Destination "$($using:InstallationPath)" -include "*.exe","*.dll","*.config" -Force

						    Write-Verbose "Step 3 :: Source Code copy operation completed"   
                          

				           # Service creation
						   
						   if (!(Get-Service $($using:ServiceName) -ErrorAction SilentlyContinue))
						   {
								. $serviceFramworkPath\ManageWindowsServices.ps1 -Action "Create" -ServiceName $($using:ServiceName) -Description $($using:ServiceDescription) -BinPath $($using:BinaryPath) -User $($using:LogonUsername) -Password $($using:LoginPassword) -StartMode $($using:startUpType)              
								Write-Verbose "Step 3 (Optional) Service created :: $($using:ServiceName)"
						   }
						   
                            if (Get-Service $($using:ServiceName) -ErrorAction SilentlyContinue)
							{
		
								. $serviceFramworkPath\ManageWindowsServices.ps1 -Action "Start" -ServiceName $($using:ServiceName)
								Write-Verbose "Step 4 Service Started :: $($using:ServiceName)" 		
							}

                            Write-Verbose "Service Instllation Completed :: $($using:ServiceName)"	
								
				 
						}
						
			TestScript = { $false }
			GetScript = { <# This must return a hash table #> }          
						
		}  
	  }
 }

 Configuration SplunkForwarder
{
     Param ($Machine)
     Node $Machine.NodeName
     {
        
        Script UpdateSplunkConfiguration
        {
            SetScript = {

                        if(!(test-path "C:\Windows\System32\WindowsPowerShell\v1.0\Modules\SplunkForwarder\SplunkForwarder.psm1"))
                        {
                            write-verbose "Exception: Splunk PS Module is not installed. Please check w/ DevOps team."
                            throw "Exception: Splunk PS Module is not installed. Please check w/ DevOps team."
                            exit 1    
                        }

                        Import-Module C:\Windows\System32\WindowsPowerShell\v1.0\Modules\SplunkForwarder\SplunkForwarder.psm1 -Force

                        <# These Param values will be injected from ConfigData #>
                        $deploymentpath = "$($using:root)\DroplocationContent"

                        $Environment =  "$($using:Machine.Environment)"
                        $CloudServiceName =  "$($using:Machine.CloudServiceName)"
                        $ComponentName =  "$($using:Machine.ComponentName)"
                        $ApplicationShortName =  "$($using:Machine.ApplicationShortName)"
                        $ApplicationName =  "$($using:Machine.ApplicationName)"
                        $inputsConf =  "$($using:Machine.SplunkConfigurationFileName)"
                        $roleRoot =  "$($using:Machine.RoleRoot)"
                        $inputsConf = join-path "$($deploymentpath)" "$($inputsConf)"

                        write-verbose "Environment $($Environment)"
                        write-verbose "CloudServiceName $($CloudServiceName)"
                        write-verbose "ComponentName $($ComponentName)"
                        write-verbose "ApplicationShortName $($ApplicationShortName)"
                        write-verbose "ApplicationName $($ApplicationName)"
                        write-verbose "inputsConf $($inputsConf)"
                        write-verbose "roleRoot $($roleRoot)"

                        if(!(test-path "$($inputsConf)"))
                        {
                            write-verbose "Exception: Splunk configuration file not found: $($inputsConf) Please check if the file was checked-in to TFS."
                            throw "Exception: Splunk configuration file not found: $($inputsConf) Please check if the file was checked-in to TFS."
                            exit 1
                        }

                        Set-InputsConf -EnvironmentName "$($Environment)" -CloudServiceName "$($CloudServiceName)" -ComponentName "$($ComponentName)" -ApplicationShortName "$($ApplicationShortName)" -ApplicationName "$($ApplicationName)" -inputsConf "$($inputsConf)" -roleRoot "$($roleRoot)"
                        }
                        
            TestScript = { $false }
            GetScript = { <# This must return a hash table #> }          
        }
     }

}

Configuration RMConfiguration
{
    Node $AllNodes.NodeName
     {
        $root = "$($PSScriptRoot)"

		InitializeTargetServer init
        {
			
	    }
		
	
		if($ConfigurationData.NonNodeData.DeployWebsite -eq $true)
		{
			DeployWebsite deployweb
			{
				Machine = $Node
                DependsOn = "[InitializeTargetServer]init"       
			}
		}

		 if ($ConfigurationData.NonNodeData.DeployWinodwsService -eq $true)
		 {
				DeployWindowsService deployService
				{
					Machine = $Node
                    ServiceName = $Node.ServiceName
                    ConfigFileName = $Node.ServiceConfigFileName
                    ServiceSetParamFileName = $Node.ServiceSetParamFileName
                    StandardOuputFileName = $Node.TransformStandardOuputFileName
                    ErrorOuputFileName = $Node.TransformErrorOuputFileName
                    InstallationPath = $Node.InstallationPath
                    ServiceDescription = $Node.ServiceDescription
                    BinaryPath = $Node.BinaryPath
                    LogonUsername = $Node.LogonUsername
                    LoginPassword = $Node.LoginPassword
                    startUpType = $Node.startUpType
					DependsOn = "[InitializeTargetServer]init"       
				}               
		  }

		 if($ConfigurationData.NonNodeData.SetupSplunk -eq $true)
		{
            SplunkForwarder splunkforwarder
			{
				Machine = $Node
                DependsOn = "[InitializeTargetServer]init"    
                   
			}
		}
         
     }
	  
}

