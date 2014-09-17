Param(
	[Parameter(Position = 1)]
	[String] $Targets = "BuildAll;Migrate",
	[Parameter(Position = 2)]
	[String] $Environment = "Development",
	[String] $Configuration = "Release",
	[String] $Verbosity = "normal"
)
$SolutionDir = Resolve-Path "$PSScriptRoot\.."
$ToolsDir = "$SolutionDir\tools"
$BuildDir = "$SolutionDir\build"
Import-Module -Name "$SolutionDir/tools/Invoke-MsBuild.psm1"

if (!(Test-Path $BuildDir)) {
	New-Item -ItemType "Directory" -Path $BuildDir
}
Write-Host -BackgroundColor "Blue" "Invoking MSBuild..."
$LogFilePath = Invoke-MsBuild -Path "$SolutionDir/deploy/VaBank.proj" -BuildLogDirectoryPath $SolutionDir -GetLogPath
$BuildSuccessful = Invoke-MsBuild -Path "$SolutionDir/deploy/VaBank.proj" -P "/t:$Targets /v:$Verbosity /p:Configuration=$Configuration /p:Environment=$Environment /p:OutDir=$BuildDir" -KeepBuildLogOnSuccessfulBuilds -BuildLogDirectoryPath $SolutionDir -ShowBuildWindowAndPromptForInputBeforeClosing 
if ($BuildSuccessful) {
	Write-Host -ForegroundColor "Green" -Object "Build was successfull. Log file path: [$LogFilePath]"
} else {
	Write-Host -ForegroundColor "Red" -Object "Build was failed. Log file path: [$LogFilePath]"
}
Write-Host "`r`nPress any key to continue..." -BackgroundColor Blue
try 
{
    $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown") | Out-Null
}
catch [Exception]
{
}