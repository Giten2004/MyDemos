echo off
REM ZeroMQ Pub-Sub pattern example 1
REM One Pub and two Sub (all messages subscription)
REM Author: Manar Ezzadeen
REM Blog  : http://idevhawk.phonezad.com
REM Email : idevhawk@gmail.com

cd /d %~dp0
start "Subscriber" cmd /T:8E /k WUClient.exe 72622 192.168.30.61 5556 tcp
start "Publisher" cmd /T:8F /k WUServer.exe tcp://192.168.30.61:5556