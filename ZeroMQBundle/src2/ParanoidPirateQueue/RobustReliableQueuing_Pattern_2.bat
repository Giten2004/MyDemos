echo off

cd /d %~dp0
start "Server (Broker)" cmd /T:3E /k ParanoidPirateQueue.exe 

start "Worker (DEALER)" cmd /T:6F /k ParanoidPirateWorker.exe Worker-1

start "Client (Req)" cmd /T:8F /k lpclient.exe MIKE