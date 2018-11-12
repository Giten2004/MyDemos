#!/usr/bin/env python
#
# -*- coding:utf-8 -*-
# File: sync_socket_client.py
#
import socket
import threading
import SocketServer

def client(ip, port, message):
    sock=socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.connect((ip, port))
    try:
        sock.sendall(message)
        response = sock.recv(1024)
        print "Received: {}".format(response)
    finally:
        sock.close()

if __name__ == "__main__":
    # Port 0 means to select an arbitrary unused port
    HOST, PORT = "localhost", 5555
    
    th1 = threading.Thread(target=client, args=(HOST, PORT, "Hello World 1",))
    th2 = threading.Thread(target=client, args=(HOST, PORT, "Hello World 2",))
    th3 = threading.Thread(target=client, args=(HOST, PORT, "Hello World 3",))
    th1.start()
    th2.start()
    th3.start()

    th1.join()
    th2.join()
    th3.join()