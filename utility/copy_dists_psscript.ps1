$easyimgur_folder = (Get-Item (Split-Path $script:MyInvocation.MyCommand.Path)).parent.FullName
$destination_folder = $easyimgur_folder + "\dist\"
$bin_folder = $easyimgur_folder + "\EasyImgur\bin\"
$utility_content_source_folder = $easyimgur_folder + "\utility\content\"

$incremented_version = "n"
while ($incremented_version -ne "yes")
{
	Write-Host "------------------------------------------------------"
	$incremented_version = Read-Host "DID YOU INCREMENT THE VERSION NUMBER IN ASSEMBLYINFO.CS?"
	Write-Host "------------------------------------------------------"
}


# Each configuration will have its target files copied to a config-specific output folder
$configs = @{"Release" = @(($bin_folder + "Release\EasyImgur.exe"), ($utility_content_source_folder + "EasyImgur Portable.bat"));
             "Debug"   = @(($bin_folder + "Debug\EasyImgur.exe"), ($utility_content_source_folder + "EasyImgur Portable.bat"))}

ForEach ($config in $configs.GetEnumerator())
{
    Write-Host "======================="
    Write-Host ("Config '" + $config.Name + "':")

    ForEach ($file in $config.Value)
    {
	    Write-Host "-----------------------"	    
        Write-Host ("    File: " + $file)        

        $filename = [System.IO.Path]::GetFileName($file)

	    $full_destination_path = $destination_folder + $config.Name + "\" + $filename
	    $full_source_path = $file
	
	    $destination_timestamp 	= Get-Date -Date "1970-01-01 00:00:00Z"
	    $source_timestamp 		= Get-Date -Date "1970-01-01 00:00:00Z"

	    if (Test-Path $full_destination_path)
	    {
		    $destination_timestamp 	= [datetime](Get-ItemProperty -Path $full_destination_path -Name LastWriteTime).lastwritetime
	    }
	    if (Test-Path $full_source_path)
	    {
		    $source_timestamp 		= [datetime](Get-ItemProperty -Path $full_source_path -Name LastWriteTime).lastwritetime
	    }
	    else
	    {
		    Write-Error ("Source file does not exist: " + $file)
		    continue
	    }
	
	    if ($source_timestamp -gt $destination_timestamp)
	    {
		    Write-Host ("Detected newer source, copied file: " + $file)

            # "Touch" the destination file, which will create a new empty file and associated folder structure if it doesn't exist yet. If we omit this and the folders don't exist, then
            # Copy-Item will raise an exception. Also note that New-Item by default has output, but we don't want to see that so we pipe it to Out-Null
            New-Item -ItemType File -Path $full_destination_path -Force | Out-Null
		    Copy-Item $full_source_path $full_destination_path
	    }
	    else
	    {
		    Write-Warning (	"Source is not newer than destination, file not copied (did you forget to recompile?): `n`t" + 
						    $file + 
						    "`n`tSource: [" + $source_timestamp + "]`tDestination: [" + $destination_timestamp + "]")
	    }
    }

    Write-Host "======================="
}

Write-Host "Press any key to continue ..."
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")