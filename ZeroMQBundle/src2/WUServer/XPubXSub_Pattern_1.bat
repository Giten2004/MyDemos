echo off
REM ZeroMQ Pub-Sub pattern example 1
REM One Pub and two Sub (all messages subscription)
REM Author: Manar Ezzadeen
REM Blog  : http://idevhawk.phonezad.com
REM Email : idevhawk@gmail.com

cd /d %~dp0

REM NOTE: multicast in zeromq is not recieve the multicast message from local.there is one setting.

start "Subscriber 1" cmd /T:8E /k WUClient.exe 72622  192.168.30.61 8100 tcp
start "Subscriber 2" cmd /T:8E /k WUClient.exe 610041  192.168.30.61 8100 pgm

start "Proxy" cmd /T:6F /k WUProxy.exe -f "127.0.0.1:5556:tcp|127.0.0.1:5557:tcp" -b 192.168.30.61:8100:all

start "Publisher" cmd /T:8F /k WUServer.exe 127.0.0.1 5556 all
start "Publisher" cmd /T:8F /k WUServer2.exe 127.0.0.1 5557 all