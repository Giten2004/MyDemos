echo off
REM ZeroMQ Pineline pattern example
REM Author: Manar Ezzadeen
REM Blog  : http://idevhawk.phonezad.com
REM Email : idevhawk@gmail.com

cd /d %~dp0

start "Worker 1" cmd /T:1F /k ZGuideExamples.exe Peering1 World Receiver0
start "Worker 2" cmd /T:1F /k TaskWorker.exe World Receiver0 Peering1
start "Worker 2" cmd /T:1F /k TaskWorker.exe Receiver0 Peering1 World
