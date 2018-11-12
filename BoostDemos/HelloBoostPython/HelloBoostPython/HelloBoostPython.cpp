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
stageĿ¼�����ɵ�boost.python�����ƺ�ǰ׺lib���� libboost_python-vc90-mt-gd-1_48.lib����
����������ʱ��ʾȱ��boost_python-vc90-mt-gd-1_48.lib�⣬�� libboost_python-vc90-mt-gd-1_48.libǰ���libɾ�����ɣ�dllͬ��

2�� you have to change the extension of the DLL to .pyd or otherwise Python will not be able to load it.
https://stackoverflow.com/questions/11036319/boost-python-hello-world-example-not-working-in-python
*/

BOOST_PYTHON_MODULE(HelloBoostPython)
{
	using namespace boost::python;
	def("greet", greet);
}

