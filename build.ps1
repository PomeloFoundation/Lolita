$root = Get-Location
$src = Join-Path $root 'src'
Set-Location $src
$directories = dir

For ($i = 0; $i -lt $directories.Count; ++$i) {
    $project = Join-Path $src $directories[$i].Name
    Set-Location $project
    dotnet pack -c Release
}

Set-Location $root
$files = Get-ChildItem -Path $src -Include '*.nupkg' -Recurse

$nupkg = Join-Path $root 'nupkg'
If (Test-Path $nupkg) {
    Remove-Item -Path $nupkg -Recurse -Force
}
New-Item -Path $nupkg -ItemType Directory

For ($i = 0; $i -lt $files.Count; ++$i) {
    $nupkgName = [System.IO.Path]::GetFileName($files[$i])
    $nupkgFullPath = Join-Path $nupkg $nupkgName
    Copy-Item -Path $files[$i] -Destination $nupkgFullPath
}