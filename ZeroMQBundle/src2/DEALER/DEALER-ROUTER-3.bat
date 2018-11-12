echo off

cd /d %~dp0


start "Server 1 (ROUTER)" cmd /T:8E /k ROUTER.exe -b tcp://127.0.0.1:5000 -r "#msg# Reply 1" -d 0
start "Server 2 (ROUTER)" cmd /T:8E /k ROUTER.exe -b tcp://127.0.0.1:5001 -r "#msg# Reply 2" -d 0

start "Client (DEALER)" cmd /T:8F /k DEALER.exe -c tcp://127.0.0.1:5000;tcp://127.0.0.1:5001 -m "ClientA  #nb#" -x 500 -d 100
start "Client (DEALER)" cmd /T:8F /k DEALER.exe -c tcp://127.0.0.1:5000;tcp://127.0.0.1:5001 -m "ClientB  #nb#" -x 500 -d 100
start "Client (DEALER)" cmd /T:8F /k DEALER.exe -c tcp://127.0.0.1:5000;tcp://127.0.0.1:5001 -m "ClientC  #nb#" -x 500 -d 100
start "Client (DEALER)" cmd /T:8F /k DEALER.exe -c tcp://127.0.0.1:5000;tcp://127.0.0.1:5001 -m "ClientD  #nb#" -x 500 -d 100
start "Client (DEALER)" cmd /T:8F /k DEALER.exe -c tcp://127.0.0.1:5000;tcp://127.0.0.1:5001 -m "ClientE  #nb#" -x 500 -d 100