echo off

cd /d %~dp0
start "Server (Rep)" cmd /T:8E /k lpserver.exe 

start "Client (Req)" cmd /T:8F /k lpclient.exe MIKE