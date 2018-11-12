# lock和Monitor

lock关键字其实是一个语法糖，如果过查看IL代码，会发现lock 调用块开始位置为Monitor::Enter，块结束位置为Monitor::Exit。



上面对Pulse和Wait方法的介绍还是很抽象的，下面进一步了解Pulse和Wait。

Wait：当线程调用 Wait 时，它释放对象的锁并进入等待队列。对象的就绪队列中的下一个线程（如果有）获取锁并拥有对对象的独占使用。所有调用 Wait 的线程都将留在等待队列中，直到它们接收到由锁的所有者发送的 Pulse 或 PulseAll 的信号为止。
Pulse：只有锁的当前所有者可以使用 Pulse 向等待对象发出信号。如果发送了 Pulse，则只影响位于等待队列最前面的线程。如果发送了 PulseAll，则将影响正等待该对象的所有线程。接收到信号后，一个或多个线程将离开等待队列而进入就绪队列。 在调用 Pulse 的线程释放锁后，就绪队列中的下一个线程（不一定是接收到脉冲的线程）将获得该锁。
使用注意事项：

在使用Enter和Exit方法的时候，建议像lock的IL代码一样，使用try/finally语句块对Enter和Exit进行包装。
Pulse 、PulseAll 和 Wait 方法必须从同步的代码块内调用。
在使用Pulse/Wait进行线程同步的时候，一定要牢记，Monitor 类不对指示 Pulse 方法已被调用的状态进行维护。 因此，如果在没有等待线程时调用 Pulse，则下一个调用 Wait 的线程将阻止，似乎 Pulse 从未被调用过。 如果两个线程正在使用 Pulse 和 Wait 交互，则可能导致死锁。





http://www.cnblogs.com/wilber2013/p/4426285.html

参考CLR via c#及Windows核心编程第五版。