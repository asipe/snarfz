thirdparty\nuget\NuGet.exe install src\Snarfz.Nuget.Packages\common\packages.config -OutputDirectory thirdparty\packages\common -ExcludeVersion
thirdparty\nuget\NuGet.exe install src\Snarfz.Nuget.Packages\net-4.0\packages.config -OutputDirectory thirdparty\packages\net-4.0 -ExcludeVersion

$env:PATH += ";.\thirdparty\packages\common\NAnt.Portable\;.\thirdparty\packages\common\NUnit.Runners\tools"

nant Clean Cycle Deploy