src\.nuget\NuGet.exe install src\.nuget\packages.config -OutputDirectory thirdparty

$env:PATH += ";.\thirdparty\NAnt.Portable.0.92\;.\thirdparty\NUnit.Runners.2.6.1\tools"

nant.exe Init.VSDebug