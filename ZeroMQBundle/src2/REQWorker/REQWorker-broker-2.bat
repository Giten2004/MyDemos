echo off


cd /d %~dp0


start "REQWorker-broker-2-02" cmd /T:8F /k REQWorker.exe "REQWorker-broker-2-02" tcp://127.0.0.1:66601
start "REQWorker-broker-2-02" cmd /T:8F /k REQWorker.exe "REQWorker-broker-2-02" tcp://127.0.0.1:66601