echo off

cd /d %~dp0

rem different receive speed, one slow receive may block all ohter receivers.....

start "MinRcv" cmd /T:8E /k MinRcv.exe 1
start "MinRcv" cmd /T:8E /k MinRcv.exe -1
start "MinRcv" cmd /T:8E /k MinRcv.exe 0