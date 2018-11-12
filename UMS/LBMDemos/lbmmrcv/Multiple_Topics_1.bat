echo off

cd /d %~dp0


start "lbmmrcv 1" cmd /T:8E /k lbmmrcv.exe -i 2
start "lbmmrcv 2" cmd /T:8E /k lbmmrcv.exe -i 2

start "lbmmsrc" cmd /T:8F /k lbmmsrc.exe -i 2