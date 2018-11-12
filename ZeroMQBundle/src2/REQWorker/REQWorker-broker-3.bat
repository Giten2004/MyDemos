echo off


cd /d %~dp0


start "REQWorker-broker-3-01" cmd /T:8F /k REQWorker.exe "REQWorker-broker-3-01" tcp://127.0.0.1:77701
start "REQWorker-broker-3-01" cmd /T:8F /k REQWorker.exe "REQWorker-broker-3-01" tcp://127.0.0.1:77701