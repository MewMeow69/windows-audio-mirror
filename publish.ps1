param(
    [string]$Version = "1.0.0",
    [string]$OutDir = ".\release"
)

$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"

Write-Host "=== Windows Audio Mirror — Release Builder ===" -ForegroundColor Cyan
Write-Host ""

# Clean
Write-Host "[1/4] Cleaning..." -ForegroundColor Gray
Remove-Item -Recurse -Force bin, obj, $OutDir -ErrorAction SilentlyContinue

# Build
Write-Host "[2/4] Building Release..." -ForegroundColor Gray
dotnet publish -c Release -r win-x64 --self-contained false -o "$OutDir\app"
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

# Copy audio_duplicator.exe if present
$dupe = ".\audio_duplicator.exe"
if (Test-Path $dupe) {
    Copy-Item $dupe "$OutDir\app\" -Force
    Write-Host "       audio_duplicator.exe included" -ForegroundColor Green
} else {
    Write-Host "       audio_duplicator.exe NOT found — users must download it separately" -ForegroundColor Yellow
    Write-Host "       See: https://github.com/Kl1movM/audio-duplicator" -ForegroundColor Yellow
}

# Zip
Write-Host "[3/4] Packaging..." -ForegroundColor Gray
$zip = "$OutDir\WindowsAudioMirror-v$Version.zip"
Compress-Archive -Path "$OutDir\app\*" -DestinationPath $zip -Force

# Clean temp app folder
Remove-Item -Recurse -Force "$OutDir\app"

# Done
Write-Host "[4/4] Done!" -ForegroundColor Green
$size = [math]::Round((Get-Item $zip).Length / 1KB, 1)
Write-Host ""
Write-Host "Release zip: $zip ($size KB)" -ForegroundColor White
Write-Host "Upload this to GitHub Releases as v$Version" -ForegroundColor Gray
