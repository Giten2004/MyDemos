rem @echo off

rem 先编译bjam
start bootstrap.bat

rem 等待一分钟待bjam编译完成（如果电脑性能过差，可以设置等待时间更长一些）
SET SLEEP=ping 127.0.0.1 -n
%SLEEP% 60 > nul

rem 利用bjam编译boost库
rem 在此，我们编译vs2015下的x86 boost库文件

rem 建立库文件夹
rem vs2008 win32库文件夹
md stage\lib\win32_vs2015\

rem ******************************************************************
rem 先删除lib下所有文件（不删除文件夹）
del stage\lib\*.* /Q

rem 编译vs2015 win32库文件
bjam stage --toolset=msvc-14.0 architecture=x86 --stagedir=./stage/lib --build-type=complete threading=multi debug release

rem 拷贝至\stage\lib\win32_vs2015
copy stage\lib\*.* stage\lib\win32_vs2015\*.* /Y
rem ##################################################################



rem boost_1_49_0共有21个库需要编译使用，分别是chrono, date_time, exception, filesystem, graph, graph_parallel, iostreams, locale, math, 
rem mpi, program_options, python, random, regex, serialization, signals, system, test, thread, timer, wave。
rem 我仅选用了自己常用的几个做以上编译示例，其他使用者可以根据自己的需求选择编译。全部编译boost大概需要1个小时以上（视机器性能）
rem 全部编译boost的命令如下：bjam --toolset=msvc-9.0 --build-type=complete stage