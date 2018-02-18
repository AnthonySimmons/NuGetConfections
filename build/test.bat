SET PREV_DIR=%CD%
CD %~dp0\..

packages\NUnit.ConsoleRunner.3.8.0\tools\nunit3-console.exe ^
--x86 ^
--labels=All ^
Test\NuGetConfections.FunctionalTests\bin\x86\Release\NuGetConfections.FunctionalTests.dll

SET RESULT=%ERRORLEVEL%
CD %PREV_DIR%
EXIT /B %RESULT%