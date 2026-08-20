param(
    [Parameter(Mandatory = $true)]
    [ValidateSet("Android", "Windows")]
    [string]$Platform,

    [Parameter(Mandatory = $false)]
    [string]$AndroidAppPath,

    [Parameter(Mandatory = $false)]
    [string]$WindowsAppId,

    [Parameter(Mandatory = $false)]
    [string]$Filter,

    [Parameter(Mandatory = $false)]
    [string]$Configuration = "Debug",

    [Parameter(Mandatory = $false)]
    [switch]$NoBuild
)

$ErrorActionPreference = "Stop"

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$projectPath = Join-Path $repoRoot "TransactionProcessor.Mobile.UITests\TransactionProcessor.Mobile.UITests.csproj"

$env:UITEST_PLATFORM = $Platform
$env:UITEST_ANDROID_APP_PATH = ""
$env:UITEST_WINDOWS_APP_ID = ""

switch ($Platform) {
    "Android" {
        if ([string]::IsNullOrWhiteSpace($AndroidAppPath)) {
            throw "AndroidAppPath is required when Platform is Android."
        }

        $env:UITEST_ANDROID_APP_PATH = $AndroidAppPath
    }
    "Windows" {
        if ([string]::IsNullOrWhiteSpace($WindowsAppId)) {
            throw "WindowsAppId is required when Platform is Windows."
        }

        $env:UITEST_WINDOWS_APP_ID = $WindowsAppId
    }
}

$testArguments = @(
    "test",
    $projectPath,
    "--configuration",
    $Configuration
)

if ($NoBuild) {
    $testArguments += "--no-build"
}

if ([string]::IsNullOrWhiteSpace($Filter) -eq $false) {
    $testArguments += "--filter"
    $testArguments += $Filter
}

Write-Host "Running UI tests for $Platform with configuration $Configuration"
if ($Platform -eq "Android") {
    Write-Host "Using APK: $AndroidAppPath"
}
else {
    Write-Host "Using AppUserModelId: $WindowsAppId"
}

dotnet @testArguments
