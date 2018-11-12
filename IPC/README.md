https://msdn.microsoft.com/en-us/library/bb546085(v=vs.110).aspx

Named pipes provide interprocess communication between a pipe server and one or more pipe clients.
They offer more functionality than anonymous pipes, 
which provide interprocess communication on a local computer. 
Named pipes support full duplex communication over a network and multiple server instances, 
message-based communication, and client impersonation, 
which enables connecting processes to use their own set of permissions on remote servers.
To implement name pipes, use the NamedPipeServerStream and NamedPipeClientStream classes.




Robust Programming
The client and server processes in this example are intended to run on the same computer, 
so the server name provided to the NamedPipeClientStream object is ".". 
If the client and server processes were on separate computers, 
"." would be replaced with the network name of the computer that runs the server process.