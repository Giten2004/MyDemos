echo off


cd /d %~dp0


start "REQWorker-broker-1-01" cmd /T:8F /k REQWorker.exe "REQWorker-broker-1-01" tcp://127.0.0.1:55501
start "REQWorker-broker-1-02" cmd /T:8F /k REQWorker.exe "REQWorker-broker-1-02" tcp://127.0.0.1:55501