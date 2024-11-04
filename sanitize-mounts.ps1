param (
    [switch] $Boom,
    [switch] $IncludeDeployFolder
)
function Clean-Folder {
    param(
        [string] $Folder
    )

    $combinedPath = Join-Path -Path $Folder -ChildPath "/*"

    if (Test-Path $Folder) {
        Write-Host "Found $Folder - cleaning..."
        Remove-Item -Path $combinedPath -Exclude ".gitkeep" -Recurse -Force
    } else {
        Write-Host "Could not find $Folder - creating..."
        New-Item -Path $Folder -ItemType Directory | Out-Null
        New-Item -Name ".gitkeep" -Path $Folder | Out-Null
    }
}

write-host "Cleaning up the logs..."
Clean-Folder -Folder "./docker/data/logs/cd"
Clean-Folder -Folder "./docker/data/logs/cm"
Clean-Folder -Folder "./docker/data/logs/id"

if ($IncludeDeployFolder -or $Boom) {
    write-host " "
    write-host " -IncludeDeployFolder was specified...  Cleaning the deployment folder..."

    Clean-Folder -Folder "./docker/data/deploy/cm"
}

if ($Boom) {

    $confirm = Read-Host "You said -Boom...  Are you sure?? [yN]"

    if ($confirm -eq 'y') {
        write-host "Removing solr indexes..."
        Remove-Item -Path "./docker/data/solr/*" -Exclude ".gitkeep" -Recurse -Force

        write-host "Removing sql databases..."
        Remove-Item -Path "./docker/data/mssql/*" -Exclude ".gitkeep" -Recurse -Force
    }
}
