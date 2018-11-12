echo off


cd /d %~dp0
start "Subscriber 3" cmd /T:4F /k SyncSub.exe 
start "Subscriber 4" cmd /T:4F /k SyncSub.exe 