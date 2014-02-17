$ErrorActionPreference = 'Stop'

function CheckLastExitCode() {
  if ($? -eq $false) {
    throw 'build failed'
  }
}

if (Test-Path('deploy')) {
  remove-item 'deploy' -Recurse -Verbose
}

New-Item .\deploy\net-3.5\snarfz.core\merged -ItemType directory -Verbose
New-Item .\deploy\net-3.5\snarfz.core\raw -ItemType directory -Verbose
Copy-Item .\debug\net-3.5\snarfz.core\*.* .\deploy\net-3.5\snarfz.core\raw -Verbose
.\thirdparty\packages\common\ilmerge\ilmerge.exe /internalize /t:library /out:.\deploy\net-3.5\snarfz.core\merged\snarfz.core.dll /targetplatform:v2 .\deploy\net-3.5\snarfz.core\raw\snarfz.core.dll .\deploy\net-3.5\snarfz.core\raw\supacharge.core.dll | Write-Host
CheckLastExitCode

New-Item .\deploy\net-4.0\snarfz.core\merged -ItemType directory -Verbose
New-Item .\deploy\net-4.0\snarfz.core\raw -ItemType directory -Verbose
Copy-Item .\debug\net-4.0\snarfz.core\*.* .\deploy\net-4.0\snarfz.core\raw -Verbose
.\thirdparty\packages\common\ilmerge\ilmerge.exe /internalize /t:library /out:.\deploy\net-4.0\snarfz.core\merged\snarfz.core.dll /targetplatform:v4 .\deploy\net-4.0\snarfz.core\raw\snarfz.core.dll .\deploy\net-4.0\snarfz.core\raw\supacharge.core.dll | Write-Host
CheckLastExitCode

New-Item .\deploy\net-4.5\snarfz.core\merged -ItemType directory -Verbose
New-Item .\deploy\net-4.5\snarfz.core\raw -ItemType directory -Verbose
Copy-Item .\debug\net-4.5\snarfz.core\*.* .\deploy\net-4.5\snarfz.core\raw -Verbose
.\thirdparty\packages\common\ilmerge\ilmerge.exe /internalize /t:library /out:.\deploy\net-4.5\snarfz.core\merged\snarfz.core.dll /targetplatform:v4 .\deploy\net-4.5\snarfz.core\raw\snarfz.core.dll .\deploy\net-4.5\snarfz.core\raw\supacharge.core.dll | Write-Host
CheckLastExitCode