Here we are going to build QPID 0.34 on windows, include Qpid clients for .net

[QPID 0.34](https://qpid.apache.org/releases/qpid-cpp-0.34/index.html) supports AMQP version 0-10 and AMQP version 1.0

To run qpid and the clients with AMQP 1.0 you need to build qpid-proton(which qpid-proton version should be used for QPID 0.34?) and link it into the qpid build.

VS2010: do not use other virtual studio version, because only vs2010 was best supported in cmake file of QPID.

## Donwload sourcecode for Qpid 0.34


Building qpid-cpp on windows is a definite challenge. As time marches on the suite of cmake-boost-qpid-proton-visual_studio must all progress together. Typical issues are cmake version X or Boost version Y doesn't have support for Visual Studio version Z. Qpid includes various source code changes to accomodate cmake, boost, Proton, and Visual Studio in its various releases. 

Build Environment
Name
version
URL
OS	Widnows 7 Professional N	
VS	Virtual Studio 2010 Sp1	vs2010 was best supported in cmake file of QPID, If you want to use other vs version, I think you need to modify the CMake files of QPID proton and QPID.
Qpid Proton	0.9	https://qpid.apache.org/releases/qpid-proton-0.9/index.html
Python	2.7.14	https://www.python.org/ftp/python/2.7.14/python-2.7.14.msi
Qpid	0.34	https://qpid.apache.org/releases/qpid-cpp-0.34/index.html
Ruby(X86)	2.4.2	https://github.com/oneclick/rubyinstaller2/releases/download/rubyinstaller-2.4.2-2/rubyinstaller-2.4.2-2-x86.exe
Cmake(X86)	3.10.0	https://cmake.org/files/v3.10/cmake-3.10.0-win32-x86.msi
boost	1.47	http://www.boost.org/users/history/ https://sourceforge.net/projects/boost/files/boost/1.47.0/

Build sequence
OS + VS2010Sp1 
            +
            +
Install Python 2.7.14
            +
            +
Install CMake 3.10.0
            +
            +
Build Qpid Proton 0.9
            +
######################
            +
Build boost 1.47
            +
            +
Install Ruby 2.4.2
            +
            +
Build Qpid 0.34

Build Steps
Proton
======
Building Proton is straightforward and has few dependencies.
That said, the trick for building a Proton that can be included by
Qpid is by using a shared install directory.
 
Qpid
====
Building Qpid has more issues with dependencies such as Boost and Proton.
With some careful attention these issues may be solved directly.
 
Pre build setup:
================
I have some resources on my Apache personal page that will help you.
 
1. Download a VS2012 Boost from:
   http://people.apache.org/~chug/boost-win-1.53/
   Select the 32-bit or 64-bit version to fit your needs.
 
   Be careful that virus scanners do not prevent boost downloads.
   I do all my qpid development on a drive that is not part of
   antivirus protection.
 
2. Download some build scripts from:
   http://people.apache.org/~chug/proton-win/
 
   Assume that your builds are in these folders:
      Qpid    - D:\qpid
      Proton  - D:\proton
 
   The scripts are named with .bat.txt suffixes to enable downloads without
   having virus scanners or system protections prevent the download. Once
   these files are downloaded, rename them to simple .bat
 
   When done you should have
      D:\qpid\build_qpid.bat
      D:\proton\build_proton.bat
 
3. Edit build_qpid.bat to set MY_BOOST_ROOT to hold the specific locations where
your boost libraries are stored on your build system.
 
4. The build_qpid script requires that you do NOT define BOOST_ROOT in your
environment. Windows-based cmake has trouble differentiating between 32-bit
and 64-bit boost installations. In my experience it is best to allow the
build scripts to define Boost on the fly. Please undefine BOOST_ROOT before
executing these scripts.
 
Ready to build
==============
You are now ready to build. Here is the important theory behind these scripts:
 
1. You must build proton first since qpid will share the proton install directory.
 
2. The build_proton script requires the root path for where it
will place the installed proton. To work with the build_qpid script
this path is the name of the directory in which the build_qpid.bat
script exists: D:\qpid
 
3. These scripts use a regular pattern for naming build/install,
visual studio version, and architecture folders. This is somewhat arbitrary but
it keeps things regular and simple. For both proton and qpid the folders are
named:
   build_2008_x86   install_2008_x86
   build_2008_x64   install_2008_x64
   build_2010_x86   install_2010_x86
   build_2010_x64   install_2010_x64
   build_2012_x86   install_2012_x86
   build_2012_x64   install_2012_x64
 
Running the build
=================
 
1. Build proton. (32-bit x86 is shown here. For 64-bit choose 'x64' instead)
    D:
    cd \proton
    build_proton D:\qpid 2012 x86
 
2. Proton build result. After a successful build you should have
    D:\proton\build_2012_x86
    D:\qpid\install_2012_x86
 
    Note: proton is installed in the qpid directory. The proton folder has
    only proton build folders and no install folders.
 
3. Build qpid
   D:
   cd \qpid
   build_qpid 2012 x86
 
   Observe during the cmake part of the build that Proton is found and that
   AMQP 1.0 support is included. Ignore the 0.7/0.8 version warnings:
 
   -- Found Proton: optimized;D:/Users/svn/qpid/install_2010_x86/lib/qpid-proton.lib;
      debug;D:/Users/svn/qpid/install_2010_x86/lib/qpid-protond.lib (found version "0.8")
      CMake Warning at src/amqp.cmake:28 (message):
         Qpid proton 0.8 is not a tested version and might not be compatible, 0.7 is
         highest tested; build may not work
 
4. Qpid build result. After a successful build you should have
    D:\proton\build_2012_x86
    D:\qpid\build_2012_x86
    D:\qpid\install_2012_x86
 
5. In folder D:\qpid\build_2012_x86 execute the procedure
   start-devenv-messaging-msvc10-x64-64-bit.bat
   This puts Boost in your path, defines QPID_BUILD_ROOT, and then
   launches the org.apache.qpid.messaging.sln

How to choose Qpid Proton verstion?
In file "D:\qpid\cpp\src\amqp.cmake" , you can find compatible version for Qpid Proton




