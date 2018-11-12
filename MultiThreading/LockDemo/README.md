# lock和Monitor

lock关键字其实是一个语法糖，如果过查看IL代码，会发现lock 调用块开始位置为Monitor::Enter，块结束位置为Monitor::Exit。









http://www.cnblogs.com/wilber2013/p/4426285.html

参考CLR via c#及Windows核心编程第五版。