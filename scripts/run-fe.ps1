$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$projectPath = Join-Path $repoRoot "src\DeviceManagerFE\DeviceManagerFE.csproj"
$env:DOTNET_CLI_HOME = $repoRoot
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"
$env:DOTNET_NOLOGO = "1"

Push-Location $repoRoot
try {
    dotnet run --project $projectPath --launch-profile http
}
finally {
    Pop-Location
}
