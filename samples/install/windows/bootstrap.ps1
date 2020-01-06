# Setup common vars and functions.
function Give-Full-Rights($path, $user) {
    Write-Host "Giving full rights to user $user on path $path"
    $acl = Get-Acl $path
    if (Test-Path -Path $path -PathType Leaf) {
        $rule = New-Object System.Security.AccessControl.FileSystemAccessRule($user, "FullControl", "None", "None", "Allow")
    }
    else {
        $rule = New-Object System.Security.AccessControl.FileSystemAccessRule($user, "FullControl", "ContainerInherit,ObjectInherit", "None", "Allow")
    }

    $acl.SetAccessRule($rule)
    Set-Acl $path $acl
}

function Add-Env-Variable($envVarsElement, $name, $value) {
    $element = $envVarsElement.OwnerDocument.CreateElement("environmentVariable")

    $nameAttr = $envVarsElement.OwnerDocument.CreateAttribute("name")
    $nameAttr.Value = $name

    $valueAttr = $envVarsElement.OwnerDocument.CreateAttribute("value")
    $valueAttr.Value = $value

    $element.Attributes.Append($nameAttr);
    $element.Attributes.Append($valueAttr);

    $envVarsElement.AppendChild($element)
}

$samplesRootPath = "C:\wolk\samples"
$wolkEnvContent = Get-Content (Join-Path -Path $samplesRootPath "install\env.json") -Raw
$wolkEnv = ConvertFrom-Json $wolkEnvContent
$iisUser = "IIS_IUSRS"
$tmpFolder = $env:TEMP


# First, install IIS (help from https://weblog.west-wind.com/posts/2017/may/25/automating-iis-feature-installation-with-powershell).
Write-Host "Installing Windows Features for IIS"
Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole
Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServer
Enable-WindowsOptionalFeature -Online -FeatureName IIS-CommonHttpFeatures
Enable-WindowsOptionalFeature -Online -FeatureName IIS-HttpErrors
Enable-WindowsOptionalFeature -Online -FeatureName IIS-HttpRedirect
Enable-WindowsOptionalFeature -Online -FeatureName IIS-ApplicationDevelopment

# Next, download the .NET Core hosting bundle.
$hostingBundleUrl = "https://download.visualstudio.microsoft.com/download/pr/fa3f472e-f47f-4ef5-8242-d3438dd59b42/9b2d9d4eecb33fe98060fd2a2cb01dcd/dotnet-hosting-3.1.0-win.exe"
$hostingBundlePath = Join-Path -Path $tmpFolder "hostingbundle-3.1.0.exe"
if (Test-Path $hostingBundlePath) {
    Write-Host "Path $hostingBundlePath already exists, not downloading again."
}
else {
    Write-Host "Downloading $hostingBundleUrl to $hostingBundlePath"
    Invoke-WebRequest -Uri $hostingBundleUrl -OutFile $hostingBundlePath -UseBasicParsing
}

# Install the .NET Core hosting bundle
Write-Host "Installing $hostingBundlePath"
$args = New-Object -TypeName System.Collections.Generic.List[System.String]

$args.Add("/quiet")
$args.Add("/norestart")

Start-Process -FilePath $hostingBundlePath -ArgumentList $args -NoNewWindow -Wait -PassThru

# Find and downloading the latest version of Wolk through GitHub API.
$apiUrl = "https://api.github.com/repos/dukeofharen/wolk/releases/latest"
$apiResponse = Invoke-WebRequest $apiUrl -UseBasicParsing
$json = ConvertFrom-Json $apiResponse.Content
$release = $json.assets | Where-Object { $_.name -eq "wolk_windows.zip" }
$wolkDownloadUrl = $release.browser_download_url
$wolkDownloadPath = Join-Path -Path $tmpFolder "wolk-$($json.name).zip"
if (Test-path $wolkDownloadPath) {
    Write-Host "Path $wolkDownloadPath already exists, not downloading again."
}
else {
    Write-Host "Downloading $wolkDownloadUrl to $wolkDownloadPath"
    Invoke-WebRequest -Uri $wolkDownloadUrl -OutFile $wolkDownloadPath -UseBasicParsing
}

# Stop Wolk IIS site if it exists.
Stop-Website -Name Wolk -ErrorAction SilentlyContinue

# Install Wolk.
$installPath = "C:\bin\wolk"
if (Test-Path $installPath) {
    Write-Host "Deleting path $installPath"
    Remove-Item $installPath -Recurse -Force
}

Write-Host "Creating folder $installPath"
New-Item -ItemType Directory $installPath

Write-Host "Extracting file $wolkDownloadPath to $installPath"
Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::ExtractToDirectory($wolkDownloadPath, $installPath)

# Create logs folder and set rights.
$logsPath = Join-Path -Path $installPath "logs"
New-Item -ItemType Directory $logsPath
Give-Full-Rights -path $logsPath -user $iisUser

# Create Wolk data folder and set rights.
$dataPath = "C:\wolkdata"
if (!(Test-Path $dataPath)) {
    Write-Host "Creating folder $dataPath"
    New-Item -ItemType Directory $dataPath
    Give-Full-Rights -path $dataPath -user $iisUser
}

$uploadsPath = Join-Path -Path $dataPath "uploads"
if (!(Test-Path $uploadsPath)) {
    Write-Host "Creating folder $uploadsPath"
    New-Item -ItemType Directory $uploadsPath
    Give-Full-Rights -path $uploadsPath -user $iisUser
}

$dbPath = Join-Path -Path $dataPath "wolk.db"
if (!(Test-Path $dbPath)) {
    Write-Host "Creating file $dbPath"
    New-Item -ItemType File $dbPath
    Give-Full-Rights -path $dbPath -user $iisUser
}

# Updating web.config of wolk with the correct variables.
$webConfigPath = Join-Path -Path $installPath "web.config"
Write-Host "Updating file $webConfigPath"
[xml]$webConfig = Get-Content $webConfigPath
$aspNetCore = $webConfig.configuration.location.'system.webServer'.aspNetCore
$aspNetCore.stdoutLogEnabled = "true"

$environmentVariables = $webConfig.CreateElement("environmentVariables")
Add-Env-Variable -envVarsElement $environmentVariables -name "ASPNETCORE_ENVIRONMENT" -value "Production"
Add-Env-Variable -envVarsElement $environmentVariables -name "ConnectionStrings:WolkDatabase" -value "Data Source=$dbPath"
Add-Env-Variable -envVarsElement $environmentVariables -name "IdentityConfiguration:JwtSecret" -value $wolkEnv.JwtSecret
Add-Env-Variable -envVarsElement $environmentVariables -name "IdentityConfiguration:ExpirationInSeconds" -value $wolkEnv.ExpirationInSeconds
Add-Env-Variable -envVarsElement $environmentVariables -name "WolkConfiguration:UploadsPath" -value $uploadsPath
Add-Env-Variable -envVarsElement $environmentVariables -name "WolkConfiguration:DefaultLoginEmail" -value $wolkEnv.DefaultLoginEmail
Add-Env-Variable -envVarsElement $environmentVariables -name "WolkConfiguration:DefaultPassword" -value $wolkEnv.DefaultPassword

$aspNetCore.AppendChild($environmentVariables)

$webConfig.Save($webConfigPath)

# Remove default site
Remove-Website -Name "Default Web Site" -ErrorAction SilentlyContinue

# Add site to IIS.
Write-Host "Adding site to IIS."
New-WebSite -Name Wolk -Port 80 -PhysicalPath "$env:systemdrive\bin\wolk" -Force
Start-Website -Name Wolk -ErrorAction SilentlyContinue

# First, retrieve a JWT from Wolk
Write-Host "Requesting Wolk JSON web token"
$loginBody = ConvertTo-Json @{email = $wolkEnv.DefaultLoginEmail; password = $wolkEnv.DefaultPassword}
$response = Invoke-WebRequest -Method Post -Uri "http://localhost/api/user/authenticate" -Body $loginBody -ContentType "application/json" -UseBasicParsing
$authenticationJson = ConvertFrom-Json $response.Content
$jwt = $authenticationJson.token

# Upload the backup to Wolk
Write-Host "Restoring backup for testing purposes"
$wolkBackupPath = Join-Path -Path $samplesRootPath "install\wolk-backup-test.zip"
$base64Backup = [Convert]::ToBase64String([IO.File]::ReadAllBytes($wolkBackupPath))
$backupBody = ConvertTo-Json @{zipBytes = $base64Backup}
Invoke-WebRequest -Method Post -Uri "http://localhost/api/backup" -Body $backupBody -ContentType "application/json" -Headers @{"Authorization" = "Bearer $jwt"} -UseBasicParsing