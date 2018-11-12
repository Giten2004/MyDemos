echo off


cd /d %~dp0
start "Subscriber 1" cmd /T:4F /k SyncSub.exe 
start "Subscriber 2" cmd /T:4F /k SyncSub.exe 

start "Publisher" cmd /T:0A /k SyncPub.exe 