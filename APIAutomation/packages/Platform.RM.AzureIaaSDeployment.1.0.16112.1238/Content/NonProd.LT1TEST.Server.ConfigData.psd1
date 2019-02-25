@{
 AllNodes = 
     @(
         @{
            NodeName = "$env:COMPUTERNAME";
            
			#Web Package related Parameters
			Roles = @('TestServer');
                   
			<#-----Test related parameters----#>
			
			<#This is the Platform value as you set in TFS Build definition#>
			Platform = "Any CPU";
            <#This is the Configuration value as you set in TFS Build definition#>
			Configuration = "Release";
			<#This is the TeamProject collection name where your tests are checked-in to TFS#>
            TeamProject = "MSI Customer Products";
			<#Test Results will be emailed to this address#>
			Email = "stahir@geico.com";
			TestType = "Load Tests";

			 <#-----Load Test related parameters----#>
			<#Space separated MSTest.exe arguments. You can specify multiple .loadtest file parameters. Check msdn for all arguments.#>
			<#[ROOT_DIR] will be replaced by runtime, keep it as is#>
			MSTestArguments = "/testcontainer:[ROOT_DIR]\LoadTest1.loadtest /testcontainer:[ROOT_DIR]\LoadTest2.loadtest /testsettings:[ROOT_DIR]\LoadTest.testsettings";
			            			      			  
			<#-----General Paramters----#>
			#Keep [BuildNumber] placeholder as is, it will be populated by runtime.
			BuildNumber = "[BuildNumber]"; 
			Environment = "LT1TEST";
			DeploymentLogsPath = "\\gzecorenp1clh04.geicoddc.net\DeploymentLogs";
         }
   );
     NonNodeData =
    @{
		RunTests = $false;
		RunLoadTests = $true;
		RunMTMTests = $false;
     } 
 }
