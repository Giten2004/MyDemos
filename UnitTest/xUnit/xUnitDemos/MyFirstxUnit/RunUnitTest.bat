echo off

cd /d %~dp0

start "ExecuteUnitTest" cmd /T:8E /k ..\..\..\packages\xunit.runner.console.2.1.0\tools\xunit.console.exe ..\..\..\MyFirstxUnit\bin\Debug\MyFirstxUnit.dll