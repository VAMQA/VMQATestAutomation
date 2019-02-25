param($StagePackage ="no" )
trap
{
    Write-Error $_
	cd $PSScriptRoot
    exit 1
}

function StagePackage($BuildNumber, $DropLocation, $PackagePath, $LogPath)
{
	write "Staging package..."
    
    md "$($LogPath)" -ErrorAction SilentlyContinue | Out-Null

    try{
                                
        $script = @{"Name"="Stage-Package";
                            "Arguments"=(
                                        @{"Name"="-BuildNumber";"Value"="$($BuildNumber)"},
                                        @{"Name"="-DropLocation";"Value"="$($DropLocation)"},
                                        @{"Name"="-PackagePath";"Value"="$($PackagePath)"},
                                        @{"Name"="-LogPath";"Value"="$($LogPath)\StagePackage.txt"}
                                       )
                            } | ConvertTo-Json -Compress

        Add-Type -AssemblyName System.Net.Http
        $mediaType = New-Object System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
        $handler = new-object System.Net.Http.HttpClientHandler
        $handler.UseDefaultCredentials = $true
        $wc = New-Object System.Net.Http.HttpClient($handler)

        $wc.Timeout = New-Object System.TimeSpan(0,20,0);
        $requestContent = New-Object System.Net.Http.StringContent($script, [System.Text.Encoding]::UTF8, "application/json");

        "Invoking web api with following parameters $script" | out-file "$($LogPath)\StagePackage.txt" -Append    
        
        $task = $wc.PostAsync("https://GZCO-DEPLOY002.dev.geicoddc.net/WebAPI/api/script", $requestContent);
        $task.Wait();
        "$($task.Result)" | out-File "$($LogPath)\StagePackage.txt" -Append 
        if(-not $task.Result.IsSuccessStatusCode){
            throw $task.Result
        }

        write "Package has been staged, check the rmclient logs to confirm."
    }
    catch [exception]
    {
         $ex = $_
        "$ex" | out-File "$($LogPath)\StagePackage.txt" -Append 
        "$($ex.GetType().FullName)" | out-File "$($LogPath)\StagePackage.txt" -Append
        "$($ex.Exception)" | out-File "$($LogPath)\StagePackage.txt" -Append
        "$($ex.Exception.GetType().FullName)" | out-File "$($LogPath)\StagePackage.txt" -Append
        "$($ex.Exception.Message)" | out-File "$($LogPath)\StagePackage.txt" -Append
        throw $ex
    }
}



#---Stage Package---#

if($StagePackage -eq "StagePackage")
{
	write-Warning $StagePackage
	
	$bindir = $env:TF_BUILD_BINARIESDIRECTORY 
    $droplocation = $env:TF_BUILD_DROPLOCATION 
    
    Copy-Item -Path "$bindir\*" -Destination "$droplocation" –Recurse -Force
	
	if($StagePackage -eq "StagePackage")
	{
		$LogPath = "\\GZCO-RMW001.dev.geicoddc.net\RMLogs\$($env:TF_BUILD_BUILDNUMBER)"

		StagePackage -BuildNumber "$($env:TF_BUILD_BUILDNUMBER)" -DropLocation "$($env:TF_BUILD_DROPLOCATION)" -PackagePath  "$($env:TF_BUILD_DROPLOCATION)\" -LogPath "$($LogPath)"
	}
}



cd $PSScriptRoot

exit 0
