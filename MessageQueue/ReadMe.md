


AMQP 1.0 library for .Net
https://azure.github.io/amqpnetlite/
https://github.com/Azure/amqpnetlite

# AMQP.NET Lite Environment setup 
AMQP.NET Lite client is using Windows certificate store as the main source of certificates. 
Additionally to the certificates stored in the Windows Certificate Store, the public key belonging
to the member certificate has to be stored in a file. This file will be loaded by the client
application and used to tell the client library which private key should be used. 

http://www.eurexclearing.com/blob/3156372/948eb925ab2cd2a9aa6789dc7df49e67/data/eurex-clearing-messaging-connectivity-B-v19.pdf