$ErrorActionPreference = 'Stop'

function CheckLastExitCode() {
  if ($? -eq $false) {
    throw 'build failed'
  }
}

$times = Measure-Command {
  C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe .\src\Snarfz.Build\Snarfz.proj /ds /maxcpucount:4 | Write-Host
  CheckLastExitCode
}

Write-Host "Build Completed in $($times.TotalSeconds) seconds ($($times.TotalMilliseconds) millis)"