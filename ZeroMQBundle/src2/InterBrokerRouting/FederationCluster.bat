echo off


cd /d %~dp0

start "Broker 1" cmd /T:0A /k InterBrokerRouting.exe -n "Broker 1" -f tcp://127.0.0.1:55500 -b tcp://127.0.0.1:55501 -F tcp://127.0.0.1:55502 -B tcp://127.0.0.1:66602;tcp://127.0.0.1:77702
start "Broker 2" cmd /T:0A /k InterBrokerRouting.exe -n "Broker 2" -f tcp://127.0.0.1:66600 -b tcp://127.0.0.1:66601 -F tcp://127.0.0.1:66602 -B tcp://127.0.0.1:55502;tcp://127.0.0.1:77702
start "Broker 3" cmd /T:0A /k InterBrokerRouting.exe -n "Broker 3" -f tcp://127.0.0.1:77700 -b tcp://127.0.0.1:77701 -F tcp://127.0.0.1:77702 -B tcp://127.0.0.1:55502;tcp://127.0.0.1:66602