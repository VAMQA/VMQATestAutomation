@{
 AllNodes = 
     @(
         @{
            NodeName = "$env:COMPUTERNAME";
            
			<#-----Web Package related Parameters-----#>

			<#This is SetParam file name produced by the TFS build
			 Example: \\gzco-file001\builddrop\V.12_Release.Plat.DuckCreek\V.12_Release.Plat.DuckCreek.56.zip\V.12_Release.Plat.DuckCreek.56\_PublishedWebsites\Server_Package\Server.INTG.SetParameters.xml
			 #>
			WebUISetParamFileName = "Server.INTG.SetParameters.xml";
	        Roles = @('WebServer');
			<#This is the Web Package file name generated on the drop location. Edit the value
			 Example: \\gzco-file001\builddrop\V.12_Release.Plat.DuckCreek\V.12_Release.Plat.DuckCreek.56.zip\V.12_Release.Plat.DuckCreek.56\_PublishedWebsites\Server_Package\Server.zip
			 #>
            PackageName = "Server.zip"; 
			<#This is folder name, inside _PublishedWebsites directory on the drop location, where web package zip file exists
			 Example: \\gzco-file001\builddrop\V.12_Release.Plat.DuckCreek\V.12_Release.Plat.DuckCreek.56.zip\V.12_Release.Plat.DuckCreek.56\_PublishedWebsites\Server_Package
			 #>
            PackageRootDirectory = "Server_Package";
			
			<#-----Splunk Related Parameters-----#>

			CloudServiceName = "Your CloudService Name Goes Here. This is the Cloud Service Name for your VM where Splunk is setup";
			
			<#Note: ComponentName - once defined for a given application needs to be consistent for all environments and its case sensitive. This is current limitation of DevOps Dashboard.#>

            ComponentName = "Your Application Component Name";
			ApplicationShortName = "Your Application Short Name";
			ApplicationName = "SPLUNK Index name goes here";
			SplunkConfigurationFileName = "SplunkForwarder.conf";
			
            <#Please ensure your application is publishing logs to F:\Logs#>
			RoleRoot = "F:\Logs";


			<#-----Windows Service related Parameters-----#>

			<#This is SetParam file name produced by the TFS build#>
			ServiceSetParamFileName = "ActivityQueueProcessor.FT1.SetParameters.xml";			
            ServiceName = "Activity Processor"; 
			<#This is destination path on windows service on target VM#>
            BinaryPath = "C:\Jobs\ActivityQueueProcessor\Geico.Applications.Integration.UmbrellaDownstream.ActivityQueueProcessor.exe";
            startUpType = "Manual";            
            LogonUsername = "GEICODDC\SVC-MSI-UMB-AZ-FTRK";
            LoginPassword = 'serviceAccountPassword';
            ServiceDescription = "The Activity Queue processor Service";            
            <#This is your windows service config file name#>
			ServiceConfigFileName = "Geico.Applications.Integration.UmbrellaDownstream.ActivityQueueProcessor.exe.config";
            TransformStandardOuputFileName = "ConfigurationTransform_StdOutput.txt";
            TransformErrorOuputFileName = "ConfigurationTransform_StdError.txt";
			<#This is the installation path of windows service on the target VM#>
            InstallationPath = "C:\Jobs\ActivityQueueProcessor";
			            			  
			<#-----General Paramters----#>
			#Keep [BuildNumber] placeholder as is, it will be populated by runtime.
			BuildNumber = "[BuildNumber]"; 
			Environment = "INTG";
			DeploymentLogsPath = "\\gzecorenp1clh04.geicoddc.net\DeploymentLogs";
         }
   );
     NonNodeData =
    @{
		DeployWebsite = $true;
		DeployWinodwsService = $true;
		SetupSplunk = $true
     } 
 }
