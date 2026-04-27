$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$dotnetCliHome = $repoRoot
$assetFiles = @(
    "src\DeviceManagerBE\obj\project.assets.json",
    "src\DeviceManagerBE.Application\obj\project.assets.json",
    "src\DeviceManagerBE.Contracts\obj\project.assets.json",
    "src\DeviceManagerBE.Infrastructure\obj\project.assets.json",
    "src\DeviceManagerFE\obj\project.assets.json",
    "src\DeviceManagerFE.Application\obj\project.assets.json",
    "src\DeviceManagerFE.Contracts\obj\project.assets.json",
    "src\DeviceManagerFE.Infrastructure\obj\project.assets.json"
)
$buildProjects = @(
    "src\DeviceManagerBE.Domain\DeviceManagerBE.Domain.csproj",
    "src\DeviceManagerBE.Contracts\DeviceManagerBE.Contracts.csproj",
    "src\DeviceManagerBE.Application\DeviceManagerBE.Application.csproj",
    "src\DeviceManagerBE.Infrastructure\DeviceManagerBE.Infrastructure.csproj",
    "src\DeviceManagerBE\DeviceManagerBE.csproj",
    "src\DeviceManagerFE.Domain\DeviceManagerFE.Domain.csproj",
    "src\DeviceManagerFE.Contracts\DeviceManagerFE.Contracts.csproj",
    "src\DeviceManagerFE.Application\DeviceManagerFE.Application.csproj",
    "src\DeviceManagerFE.Infrastructure\DeviceManagerFE.Infrastructure.csproj",
    "src\DeviceManagerFE\DeviceManagerFE.csproj"
)

$env:DOTNET_CLI_HOME = $dotnetCliHome
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"
$env:DOTNET_NOLOGO = "1"

function Stop-WorkspaceDotnetProcess {
    param(
        [Parameter(Mandatory = $true)]
        [string]$ProcessName
    )

    $candidates = Get-Process -Name $ProcessName -ErrorAction SilentlyContinue
    foreach ($process in $candidates) {
        $processPath = $null
        try {
            $processPath = $process.Path
        }
        catch {
            continue
        }

        if ([string]::IsNullOrWhiteSpace($processPath)) {
            continue
        }

        if ($processPath.StartsWith($repoRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
            Write-Host "Stopping $ProcessName ($($process.Id)) from $processPath"
            Stop-Process -Id $process.Id -Force
        }
    }
}

Stop-WorkspaceDotnetProcess -ProcessName "DeviceManagerFE"
Stop-WorkspaceDotnetProcess -ProcessName "DeviceManagerBE"

Push-Location $repoRoot
try {
    dotnet --version
    dotnet build-server shutdown | Out-Host

    $needsRestore = @(
        $assetFiles |
        ForEach-Object { Join-Path $repoRoot $_ } |
        Where-Object { -not (Test-Path $_) } |
        Select-Object -First 1
    )

    if ($needsRestore) {
        throw "Missing NuGet assets. Run 'dotnet restore' manually first, then retry debug."
    }

    foreach ($project in $buildProjects) {
        Write-Host "Building $project"
        dotnet build $project --no-restore -v minimal -m:1 /p:BuildInParallel=false /p:UseSharedCompilation=false
    }
}
finally {
    Pop-Location
}
