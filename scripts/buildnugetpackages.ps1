$ErrorActionPreference = 'Stop'

function CheckLastExitCode() {
  if ($? -eq $false) {
    throw 'build failed'
  }
}

if (Test-Path('nugetworking')) {
  remove-item 'nugetworking' -Recurse -Verbose
}

#New-Item .\nugetworking\core\lib\net35 -ItemType directory -Verbose
New-Item .\nugetworking\snarfz.core\lib\net40 -ItemType directory -Verbose
#New-Item .\nugetworking\core\lib\net45 -ItemType directory -Verbose

#Copy-Item .\debug\net-3.5\snarfz.core\snarfz.core.dll .\nugetworking\core\lib\net35 -Verbose
Copy-Item .\deploy\net-4.0\snarfz.core\merged\snarfz.core.dll .\nugetworking\snarfz.core\lib\net40 -Verbose
#Copy-Item .\debug\net-4.5\snarfz.core\snarfz.core.dll .\nugetworking\core\lib\net45 -Verbose
Copy-Item .\src\snarfz.nuget.specs\snarfz.core.dll.nuspec .\nugetworking\snarfz.core -Verbose

thirdparty\nuget\nuget.exe pack .\nugetworking\snarfz.core\snarfz.core.dll.nuspec -OutputDirectory .\nugetworking\snarfz.core | Write-Host
CheckLastExitCode