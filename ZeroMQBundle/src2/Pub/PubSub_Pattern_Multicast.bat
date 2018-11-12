echo off
REM ZeroMQ Pub-Sub pattern example 1
REM One Pub and two Sub (all messages subscription)
REM Author: Manar Ezzadeen
REM Blog  : http://idevhawk.phonezad.com
REM Email : idevhawk@gmail.com

cd /d %~dp0
REM start "Subscriber 1" cmd /T:8E /k Sub.exe -c epgm://172.16.0.101;239.192.1.1:8100 -d 0

REM start "Publisher" cmd /T:7F /k Pub.exe -b epgm://172.16.0.101;239.192.1.1:8100 -m "Orange #nb#";"Apple  #nb#" -d 1000

REM https://stackoverflow.com/questions/20748244/clrzmq-pubsub-via-tcp-working-via-pgm-not-so-much

REM Can't work on the same computer, because there is a default setting which that ZeroMQ did not receive the multicast messages from local

start "Subscriber 2" cmd /T:8E /k Sub.exe -c pgm://;239.192.1.1:8100 -d 0

start "Publisher" cmd /T:8F /k Pub.exe -b pgm://192.168.30.61;239.192.1.1:8100 -m "Orange #nb#";"Apple  #nb#" -x 5000 -d 1000