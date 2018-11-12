// HelloBoostPython.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"


#include <boost/python/module.hpp>
#include <boost/python/def.hpp>

char const* greet()
{
	return "hello, world from .pyd";
}


/*
stage目录里生成的boost.python库名称含前缀lib（如 libboost_python-vc90-mt-gd-1_48.lib），
如果编译程序时提示缺少boost_python-vc90-mt-gd-1_48.lib库，将 libboost_python-vc90-mt-gd-1_48.lib前面的lib删掉即可，dll同理。

2） you have to change the extension of the DLL to .pyd or otherwise Python will not be able to load it.
https://stackoverflow.com/questions/11036319/boost-python-hello-world-example-not-working-in-python
*/

BOOST_PYTHON_MODULE(HelloBoostPython)
{
	using namespace boost::python;
	def("greet", greet);
}

