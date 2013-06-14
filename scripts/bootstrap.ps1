src\.nuget\NuGet.exe install src\.nuget\packages.config -OutputDirectory src\packages

$env:PATH += ";.\src\packages\NAnt.Portable.0.92\;.\src\packages\NUnit.Runners.2.6.1\tools"

nant Clean Cycle Deploy