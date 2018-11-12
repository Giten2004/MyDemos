echo off
REM ZeroMQ Req-Rep pattern example 1
REM One Req and one Reb
REM Author: Manar Ezzadeen
REM Blog  : http://idevhawk.phonezad.com
REM Email : idevhawk@gmail.com

cd /d %~dp0

start "ReqRepBroker" cmd /T:8E /k ReqRepBroker.exe

start "RepWorker 1" cmd /T:6A /k RepWorker.exe "RepWorker 1" tcp://127.0.0.1:5560
start "RepWorker 2" cmd /T:6A /k RepWorker.exe "RepWorker 2" tcp://127.0.0.1:5560
start "RepWorker 3" cmd /T:6A /k RepWorker.exe "RepWorker 3" tcp://127.0.0.1:5560
start "RepWorker 4" cmd /T:6A /k RepWorker.exe "RepWorker 4" tcp://127.0.0.1:5560
start "RepWorker 5" cmd /T:6A /k RepWorker.exe "RepWorker 5" tcp://127.0.0.1:5560

start "ReqClient 1" cmd /T:3F /k ReqClient.exe
start "ReqClient 2" cmd /T:3F /k ReqClient.exe
start "ReqClient 3" cmd /T:3F /k ReqClient.exe
start "ReqClient 4" cmd /T:3F /k ReqClient.exe
start "ReqClient 5" cmd /T:3F /k ReqClient.exe
start "ReqClient 6" cmd /T:3F /k ReqClient.exe