REM register for com so we can test the register/unregister functions while debugging
cd /d %~dp0

%SystemRoot%\Microsoft.NET\Framework\v2.0.50727\regasm.exe /codebase BHOButton.dll

pause