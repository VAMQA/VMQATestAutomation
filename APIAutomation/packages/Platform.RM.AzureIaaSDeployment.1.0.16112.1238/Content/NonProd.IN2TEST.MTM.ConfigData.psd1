@{
 AllNodes = 
     @(
         @{
            NodeName = "$env:COMPUTERNAME";
            
			#Web Package related Parameters
			Roles = @('TestServer');
                   
			<#-----MTM related parameters----#>

			<#Name of the dll that contains your tests. It could be integration, smoke, regression or CodedUI Tests#>
			Testdll = "Geico.Platform.DuckCreek.RRScripts.dll";
	        TestURL = "https://gzeduckin1exp01.geicoddc.net/DuckCreek/DCTServer.aspx";
			<#Name of the test configuration file that you would like to transform. This is your App.config#>
			TestConfiguration = "Geico.Platform.DuckCreek.RRScripts.dll.config";
			<#This is the TeamProject collection name where your tests are checked-in to TFS#>
            TeamProject = "MSI Foundation";
			<#Test Results will be emailed to this address#>
			Email = "stahir@geico.com";
			<#Valid values: Integration, Coded UI, Regression, Smoke#>
			TestType = "MTM";
			<#MTM Suite id#>
			SuiteId	= "266622";
            <#MTM Config id#>
			ConfigId = "46";
			<#MTM Plan id#>
			PlanID = "266620";
			<#MTM Settings Name#>
			SettingsName = "WebLightweight";
			<#Visual Studio version#>
			VSVersion = "2013";
			  			            			      			  
			<#-----General Paramters----#>
			#Keep [BuildNumber] placeholder as is, it will be populated by runtime.
			BuildNumber = "[BuildNumber]"; 
			Environment = "IN1TEST";
			DeploymentLogsPath = "\\gzecorenp1clh04.geicoddc.net\DeploymentLogs";
         }
   );
     NonNodeData =
    @{
		RunTests = $false;
		RunLoadTests = $false;
		RunMTMTests = $true;
     } 
 }
