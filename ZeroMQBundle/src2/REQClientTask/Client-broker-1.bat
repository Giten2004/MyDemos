echo off


cd /d %~dp0


start "Client - broker-1 1" cmd /T:4F /k REQClientTask.exe "Client - broker-1 1" tcp://127.0.0.1:55500
start "Client - broker-1 2" cmd /T:4F /k REQClientTask.exe "Client - broker-1 2" tcp://127.0.0.1:55500
start "Client - broker-1 3" cmd /T:4F /k REQClientTask.exe "Client - broker-1 3" tcp://127.0.0.1:55500
start "Client - broker-1 4" cmd /T:4F /k REQClientTask.exe "Client - broker-1 4" tcp://127.0.0.1:55500
start "Client - broker-1 5" cmd /T:4F /k REQClientTask.exe "Client - broker-1 5" tcp://127.0.0.1:55500
start "Client - broker-1 6" cmd /T:4F /k REQClientTask.exe "Client - broker-1 6" tcp://127.0.0.1:55500
start "Client - broker-1 7" cmd /T:4F /k REQClientTask.exe "Client - broker-1 7" tcp://127.0.0.1:55500
start "Client - broker-1 8" cmd /T:4F /k REQClientTask.exe "Client - broker-1 8" tcp://127.0.0.1:55500
start "Client - broker-1 9" cmd /T:4F /k REQClientTask.exe "Client - broker-1 9" tcp://127.0.0.1:55500
start "Client - broker-1 10" cmd /T:4F /k REQClientTask.exe "Client - broker-1 10" tcp://127.0.0.1:55500