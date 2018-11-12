https://www.cnblogs.com/skynet/p/3372855.html

与Linux相比，在Windows系统下创建动态库要稍微麻烦一些。
Window与Linux执行文件格式不同，在创建动态库的时候有一些差异。

1)  在Windows系统下的执行文件格式是PE格式，动态库需要一个DllMain函数做出初始化的入口，通常在导出函数的声明时需要有_declspec(dllexport)关键字。

2)  Linux下gcc编译的执行文件默认是ELF格式，不需要初始化入口，亦不需要函数做特别的声明，编写比较方便。


1)Linux Static Libary

g++ -c StaticMath.cpp
ar -crv libstaticmath.a StaticMath.o
g++ TestStaticLibrary.cpp -L ./ -l staticmath


2)Linux Dynamic Libary

g++ -fPIC -c DynamicMath.cpp
g++ -shared -o libdynmath.so DynamicMath.o
g++ TestDynamicLibrary.cpp -L ./ -l dynmath




Cmake 入门
https://github.com/carl-wang-cn/demo/tree/master/cmake

set (EXTRA_LIBS ${EXTRA_LIBS} MathFunctions) #解释原因：https://stackoverflow.com/questions/29078618/set-syntax-in-cmake



https://www.ibm.com/developerworks/cn/linux/l-cn-cmake/
https://cmake.org/cmake-tutorial/
http://www.hahack.com/codes/cmake/

C++ 内存模型：
在C++中，动态内存分配技术可以保证我们在程序运行过程中按照实际需要申请适量的内存，使用结束后还可以释放，
这种在程序运行过程中申请和释放的存储单元也被称为堆对象。（Ref 内存对齐与内存碎片）


C++转向C#的疑惑：难道C#中没有拷贝构造函数 ？
http://blog.csdn.net/zhuweisky/article/details/415661

指针函数与函数指针的区别
http://www.cnblogs.com/gmh915/archive/2010/06/11/1756067.html

C/C++内存管理详解
http://chenqx.github.io/2014/09/25/Cpp-Memory-Management/