echo off

cd /d %~dp0
start "Server (Rep)" cmd /T:8E /k lpserver.exe 

start "Client (Req-1)" cmd /T:8F /k lpclient.exe MIKE
start "Client (Req-2)" cmd /T:8F /k lpclient.exe Lucy