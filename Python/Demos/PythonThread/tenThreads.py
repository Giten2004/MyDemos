import threading
 
# Our thread class
class MyThread (threading.Thread):
 
    def __init__(self, x):
        self.__x = x
        threading.Thread.__init__(self)
 
    def run (self):
          print str(self.__x)
 
# Start 10 threads.
for x in xrange(10):
    MyThread(x).start()