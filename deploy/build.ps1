Set-ExecutionPolicy Unrestricted

$SolutionDir = Resolve-Path "$PSScriptRoot\.."
$SuccessColor = "Green"
$FailureColor = "Red"

Import-Module -Name "$SolutionDir/tools/Invoke-MsBuild.psm1"
$LogFilePath = Invoke-MsBuild -Path "$SolutionDir/deploy/VaBank.proj"-GetLogPath
$BuildSuccessful = Invoke-MsBuild -Path "$SolutionDir/deploy/VaBank.proj" -P "/t:Migrate /v:normal"  -KeepBuildLogOnSuccessfulBuilds -AutoLaunchBuildLogOnFailure -ShowBuildWindowAndPromptForInputBeforeClosing
if ($BuildSuccessful) {
	Write-Host -ForegroundColor $SuccessColor -Object "Build was successfull. Log file path: [$LogFilePath]"
} else {
	Write-Host -ForegroundColor $FailureColor -Object "Build was failed."
}