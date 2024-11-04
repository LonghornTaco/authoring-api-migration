param(
    [switch] $RecreateCerts = $false,
    [switch] $ExcludeItemSync = $false,
    [switch] $IncludeIndexRebuild = $false,
    [switch] $ExcludeItemWatcher = $false
)
clear-host
Import-Module -Name (Join-Path $PSScriptRoot ".\docker\tools\logo")
Import-Module -Name (Join-Path $PSScriptRoot ".\docker\tools\util")
Show-Start

#----------------------------------------------------------
## load variables
#----------------------------------------------------------

$cmUrl = Get-EnvVar -Key CM_HOST
$cdUrl = Get-EnvVar -Key CD_HOST
$idUrl = Get-EnvVar -Key ID_HOST
$renderingUrl = Get-EnvVar -Key RENDERING_HOST
$webhookUrl = Get-EnvVar -Key WEBHOOK_HOST

$cliSecret = Get-EnvVar -Key CLI_IDSECRET

$sanList = @($cmUrl, $cdUrl, $idUrl, $renderingUrl, $webhookUrl)

#----------------------------------------------------------
## check data folders
#----------------------------------------------------------

Write-host "Checking that data folders exist..."
Test-Folders

#----------------------------------------------------------
## check license is present
#----------------------------------------------------------

$licensePath = Get-EnvVar -Key LICENSE_PATH
if (-not (Test-Path (Join-Path $licensePath "license.xml"))) {
    Write-Host "License file not present in folder." -ForegroundColor Red
    Break
}

#----------------------------------------------------------
## check traefik ssl certs present
#----------------------------------------------------------

if ($RecreateCerts -eq $true) {
    Remove-Item -Path .\docker\traefik\certs\cert.pem
    Remove-Item -Path .\docker\traefik\certs\key.pem
}

if (-not (Test-Path .\docker\traefik\certs\cert.pem)) {
    .\docker\tools\mkcert.ps1 -sanList $sanList
}

#----------------------------------------------------------
## check if user override env file exists
#----------------------------------------------------------

Read-UserEnvFile

#----------------------------------------------------------
## start docker
#----------------------------------------------------------

Write-Host "Starting Sitecore..." -ForegroundColor Green
docker-compose up -d

#----------------------------------------------------------
## Wait for Traefik to expose CM route
#----------------------------------------------------------

Write-Host "Waiting for CM to become available..." -ForegroundColor Green
$startTime = Get-Date
do {
    Start-Sleep -Milliseconds 100
    try {
        $status = Invoke-RestMethod "http://localhost:8079/api/http/routers/cm-secure@docker"
    } catch {
        if ($_.Exception.Response.StatusCode.value__ -ne "404") {
            throw
        }
    }
} while ($status.status -ne "enabled" -and $startTime.AddSeconds(15) -gt (Get-Date))
if (-not $status.status -eq "enabled") {
    $status
    Write-Error "Timeout waiting for Sitecore CM to become available via Traefik proxy. Check CM container logs."
}
Write-Host "CM is now available. Warming it up..." -ForegroundColor Green
try {
   Invoke-WebRequest -method get -uri https://$cmUrl -TimeoutSec 200
} catch {
   # do nothing on purpose
}
# if ($statusCode -eq "200") {
#    Write-host "CM is warmed up." -ForegroundColor Green
# } else {
#    Write-host "There was a problem warming up the CM" -ForegroundColor Red
# }

#----------------------------------------------------------
## Sitecore CLI
#----------------------------------------------------------

Write-Host "Restoring Sitecore CLI..." -ForegroundColor Green
    dotnet tool restore
Write-Host "Installing Sitecore CLI Plugins..."
dotnet sitecore --help | Out-Null
if ($LASTEXITCODE -ne 0) {
    Write-Error "Unexpected error installing Sitecore CLI Plugins"
}

#----------------------------------------------------------
## Log in for the CLI
#----------------------------------------------------------

Write-Host "Logging into Sitecore..." -ForegroundColor Green
dotnet sitecore login --authority https://$idUrl --cm https://$cmUrl --allow-write true --client-credentials true --client-id SitecoreCLIServer --client-secret $cliSecret
#dotnet sitecore connect --cm https://$cmUrl --allow-write true -n local -r default

if ($LASTEXITCODE -ne 0) {
    Write-Error "Unable to log into Sitecore, did the Sitecore environment start correctly? See logs above."
}

if ($IncludeIndexRebuild) {
   #----------------------------------------------------------
   ## Populate Solr managed schemas to avoid errors during item deploy
   #----------------------------------------------------------

   Write-Host "Populating Solr managed schema..." -ForegroundColor Green
   dotnet sitecore index schema-populate
   if ($LASTEXITCODE -ne 0) {
      Write-Error "Populating Solr managed schema failed, see errors above."
   }

   #----------------------------------------------------------
   ## Rebuild indexes
   #----------------------------------------------------------

   Write-Host "Rebuilding indexes ..." -ForegroundColor Green
   dotnet sitecore index rebuild
} else {
   Write-Host "Skipping Index Rebuild" -ForegroundColor Yellow
}

if ($ExcludeItemSync) {
   Write-Host "Skipping Item Sync..." -ForegroundColor Yellow
} else {
   #----------------------------------------------------------
   ## Syncing items
   #----------------------------------------------------------

   Write-Host "Syncing items" -ForegroundColor Green
   dotnet sitecore ser push
}

#----------------------------------------------------------
## Opening site
#----------------------------------------------------------

Write-Host "Opening site..." -ForegroundColor Green
Start-Process https://$cmUrl/sitecore/


#----------------------------------------------------------
## Item Wathcer
#----------------------------------------------------------
if (-not $ExcludeItemWatcher) {
   dotnet sitecore ser watch -s
}
