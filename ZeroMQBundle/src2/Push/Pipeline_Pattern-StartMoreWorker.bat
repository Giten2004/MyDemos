echo off
REM ZeroMQ Pineline pattern example
REM Author: Manar Ezzadeen
REM Blog  : http://idevhawk.phonezad.com
REM Email : idevhawk@gmail.com

cd /d %~dp0

start "Worker 3" cmd /T:1F /k PullPushWorker.exe -l tcp://127.0.0.1:5000 -s tcp://127.0.0.1:5001 -t "#msg# (Worker 3)" -d 100
start "Worker 4" cmd /T:1F /k PullPushWorker.exe -l tcp://127.0.0.1:5000 -s tcp://127.0.0.1:5001 -t "#msg# (Worker 4)" -d 100
