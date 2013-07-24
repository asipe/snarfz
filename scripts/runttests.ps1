$ErrorActionPreference = 'Stop'

function CheckLastExitCode() {
  if ($? -eq $false) {
    throw 'build failed'
  }
}


#Write-Host ''
#Write-Host '------------net-3.5------------' -ForegroundColor DarkYellow
#.\packages\common\NUnit.Runners\tools\nunit-console.exe debug\net-3.5\snarfz.unittests\snarfz.unittests.dll /nologo | Write-Host
#CheckLastExitCode
#Write-Host '-------------------------------' -ForegroundColor DarkYellow
#Write-Host ''

Write-Host ''
Write-Host '------------net-4.0------------' -ForegroundColor DarkYellow
.\thirdparty\packages\common\NUnit.Runners\tools\nunit-console.exe debug\net-4.0\snarfz.unittests\snarfz.unittests.dll /nologo | Write-Host
CheckLastExitCode
Write-Host '-------------------------------' -ForegroundColor DarkYellow
Write-Host ''

#Write-Host ''
#Write-Host '------------net-4.5------------' -ForegroundColor DarkYellow
#.\packages\common\NUnit.Runners\tools\nunit-console.exe debug\net-4.5\snarfz.unittests\snarfz.unittests.dll /nologo | Write-Host
#CheckLastExitCode
#Write-Host '-------------------------------' -ForegroundColor DarkYellow
#Write-Host ''
