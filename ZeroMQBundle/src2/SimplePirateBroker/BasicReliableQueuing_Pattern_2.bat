echo off

cd /d %~dp0
start "Server (Broker)" cmd /T:8E /k SimplePirateBroker.exe 

start "Worker1 (Req)" cmd /T:6F /k SimplePirateWorker.exe Worker-1
start "Worker2 (Req)" cmd /T:6F /k SimplePirateWorker.exe Worker-2
start "Worker3 (Req)" cmd /T:6F /k SimplePirateWorker.exe Worker-3
start "Worker4 (Req)" cmd /T:6F /k SimplePirateWorker.exe Worker-4
start "Worker5 (Req)" cmd /T:6F /k SimplePirateWorker.exe Worker-5
start "Worker6 (Req)" cmd /T:6F /k SimplePirateWorker.exe Worker-6

start "Client (Req)MIKE" cmd /T:8F /k lpclient.exe MIKE
start "Client (Req)Lucy" cmd /T:8F /k lpclient.exe Lucy
start "Client (Req)Laurent" cmd /T:8F /k lpclient.exe Laurent
start "Client (Req)James" cmd /T:8F /k lpclient.exe James