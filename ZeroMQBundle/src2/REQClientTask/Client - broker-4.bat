echo off


cd /d %~dp0


start "Client 1" cmd /T:4F /k REQClientTask.exe "Client 1" tcp://127.0.0.1:5000
start "Client 2" cmd /T:4F /k REQClientTask.exe "Client 2" tcp://127.0.0.1:5000