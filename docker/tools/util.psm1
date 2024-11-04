function Get-EnvVar {
  param(
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string] 
    $Key
  )

  select-string -Path ".env" -Pattern "^$Key=(.+)$" | % { $_.Matches.Groups[1].Value }
}

function Read-UserEnvFile {
  param(
    [Parameter()]
    [string] $EnvFile = ".env.user"
  )

  if (Test-Path $EnvFile) {
      Write-Host "User specific .env file found. Starting Docker with custom user settings." -ForegroundColor Green
      Write-Host "Variable overrides:-" -ForegroundColor Yellow
      
      Get-Content $EnvFile | Where-Object { $_ -notmatch '^#.*' -and $_ -notmatch '^\s*$' } | ForEach-Object {
          $var, $val = $_.trim().Split('=')
          Write-Host "  $var=$val" -ForegroundColor Yellow
          Set-Item -Path "env:$($var)" -Value $val
      }
  }
}

function Test-Folder {
  param(
    [string] $path,
    [string] $folderName
  )

  $combinedPath = Join-Path $path $folderName
  if (-not (Test-Path $combinedPath)) {
    Write-Host "$($combinedPath) ... " -NoNewline
    Write-Host "Not found" -NoNewline -ForegroundColor Red
    New-Item -Path $combinedPath -ItemType Directory | Out-Null
    New-Item -Name ".gitkeep" -Path $combinedPath | Out-Null
    Write-Host " ... " -NoNewline
    Write-Host "Created" -ForegroundColor Green
  } 
}

function Test-Folders {
  
  $dataPath = Get-EnvVar -Key LOCAL_DATA_PATH
  $deployPath = Get-EnvVar -Key LOCAL_DEPLOY_PATH
  $logPath = Get-EnvVar -Key LOCAL_LOG_PATH

  Test-Folder -path $dataPath -folderName "deploy"
  Test-Folder -path $dataPath -folderName "logs"
  Test-Folder -path $dataPath -folderName "mssql"
  Test-Folder -path $dataPath -folderName "solr"

  Test-Folder -path $deployPath -folderName "cm"
  Test-Folder -path $deployPath -folderName "id"

  Test-Folder -path $logPath -folderName "cd"
  Test-Folder -path $logPath -folderName "cm"
  Test-Folder -path $logPath -folderName "head"
  Test-Folder -path $logPath -folderName "id"
}

Export-ModuleMember -Function *
