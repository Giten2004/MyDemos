How to build Boost

1. 从http://www.boost.org/users/download/下载boost
2. 解压文件：在D盘创建文件夹Boost，将下载文件解压至此
3. 运行D:\Boost\boost_*\bootstrap.bat
4.  编译bjam：打开命令提示符（cmd），进入到运行D:\Boost\boost_*\目录，输入

`
bjam stage --toolset=msvc-10.0 architecture=x86 --stagedir=./boost_lib --build-type=complete threading=multi debug release
`



5. 设置编译器查找库的路径：待编译完毕，打开VS项目，在工具->选项->项目和解决方案->VC++目录中加入D:\Boost\boost_*\boost_lib\lib




* 参考一
https://www.cnblogs.com/zhcncn/p/3950477.html
* 参考二
https://www.cnblogs.com/wondering/archive/2009/05/21/boost_setup.html

