echo off

cd /d %~dp0
start "Server 1 (ROUTER)" cmd /T:8E /k ROUTER.exe -b tcp://127.0.0.1:5000 -r "#msg# Reply 1" -d 0
start "Server 2 (ROUTER)" cmd /T:8E /k ROUTER.exe -b tcp://127.0.0.1:5001 -r "#msg# Reply 2" -d 0

start "Client (Req)" cmd /T:8F /k Req.exe -c tcp://127.0.0.1:5000;tcp://127.0.0.1:5001 -m "Request  #nb#" -x 5 -d 1000
