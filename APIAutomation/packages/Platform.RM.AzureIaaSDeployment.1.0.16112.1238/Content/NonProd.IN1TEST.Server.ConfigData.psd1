@{
 AllNodes = 
     @(
         @{
            NodeName = "$env:COMPUTERNAME";
            
			#Web Package related Parameters
			Roles = @('TestServer');
                   
			<#-----Test related parameters----#>

			<#Name of the dll that contains your tests. It could be integration, smoke, regression or CodedUI Tests#>
			Testdll = "Geico.Platform.DuckCreek.RRScripts.dll";
	        TestURL = "https://gzeduckin1exp01.geicoddc.net/DuckCreek/DCTServer.aspx";
			<#Name of the test configuration file that you would like to transform. This is your App.config#>
			TestConfiguration = "Geico.Platform.DuckCreek.RRScripts.dll.config";
			<#If you would like limit test execution to a specific TestCategory, define it here#>
			TestCategory = "Umbrella Smoke Tests INTG";
            <#This is the Platform value as you set in TFS Build definition#>
			Platform = "Any CPU";
            <#This is the Configuration value as you set in TFS Build definition#>
			Configuration = "Release";
			<#This is the TeamProject collection name where your tests are checked-in to TFS#>
            TeamProject = "MSI Customer Products";
			<#Test Results will be emailed to this address#>
			Email = "stahir@geico.com";
			<#Valid values: Integration, Coded UI, Regression, Smoke#>
			 TestType = "Coded UI";
			 			            			      			  
			<#-----General Paramters----#>
			#Keep [BuildNumber] placeholder as is, it will be populated by runtime.
			BuildNumber = "[BuildNumber]"; 
			Environment = "IN1TEST";
			DeploymentLogsPath = "\\gzecorenp1clh04.geicoddc.net\DeploymentLogs";
         }
   );
     NonNodeData =
    @{
		RunTests = $true;
		RunLoadTests = $false;
		RunMTMTests = $false;
     } 
 }
