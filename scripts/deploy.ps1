$ErrorActionPreference = 'Stop'

function CheckLastExitCode() {
  if ($? -eq $false) {
    throw 'build failed'
  }
}

if (Test-Path('deploy')) {
  remove-item 'deploy' -Recurse -Verbose
}

New-Item .\deploy\net-4.0\snarfz.core\merged -ItemType directory -Verbose
New-Item .\deploy\net-4.0\snarfz.core\raw -ItemType directory -Verbose

Copy-Item .\debug\net-4.0\snarfz.core\*.* .\deploy\net-4.0\snarfz.core\raw -Verbose

.\thirdparty\packages\common\ilmerge\ilmerge.exe /t:library /out:.\deploy\net-4.0\snarfz.core\merged\Snarfz.Core.dll .\deploy\net-4.0\snarfz.core\raw\Snarfz.Core.dll .\deploy\net-4.0\snarfz.core\raw\SupaCharge.Core.dll | Write-Host
CheckLastExitCode