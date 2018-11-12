echo off

cd /d %~dp0

REM NOTE: multicast in zeromq is not recieve the multicast message from local.there is one setting.

start "Subscriber 2" cmd /T:8E /k PubSubTracingSub.exe

start "PubSubTracing" cmd /T:6F /k PubSubTracing.exe 

start "Publisher" cmd /T:8F /k PubSubTracingPub.exe 