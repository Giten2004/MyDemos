echo off
REM ZeroMQ Req-Rep pattern example 1
REM One Req and one Reb
REM Author: Manar Ezzadeen
REM Blog  : http://idevhawk.phonezad.com
REM Email : idevhawk@gmail.com

cd /d %~dp0

start "MDBroker" cmd /T:8E /k MDBroker.exe

start "MDWorker 1" cmd /T:6A /k MDWorker.exe 

start "MDClient 1" cmd /T:3F /k MDClient.exe
