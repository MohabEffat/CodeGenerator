param(
  [string]$Collection = "postman/CodeGenerator.postman_collection.json",
  [string]$Environment = "postman/Local.postman_environment.json",
  [string]$Report = "postman/report.html"
)

Write-Host "Running Postman collection with Newman..." -ForegroundColor Cyan

# Ensure newman is available
$newman = Get-Command newman -ErrorAction SilentlyContinue
if (-not $newman) {
  Write-Host "Newman CLI not found." -ForegroundColor Yellow
  Write-Host "Install it with: npm install -g newman newman-reporter-htmlextra" -ForegroundColor Yellow
  exit 1
}

# Prefer htmlextra reporter if installed
$htmlextra = (newman -h | Select-String -SimpleMatch "htmlextra")
if ($htmlextra) {
  newman run $Collection -e $Environment --reporters cli,htmlextra --reporter-htmlextra-export $Report
} else {
  newman run $Collection -e $Environment --reporters cli
}

if ($LASTEXITCODE -eq 0) {
  Write-Host "All tests passed." -ForegroundColor Green
  if (Test-Path $Report) {
    Write-Host "HTML report: $Report" -ForegroundColor Green
  }
} else {
  Write-Host "Some tests failed. See output above." -ForegroundColor Red
  exit $LASTEXITCODE
}

