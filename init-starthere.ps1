clear-host
Import-Module -Name (Join-Path $PSScriptRoot ".\docker\tools\logo")
Import-Module -Name (Join-Path $PSScriptRoot ".\docker\tools\util")
Show-Start

Write-host "Checking that data folders exist..."
Test-Folders

Write-host "Starting init containers..."
docker-compose -f .\docker-compose.override-init.yml up -d