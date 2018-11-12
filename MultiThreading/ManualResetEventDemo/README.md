# ManualResetEventDemo

本质上AutoResetEvent.Set()方法相当于ManualResetEvent.Set()+ManualResetEvent.Reset();

因此AutoResetEvent一次只能唤醒一个线程，其他线程还是堵塞







http://www.cnblogs.com/miku/p/4295533.html


参考CLR via c#及Windows核心编程第五版。