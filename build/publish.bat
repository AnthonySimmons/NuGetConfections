SET PREV_DIR=%CD%
CD %~dp0\..

nuget push NuGetConfections.*.nupkg -NonInteractive -Source https://api.nuget.org/v3/index.json

SET RESULT=%ERRORLEVEL%
CD %PREV_DIR%
EXIT /B %RESULT%