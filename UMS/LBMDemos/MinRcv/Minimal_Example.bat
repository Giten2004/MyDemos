echo off

cd /d %~dp0


start "MinRcv" cmd /T:8E /k MinRcv.exe

start "MinSrc" cmd /T:8F /k MinSrc.exe 