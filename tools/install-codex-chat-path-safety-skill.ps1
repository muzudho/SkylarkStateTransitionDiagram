param(
    [switch]$Force
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$sourceDir = Join-Path $repoRoot "YukaiLarkStateTransitionDiagram\\Docs\\運用\\skills\\codex-chat-path-safety"

if (-not (Test-Path -LiteralPath $sourceDir)) {
    throw "Skill source directory not found: $sourceDir"
}

if ($env:CODEX_HOME) {
    $skillsRoot = Join-Path $env:CODEX_HOME "skills"
}
else {
    $skillsRoot = Join-Path $HOME ".codex\\skills"
}

$destinationDir = Join-Path $skillsRoot "codex-chat-path-safety"
$metadataPath = Join-Path $destinationDir ".codex-skill-install.json"
$sourceRepo = (Resolve-Path -LiteralPath $repoRoot).Path
$backupDir = "$destinationDir.backup-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

New-Item -ItemType Directory -Path $skillsRoot -Force | Out-Null

if (Test-Path -LiteralPath $destinationDir) {
    $canOverwrite = $false

    if (Test-Path -LiteralPath $metadataPath) {
        $existingMetadata = Get-Content -LiteralPath $metadataPath -Raw | ConvertFrom-Json
        if ($existingMetadata.repository -eq $sourceRepo -and $existingMetadata.skill -eq "codex-chat-path-safety") {
            $canOverwrite = $true
        }
    }

    if (-not $canOverwrite) {
        if (-not $Force) {
            throw "Destination already exists and was not installed from this repository: $destinationDir"
        }

        Move-Item -LiteralPath $destinationDir -Destination $backupDir
    }
    else {
        Remove-Item -LiteralPath $destinationDir -Recurse -Force
    }
}

Copy-Item -LiteralPath $sourceDir -Destination $destinationDir -Recurse

$metadata = [ordered]@{
    repository = $sourceRepo
    skill = "codex-chat-path-safety"
    source = $sourceDir
    installed_at = (Get-Date).ToString("o")
}

$metadata | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $destinationDir ".codex-skill-install.json") -Encoding utf8

Write-Host "Installed codex-chat-path-safety to $destinationDir"
if (Test-Path -LiteralPath $backupDir) {
    Write-Host "Backed up previous skill to $backupDir"
}
