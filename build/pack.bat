SET PREV_DIR=%CD%
CD %~dp0\..

IF "%1" == "" (
    ECHO "Usage pack.bat <Version>"
    ECHO "Example: pack.bat 1.0.0.0"
    EXIT 1
)
SET VERSION=%1

nuget.exe pack NuGetConfections\NuGetConfections.csproj -Properties Configuration=Release -Properties Platform=x86 -Version %VERSION%

SET RESULT=%ERRORLEVEL%
CD %PREV_DIR%
EXIT /B %RESULT%