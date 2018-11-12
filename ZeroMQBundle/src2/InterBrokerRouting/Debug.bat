echo off


cd /d %~dp0

start "Broker 1" cmd /T:0A /k InterBrokerRouting.exe -n "Broker 1" -f tcp://127.0.0.1:55500 -b tcp://127.0.0.1:55501 -F tcp://127.0.0.1:55502 -M tcp://127.0.0.1:55503 -S tcp://127.0.0.1:55504

start "REQWorker-broker-1-01" cmd /T:8F /k REQWorker.exe "REQWorker-broker-1-01" tcp://127.0.0.1:55501

start "Client - broker-1 1" cmd /T:4F /k REQClientTask.exe "Client - broker-1 1" tcp://127.0.0.1:55500 tcp://127.0.0.1:55503