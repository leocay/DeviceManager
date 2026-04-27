$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$runBeScript = Join-Path $PSScriptRoot "run-be.ps1"
$runFeScript = Join-Path $PSScriptRoot "run-fe.ps1"

$env:DOTNET_CLI_HOME = $repoRoot
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"
$env:DOTNET_NOLOGO = "1"

function Stop-WorkspaceProcess {
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

Stop-WorkspaceProcess -ProcessName "DeviceManagerFE"
Stop-WorkspaceProcess -ProcessName "DeviceManagerBE"

$beProcess = Start-Process powershell `
    -ArgumentList "-ExecutionPolicy", "Bypass", "-File", $runBeScript `
    -WorkingDirectory $repoRoot `
    -PassThru

Write-Host "Started DeviceManagerBE in background. PID: $($beProcess.Id)"
Start-Sleep -Seconds 3

& powershell -ExecutionPolicy Bypass -File $runFeScript
