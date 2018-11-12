echo off

cd /d %~dp0

REM NOTE: multicast in zeromq is not recieve the multicast message from local.there is one setting.

start "Subscriber 2" cmd /T:8E /k pathosub.exe tcp://127.0.0.1:5558

start "Subscriber 3" cmd /T:8E /k pathosub.exe tcp://127.0.0.1:5558