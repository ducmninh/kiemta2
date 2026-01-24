# Script to kill old process and restart the application
Write-Host "Stopping any running PCM_396 processes..." -ForegroundColor Yellow

# Kill all PCM_396 processes
Get-Process -Name "PCM_396" -ErrorAction SilentlyContinue | Stop-Process -Force
Start-Sleep -Seconds 1

Write-Host "Building project..." -ForegroundColor Cyan
dotnet build PCM_396.csproj

if ($LASTEXITCODE -eq 0) {
    Write-Host "`nBuild successful! Starting application..." -ForegroundColor Green
    Write-Host "Press Ctrl+C to stop the application`n" -ForegroundColor Yellow
    dotnet run --project PCM_396.csproj
} else {
    Write-Host "`nBuild failed! Check errors above." -ForegroundColor Red
}
