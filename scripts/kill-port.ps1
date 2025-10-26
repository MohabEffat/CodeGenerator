param([Parameter(Mandatory=$true)][int]$Port)

try {
  if (Get-Command Get-NetTCPConnection -ErrorAction SilentlyContinue) {
    $conn = Get-NetTCPConnection -LocalPort $Port -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($conn) {
      $pid = $conn.OwningProcess
      if ($pid) {
        Stop-Process -Id $pid -Force
        Write-Host "Killed PID $pid on port $Port"
        exit 0
      }
    }
  }

  $pid = (netstat -ano | Select-String (":" + $Port + "\s") | ForEach-Object { ($_ -split "\s+")[-1] } | Select-Object -First 1)
  if ($pid) {
    Stop-Process -Id $pid -Force
    Write-Host "Killed PID $pid on port $Port"
  } else {
    Write-Host "No process on port $Port"
  }
} catch {
  Write-Host $_.Exception.Message
}

