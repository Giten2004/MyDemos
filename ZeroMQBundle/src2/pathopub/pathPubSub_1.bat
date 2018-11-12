echo off

cd /d %~dp0

REM NOTE: multicast in zeromq is not recieve the multicast message from local.there is one setting.

start "Subscriber" cmd /T:8E /k pathosub.exe

start "Publisher" cmd /T:8F /k pathopub.exe 