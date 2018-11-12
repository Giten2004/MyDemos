echo off

cd /d %~dp0


start "Reply" cmd /T:8E /k lbmresp.exe -v -v -q bxu

start "Request use event queun" cmd /T:3F /k lbmreq.exe -v -v -q -i -P 0 bxu

start "Request" cmd /T:8F /k lbmreq.exe -v -v bxu