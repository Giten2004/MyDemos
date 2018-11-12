echo off
REM ZeroMQ Pineline pattern example
REM Author: Manar Ezzadeen
REM Blog  : http://idevhawk.phonezad.com
REM Email : idevhawk@gmail.com

cd /d %~dp0

start "Worker 1" cmd /T:1F /k TaskWorker.exe
start "Worker 2" cmd /T:1F /k TaskWorker.exe
start "Worker 3" cmd /T:1F /k TaskWorker.exe
start "Worker 4" cmd /T:1F /k TaskWorker.exe
start "Worker 5" cmd /T:1F /k TaskWorker.exe
start "Worker 6" cmd /T:1F /k TaskWorker.exe