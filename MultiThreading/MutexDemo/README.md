# 互斥体Mutex

互斥体Mutex也是Windows用来进行线程同步的内核对象。当两个或更多线程需要同时访问一个共享资源时，可以使用 Mutex 同步基元，它只向一个线程授予对共享资源的独占访问权。 如果一个线程获取了互斥体，则要获取该互斥体的第二个线程将被挂起，直到第一个线程释放该互斥体。

说到这里，可以回想一下前面介绍的lock和Monitor，其实如果只是进行同一进程之间的线程同步，建议更多的使用lock和Monitor，因为Mutex示一个内核对象，所以使用的时候会产生很大的性能消耗；但是，Mutex可以支持不同进程之间的线程同步，同时允许同一个线程多次重复访问共享区，但是对于别的线程那就必须等待。因此，要根据应用的需求来选择lock/Monitor还是Mutex。


http://www.cnblogs.com/wilber2013/p/4440117.html

参考CLR via c#及Windows核心编程第五版。

通过内核对象进行线程同步会带来很大的性能开销，但是，由于内核对象属于操作系统，对所有进程可见，所以利用这些线程同步方式可以很容易的实现不同进程之间的线程同步。

interprocess synchronization:
Mutex, Semaphore, EventWaitHandle(construct a named event)
信号量，互斥体，事件是Windows专门用来进行线程同步的内核对象。