echo off


cd /d %~dp0

FederationCluster.bat

Client - broker-1.bat
Client - broker-2.bat
Client - broker-3.bat

REQWorker-broker-1.bat
REQWorker-broker-2.bat
REQWorker-broker-3.bat