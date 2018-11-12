echo off

cd /d %~dp0


start "Request 02" cmd /T:8F /k lbmreq.exe -v -v bxu

start "Request 03" cmd /T:8F /k lbmreq.exe -v -v bxu