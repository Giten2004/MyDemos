REM unregister HelloBHOWorld.dll for COM
cd /d %~dp0

%SystemRoot%\Microsoft.NET\Framework\v2.0.50727\regasm.exe /unregister BHOButton.dll

pause