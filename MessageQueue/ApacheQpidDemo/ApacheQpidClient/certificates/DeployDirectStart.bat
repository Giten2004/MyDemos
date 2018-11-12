@ECHO OFF

:: http://stackoverflow.com/questions/21606419/set-windows-environment-variables-with-in-batch-file
:: %HOMEDRIVE% = C:
:: %HOMEPATH% = \Users\Ruben
:: %system32% ??
:: No spaces in paths
:: Program Files > ProgramFiles
:: cls = clear screen
:: CMD reads the system environment variables when it starts. To re-read those variables you need to restart CMD
:: Use console 2 http://sourceforge.net/projects/console/


:: Assign all Path variables
SET QPID_LOG_ENABLE="trace+"
SET QPID_LOG_TO_FILE="..\..\logs\QpidLog.log"
SET QPID_SSL_CERT_FILENAME=".\cert\LCMLO_LIQSPALBBLCM1.p12"
SET QPID_SSL_CERT_NAME="LCMLO_LIQSPALBBLCM1"
SET QPID_SSL_CERT_PASSWORD_FILE=".\cert\LCMLO_LIQSPALBBLCM1.pwd"


:: Set system variable
:: setx JAVA_HOME "%HOMEDRIVE%\ProgramFiles\Java\jdk1.7.0_21" /m
setx QPID_LOG_ENABLE "trace+" /m
setx QPID_LOG_TO_FILE "..\..\logs\QpidLog.log" /m
setx QPID_SSL_CERT_FILENAME ".\cert\LCMLO_LIQSPALBBLCM1.p12" /m
setx QPID_SSL_CERT_NAME "LCMLO_LIQSPALBBLCM1" /m
setx QPID_SSL_CERT_PASSWORD_FILE ".\cert\LCMLO_LIQSPALBBLCM1.pwd" /m


PAUSE