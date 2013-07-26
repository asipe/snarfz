$ErrorActionPreference = 'Stop'

if (Test-Path('debug')) {
  remove-item 'debug' -Recurse -Verbose
}

if (Test-Path('deploy')) {
  remove-item 'deploy' -Recurse -Verbose
}

if (Test-Path('thirdparty\packages')) {
  remove-item 'thirdparty\packages' -Recurse -Verbose
}

if (Test-Path('nugetworking')) {
  remove-item 'nugetworking' -Recurse -Verbose
}