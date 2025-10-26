param(
  [string]$Input = "postman/Local.postman_environment.json",
  [string]$Output = "postman/Local.tmp.postman_environment.json",
  [string]$Port = "5080"
)

try {
  $json = Get-Content $Input -Raw | ConvertFrom-Json
  foreach ($v in $json.values) {
    if ($v.key -eq 'baseUrl') {
      $v.value = "http://localhost:$Port"
    }
  }
  $json | ConvertTo-Json -Depth 10 | Set-Content -NoNewline $Output
  Write-Host "Wrote environment to $Output with baseUrl=http://localhost:$Port"
} catch {
  Write-Host $_.Exception.Message
  exit 1
}

