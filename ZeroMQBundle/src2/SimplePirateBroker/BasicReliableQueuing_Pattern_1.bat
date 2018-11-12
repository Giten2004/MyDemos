echo off

cd /d %~dp0
start "Server (Broker)" cmd /T:8E /k SimplePirateBroker.exe 

start "Worker (Req)" cmd /T:6F /k SimplePirateWorker.exe Worker-1

start "Client (Req)" cmd /T:8F /k lpclient.exe MIKE