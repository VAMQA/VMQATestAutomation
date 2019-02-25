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


configuration RunTests{ 
    
	 Param ($Machine)
     Node $Machine.NodeName
     {
        
            switch ($Machine.Roles)
			{
            
            'TestServer'
                {
                    Script ExecuteTests
                    {
                        SetScript = {
                       
									$deploymentpath = "$($using:root)\DroplocationContent"
							        $unique = $((Get-Date).ToString('yyyy-MM-dd-ss-fff'))
									
							        $testdllpath = join-path $deploymentpath "$($using:Machine.Testdll)"
									$testConfigPath = join-path $deploymentpath "$($using:Machine.TestConfiguration)"
									$Platform =  "$($using:Machine.Platform)"
									$Configuration = "$($using:Machine.Configuration)"
									$TeamProject = "$($using:Machine.TeamProject)"
									$BuildNumber = "$($using:Machine.BuildNumber)"
									$TFSCollectionUri = "https://tfs.ext.geicoddc.net/tfs/geico"
									$RunTitle = $BuildNumber + "RegRunner"
									$stdOutput = "$($deploymentpath)\$($unique)TestOutputLog.txt"
									$stdError = "$($deploymentpath)\$($unique)TestErrorLog.txt"
									$testCategory = "$($using:Machine.TestCategory)"
									$testURL = "$($using:Machine.TestURL)" 												
									$testcaseFilter = "TestCategory=$($testCategory)"
									$environment = "$($using:Machine.Environment)"
									$email = "$($using:Machine.Email)"
	        						$testtype = "$($using:Machine.TestType)"
  

									$MTMFile = join-path "$($deploymentpath)" "VSTest_Executed.txt"
									"MTM Executed." | out-file -FilePath $MTMFile -Append
                                
									write-verbose "testdllpath $($testdllpath)"
									write-verbose "platform: $($platform)"
									write-verbose "Configuration: $($Configuration)"
									write-verbose "TeamProject $($TeamProject)"
									write-verbose "BuildNumber: $($BuildNumber)"
									write-verbose "TFSCollectionUri: $($TFSCollectionUri)"
									write-verbose "RunTitle $($RunTitle)"
									write-verbose "stdOutput: $($stdOutput)"
									write-verbose "stdError: $($stdError)"
                                    write-verbose "test category: $($testCategory)"
									write-verbose "test url: $($testURL)"
								    write-verbose "test Config Path: $($testConfigPath)"
                                    write-Verbose "test filter: $($testcaseFilter)"
									write-verbose "Environment: $($environment)"
							        write-verbose "Email: $($email)"
									
								    if(!(test-path "$($testConfigPath)"))
	                                {
                                        write-verbose "Error: Test Configuration file not found.."
                                        exit 1
                                    }
									
                                    [xml]$xml = Get-Content "$($testConfigPath)" 

									<# Note: This looks for <Configuration><appSettings><Add key=ServerURL> and replaces the value of the EndPoint before running the tests. 
									Feel free to edit this logic and add your custom transforms here#> 
									
									$xml.configuration.appSettings.add| Foreach-Object {
										if($_.key -eq "ServerUrl")
										{
											$_.value = "$($testURL)"
										}
									}

									$xml.Save("$($testConfigPath)")
									#----Create json params-----

									$script = @{"Name"="Invoke-VSTest2013";
															"Arguments"=(
																		@{"Name"="-testdllpath";"Value"="$testdllpath"},
																		@{"Name"="-Platform";"Value"="$Platform"},
																		@{"Name"="-Configuration";"Value"="$Configuration"},
																		@{"Name"="-TeamProject";"Value"="$TeamProject"},
																		@{"Name"="-BuildNumber";"Value"="$BuildNumber"},
																		@{"Name"="-TFSCollectionUri";"Value"="$TFSCollectionUri"},
																		@{"Name"="-standardErrorPath";"Value"="$stdError"},
																		@{"Name"="-RunTitle";"Value"="$RunTitle"},
																		@{"Name"="-standardOutputPath";"Value"="$stdOutput"},
																		@{"Name"="-TestCaseFilter";"Value"="$testcaseFilter"},
																        @{"Name"="-Environment";"Value"="$environment"},
																		@{"Name"="-Email";"Value"="$email"},
																        @{"Name"="-TestType";"Value"="$testtype"}
																		#,@{"Name"="-Settings";"Value"="$testSettingspath"}
																	   )
															} | ConvertTo-Json -Compress

								$testProcessingDir = "C:\TestProcessing\Params"
								$fileName = "Param" + "$(Get-Date -Format HH.mm.ss.fff)" + ".tmp"
								$paramfilePath = join-path $testProcessingDir $fileName
								$paramfilejson = $paramfilePath + ".json"
								write-verbose "Writing Param file to: $paramfilePath"
								$script | out-file "$paramfilePath"
								rename-Item "$paramfilePath" "$paramfilejson"
								
								write-verbose "Running tests..."
								write-verbose "Check the test logs at following URLs"
								$baseurl = "https://gzecorenp1mtm02.geicoddc.net/Logs/"
								$baseDir = "C:\Packages\Plugins\Microsoft.Powershell.DSC\2.7.0.0\DSCWork\"

								$out = ("$($stdOutput)".Replace("$($baseDir)","")).Replace("\","/")
								$error = "$($stdError)".Replace("$($baseDir)","").Replace("\","/")

								$outURL = $baseurl + $out
								$errorURL = $baseurl + $error

								write-verbose $outURL
								write-verbose $errorURL
                        }
                        TestScript = { 
                        
                                $deploymentpath = "$($using:root)\DroplocationContent"
                                $MTMFile = join-path "$($deploymentpath)" "VSTest_Executed.txt"

                                (Test-Path "$MTMFile") 
                                
                                }
                        GetScript = {  }     
                   }
                    
                }
            }
     }
 }

 configuration RunLoadTests{ 
    
	 Param ($Machine)
     Node $Machine.NodeName
     {
        
            switch ($Machine.Roles)
			{
            
            'TestServer'
                {
                    Script ExecuteTests
                    {
                        SetScript = {
                       
									$deploymentpath = "$($using:root)\DroplocationContent"
							        $unique = $((Get-Date).ToString('yyyy-MM-dd-ss-fff'))

									$testConfigPath = join-path $deploymentpath "$($using:Machine.TestConfiguration)"
									$Platform =  "$($using:Machine.Platform)"
									$Configuration = "$($using:Machine.Configuration)"
									$TeamProject = "$($using:Machine.TeamProject)"
									$BuildNumber = "$($using:Machine.BuildNumber)"
									$TFSCollectionUri = "https://tfs.ext.geicoddc.net/tfs/geico"
									$stdOutput = "$($deploymentpath)\$($unique)TestOutputLogs.txt"
									$stdError = "$($deploymentpath)\$($unique)TestErrorLogs.txt"
									$testCategory = "$($using:Machine.TestCategory)"
									$testURL = "$($using:Machine.TestURL)" 												
									$environment = "$($using:Machine.Environment)"
									$email = "$($using:Machine.Email)"
                                    $mstestArguments = "$($using:Machine.MSTestArguments)"
        							$testtype = "$($using:Machine.TestType)"

							        $mstestArguments = $mstestArguments.Replace("[ROOT_DIR]", "$($deploymentpath)")

									$MTMFile = join-path "$($deploymentpath)" "LoadTests_Executed.txt"
									"MTM Executed." | out-file -FilePath $MTMFile -Append
                                
									write-verbose "platform: $($platform)"
									write-verbose "Configuration: $($Configuration)"
									write-verbose "TeamProject $($TeamProject)"
									write-verbose "BuildNumber: $($BuildNumber)"
									write-verbose "TFSCollectionUri: $($TFSCollectionUri)"
									write-verbose "stdOutput: $($stdOutput)"
									write-verbose "stdError: $($stdError)"
                                    write-verbose "test category: $($testCategory)"
									write-verbose "test url: $($testURL)"
								    write-verbose "test Config Path: $($testConfigPath)"
                                    write-verbose "Environment: $($environment)"
							        write-verbose "Email: $($email)"
                                    write-verbose "MSTestArguments: $($mstestArguments)"				

								    <#
                                    if(!(test-path "$($testConfigPath)"))
	                                {
                                        write-verbose "Error: Test Configuration file not found.."
                                        exit 1
                                    }
									
                                    [xml]$xml = Get-Content "$($testConfigPath)" 

									
									$xml.configuration.appSettings.add| Foreach-Object {
										if($_.key -eq "ServerUrl")
										{
											$_.value = "$($testURL)"
										}
									}

									$xml.Save("$($testConfigPath)")
                                    #>
									#----Create json params-----

									$script = @{"Name"="Invoke-MSTest2013";
															"Arguments"=(
																		@{"Name"="-MSTestArguments";"Value"="$($mstestArguments)"},
																		@{"Name"="-Platform";"Value"="$Platform"},
																		@{"Name"="-Configuration";"Value"="$Configuration"},
																		@{"Name"="-TeamProject";"Value"="$TeamProject"},
																		@{"Name"="-BuildNumber";"Value"="$BuildNumber"},
																		@{"Name"="-TFSCollectionUri";"Value"="$TFSCollectionUri"},
																		@{"Name"="-standardErrorPath";"Value"="$stdError"},
																		@{"Name"="-standardOutputPath";"Value"="$stdOutput"},
																		@{"Name"="-Environment";"Value"="$environment"},
																		@{"Name"="-Email";"Value"="$email"},
	        															@{"Name"="-TestType";"Value"="$testtype"}
   																	   )
															} | ConvertTo-Json -Compress

								$testProcessingDir = "C:\TestProcessing\Params"
								$fileName = "Param" + "$(Get-Date -Format HH.mm.ss.fff)" + ".tmp"
								$paramfilePath = join-path $testProcessingDir $fileName
								$paramfilejson = $paramfilePath + ".json"
								write-verbose "Writing Param file to: $paramfilePath"
								$script | out-file "$paramfilePath"
								rename-Item "$paramfilePath" "$paramfilejson"
								
								write-verbose "Running tests..."
								write-verbose "Check the test logs at following URLs"
								$baseurl = "https://gzecorenp1mtm02.geicoddc.net/Logs/"
								$baseDir = "C:\Packages\Plugins\Microsoft.Powershell.DSC\2.15.0.0\DSCWork\"

								$out = ("$($stdOutput)".Replace("$($baseDir)","")).Replace("\","/")
								$error = "$($stdError)".Replace("$($baseDir)","").Replace("\","/")

								$outURL = $baseurl + $out
								$errorURL = $baseurl + $error

								write-verbose $outURL
								write-verbose $errorURL
                        }
                        TestScript = { 
                        
                                $deploymentpath = "$($using:root)\DroplocationContent"
                                $MTMFile = join-path "$($deploymentpath)" "LoadTests_Executed.txt"

                                (Test-Path "$MTMFile") 
                                
                                }
                        GetScript = {  }     
                   }
                    
                }
            }
     }
 }

configuration RunMTMTests{ 
    
	 Param ($Machine)
     Node $Machine.NodeName
     {
        
            switch ($Machine.Roles)
			{
            
            'TestServer'
                {
                    Script ExecuteTests
                    {
                        SetScript = {
                       
									$deploymentpath = "$($using:root)\DroplocationContent"
							        $unique = $((Get-Date).ToString('yyyy-MM-dd-ss-fff'))
									
							        $testdllpath = join-path $deploymentpath "$($using:Machine.Testdll)"
									$testConfigPath = join-path $deploymentpath "$($using:Machine.TestConfiguration)"
									$TeamProject = "$($using:Machine.TeamProject)"
									$BuildNumber = "$($using:Machine.BuildNumber)"
									$TFSCollectionUri = "https://tfs.ext.geicoddc.net/tfs/geico"
									$RunTitle = $BuildNumber + " MTM Tests"
									$stdOutput = "$($deploymentpath)\$($unique)TestOutputLog.txt"
									$testURL = "$($using:Machine.TestURL)" 												
									$environment = "$($using:Machine.Environment)"
									$email = "$($using:Machine.Email)"
	        						$testtype = "$($using:Machine.TestType)"
                                    $PlanID = "$($using:Node.PlanID)"
                                    $SutieID = "$($using:Node.SuiteId)"
                                    $ConfigID = "$($using:Node.ConfigId)"
                                    $SettingsName = "$($using:Node.SettingsName)"
                                    $version = "$($using:Node.VSVersion)"

									$MTMFile = join-path "$($deploymentpath)" "MTM_Executed.txt"
									"MTM Executed." | out-file -FilePath $MTMFile -Append
                                
									write-verbose "testdllpath $($testdllpath)"
									write-verbose "TeamProject $($TeamProject)"
									write-verbose "BuildNumber: $($BuildNumber)"
									write-verbose "TFSCollectionUri: $($TFSCollectionUri)"
									write-verbose "RunTitle $($RunTitle)"
									write-verbose "stdOutput: $($stdOutput)"
									write-verbose "test url: $($testURL)"
								    write-verbose "test Config Path: $($testConfigPath)"
                                    write-verbose "Environment: $($environment)"
							        write-verbose "Email: $($email)"
                                    write-verbose "Test Type: $($testtype)"
                                    write-verbose "Plan id: $($PlanID)"
                                    write-verbose "Suite id: $($SutieID)"
                                    write-verbose "Config id: $($ConfigID)"
                                    write-verbose "Settings Name: $($SettingsName)"
                                    write-verbose "VS Version: $($version)"
									
								    if(!(test-path "$($testConfigPath)"))
	                                {
                                        write-verbose "Error: Test Configuration file not found.."
                                        exit 1
                                    }
									
                                    [xml]$xml = Get-Content "$($testConfigPath)" 

									<# Note: This looks for <Configuration><appSettings><Add key=ServerURL> and replaces the value of the EndPoint before running the tests. 
									Feel free to edit this logic and add your custom transforms here#> 
									
									$xml.configuration.appSettings.add| Foreach-Object {
										if($_.key -eq "ServerUrl")
										{
											$_.value = "$($testURL)"
										}
									}

									$xml.Save("$($testConfigPath)")
									#----Create json params-----

									$script = @{"Name"="Invoke-MTM";
															"Arguments"=(
																		@{"Name"="-Title";"Value"="$RunTitle"},
                                                                        @{"Name"="-PlanId";"Value"="$PlanID"},
                                                                        @{"Name"="-SuiteId";"Value"="$SutieID"},
                                                                        @{"Name"="-ConfigId";"Value"="$ConfigID"},
                                                                        @{"Name"="-Collection";"Value"="$TFSCollectionUri"},
                                                                        @{"Name"="-TeamProject";"Value"="$TeamProject"},
                                                                        @{"Name"="-BuildDirectory";"Value"="$deploymentpath"},
                                                                        @{"Name"="-BuildNumber";"Value"="$BuildNumber"},
                                                                        @{"Name"="-SettingsName";"Value"="$SettingsName"},
                                                                        @{"Name"="-logFilePath";"Value"="$stdOutput"},
                                                                        @{"Name"="-Environment";"Value"="$environment"},
																		@{"Name"="-Email";"Value"="$email"},
																        @{"Name"="-TestType";"Value"="$testtype"},
                                                                        @{"Name"="-Version";"Value"="$version"}
																		)
															} | ConvertTo-Json -Compress

								$testProcessingDir = "C:\TestProcessing\Params"
								$fileName = "Param" + "$(Get-Date -Format HH.mm.ss.fff)" + ".tmp"
								$paramfilePath = join-path $testProcessingDir $fileName
								$paramfilejson = $paramfilePath + ".json"
								write-verbose "Writing Param file to: $paramfilePath"
								$script | out-file "$paramfilePath"
								rename-Item "$paramfilePath" "$paramfilejson"
								
								write-verbose "Running tests..."
								write-verbose "Check the test logs at following URLs"
								$baseurl = "https://gzecorenp1mtm02.geicoddc.net/Logs/"
								$baseDir = "C:\Packages\Plugins\Microsoft.Powershell.DSC\2.7.0.0\DSCWork\"

								$out = ("$($stdOutput)".Replace("$($baseDir)","")).Replace("\","/")
								$error = "$($stdError)".Replace("$($baseDir)","").Replace("\","/")

								$outURL = $baseurl + $out
								$errorURL = $baseurl + $error

								write-verbose $outURL
								write-verbose $errorURL
                        }
                        TestScript = { 
                        
                                $deploymentpath = "$($using:root)\DroplocationContent"
                                $MTMFile = join-path "$($deploymentpath)" "MTM_Executed.txt"

                                (Test-Path "$MTMFile") 
                                
                                }
                        GetScript = {  }     
                   }
                    
                }
            }
     }
 }


Configuration TestConfiguration
{
    
    Node $AllNodes.NodeName
     {
        $root = "$($PSScriptRoot)"

		InitializeTargetServer init
        {
			
	    }
		
		if($ConfigurationData.NonNodeData.RunTests -eq $true)
		{
            RunTests runTests
			{
				Machine = $Node
                DependsOn = "[InitializeTargetServer]init"    
                   
			}
		}
		 if($ConfigurationData.NonNodeData.RunLoadTests -eq $true)
		{
            RunLoadTests runLoadTests
			{
				Machine = $Node
                DependsOn = "[InitializeTargetServer]init"    
                   
			}
		}
       if($ConfigurationData.NonNodeData.RunMTMTests -eq $true)
		{
            RunMTMTests runMTMTests
			{
				Machine = $Node
                DependsOn = "[InitializeTargetServer]init"    
                   
			}
		}
        
     }   
}

