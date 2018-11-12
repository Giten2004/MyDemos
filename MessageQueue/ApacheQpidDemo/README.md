# ApacheQpidDemo

Messaging built on AMQP (https://qpid.apache.org/)
Apache Qpid™ makes messaging tools that speak AMQP and support many languages and platforms.

AMQP versions:
* 0-8 2006-06
* 0-9 2006-12
* 0-9-1 2008-11
* 0-10 2009
* 1.0 draft

From http://www.amqp.org/resources/download
Since its inception, the AMQP Working Group has stated that backwards compatibility for protocol 
versions created before 1.0 series could not be assumed. 
After 1.0 backwards compatibility is a core goal of the AMQP Working Group.

Here i am want to talk about the client of Apache Qpid.
There is no binary release of Apache Qpid client. you have to download the source code and to build it for yourself.
There are many AMQP client, like RabbitMQ which is base on  AMQP 0-9-1. the Amqp.Net Lite is base on AMQP 1.0.

Red Hat MRG Messaging（base on qpid） provides the pre build client on multiple languages,include C#, 
But it needs to pay.No evaluation.
https://access.redhat.com/downloads/
《Red_Hat_Enterprise_MRG-3-Messaging_Programming_Reference-en-US》

#Build Apache Qpid on windows
To to get the Qpid .net client. Actually, the .net client of Qpid(Org.Apache.Qpid.Messaging.dll) is not
write in C#, it's writed by managed C++ that can be directly reference and called by C#.
The simple build guid: https://issues.apache.org/jira/browse/QPID-6266

* Qpid0.30 (https://svn.apache.org/repos/asf/qpid/tags/0.30/qpid)+ 
* Qpid Proton 0.8 (https://svn.apache.org/repos/asf/qpid/proton/tags/0.8)+ 
* CMake 3.2.3 + 
* VS2010 + 
* Ruby2.2.2 + 
* boost-win-1.47-32bit-vs2010 (http://people.apache.org/~chug/boost-win-1.47/)+ 
* Phython2.5.2

can be build successfuly


To build Qpid 0.35
* Qpid (https://svn.apache.org/repos/asf/qpid/trunk/qpid) 2015-07-22 the version is about 0.35 + 
* Qpid Proton (https://svn.apache.org/repos/asf/qpid/proton/trunk) 2015-07-22 the version is about 0.8+ + 
* CMake 2.8.12.2 + 
* VS2010 + 
* Ruby2.2.2 + 
* boost-win-1.47-32bit-vs2010 (http://people.apache.org/~chug/boost-win-1.47/)+ 
* Phython2.5.2

can be build successfuly


#AMQP SSL connection
AMQP SSL connection
I fixed the SSL connection issue between demo application to Simple AMQP broker by debugging the source code of QPID.
Summary:
* The Qpid C++ client(include .net) must use the system environment variable to specify out the certificate Name, and path. 
It must set before you start your app. you can't set it in your code, it does not take effect. 
there are two types of environment variable you can use.
* The client private certificate was saved in the System's store.
```
set QPID_SSL_CERT_STORE=<CertificateStore>
set QPID_SSL_CERT_NAME=<friendlyName> //Be careful the name, it's sensitive. you can copy it from the certificate
For example:
set QPID_SSL_CERT_STORE=Personal
set QPID_SSL_CERT_NAME=abcfr_abcfralmmacc1
```

* Another type. The certificate is save in PKCS12 format file, protected by password.
```
set QPID_SSL_CERT_FILENAME=<certificateFile>
set QPID_SSL_CERT_PASSWORD_FILE=<passwordFile>
set QPID_SSL_CERT_NAME=<friendlyName>
for example:
set QPID_SSL_CERT_FILENAME=.\ABCFR_ABCFRALMMACC1.p12
//The password file is a plain text file containing the password to the PKCS12 file
set QPID_SSL_CERT_PASSWORD_FILE=.\ABCFR_ABCFRALMMACC1.pwd
//Be careful the name, it's sensitive. you can copy it from the certificate, 
  you can get from the tool or import the certificate to system, and get it from certmgr.msc
set QPID_SSL_CERT_NAME=abcfr_abcfralmmacc1
```

* There are Two issues in Eurex's document and demo code of make SSL connection. one is certificate, 
already comment out in last section. Another is in making SSL connection, the "username" must be specified. and it's sensitive.
```
// the username is case sensitive
connection.SetOption("username", "ABCFR_ABCFRALMMACC1");
```
* In broker Address, must use the host name, not the IP. if you use IP, 
you will get the error"notice SSL negotiation failed to 192.168.34.11:11234: The target principal name is incorrect."
The host-name is not signed in certificate, I don't known why.
The correct one:
```
"amqp:ssl:chengdudev6:11234"
```
When you change the System Environment Variable, you should restart your Visual Studio, if you do't do that, you will find that the your environment variable of QPID client does not take effect.






#qpid-config
ead the Eurex's simple AMQP broker installer script, found why we execute qpid-conifg command failed with message "Connection refused", the simple AMQP broker needs to login, then you can execute the command.
like
```
qpid-config queues -a admin/admin@chengdudev6:21234
```

-a admin/admin@chengdudev6:21234 was the login information.
