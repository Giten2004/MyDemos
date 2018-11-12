echo off


cd /d %~dp0


start "Client - broker-2 1" cmd /T:4F /k REQClientTask.exe "Client - broker-2 1" tcp://127.0.0.1:66600
start "Client - broker-2 2" cmd /T:4F /k REQClientTask.exe "Client - broker-2 2" tcp://127.0.0.1:66600
start "Client - broker-2 3" cmd /T:4F /k REQClientTask.exe "Client - broker-2 3" tcp://127.0.0.1:66600
start "Client - broker-2 4" cmd /T:4F /k REQClientTask.exe "Client - broker-2 4" tcp://127.0.0.1:66600
start "Client - broker-2 5" cmd /T:4F /k REQClientTask.exe "Client - broker-2 5" tcp://127.0.0.1:66600
start "Client - broker-2 6" cmd /T:4F /k REQClientTask.exe "Client - broker-2 6" tcp://127.0.0.1:66600
start "Client - broker-2 7" cmd /T:4F /k REQClientTask.exe "Client - broker-2 7" tcp://127.0.0.1:66600
start "Client - broker-2 8" cmd /T:4F /k REQClientTask.exe "Client - broker-2 8" tcp://127.0.0.1:66600
start "Client - broker-2 9" cmd /T:4F /k REQClientTask.exe "Client - broker-2 9" tcp://127.0.0.1:66600
start "Client - broker-2 10" cmd /T:4F /k REQClientTask.exe "Client - broker-2 10" tcp://127.0.0.1:66600