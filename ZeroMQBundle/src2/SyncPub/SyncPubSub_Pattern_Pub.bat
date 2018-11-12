echo off


cd /d %~dp0
rem start "Subscriber 1" cmd /T:4F /k SyncSub.exe 
rem start "Subscriber 2" cmd /T:4F /k SyncSub.exe 

start "Publisher" cmd /T:0A /k SyncPub.exe 