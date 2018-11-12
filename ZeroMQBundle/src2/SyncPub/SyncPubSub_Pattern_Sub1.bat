echo off


cd /d %~dp0
start "Subscriber 1" cmd /T:4F /k SyncSub.exe 
