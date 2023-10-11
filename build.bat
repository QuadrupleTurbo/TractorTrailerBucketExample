set targetfile=%~1
set folderToUse=%~2
echo %targetfile%
xcopy /Y /S %targetfile% "F:\Git-Projects\TractorTrailerBucketExample\TractorTrailerBucketExample\%folderToUse%"
xcopy /Y /S %targetfile% "F:\QDX-Test-Environment\resources\[Other Scripts]\TractorTrailerBucketExample\%folderToUse%"

:: For copying dlls to specific folders: $(SolutionDir)build.bat "$(TargetDir)$(TargetFileName)" "Client/Server"