// HelloBoost.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <iostream>
#include <boost\thread\thread.hpp>

void hello()
{
	std::cout << "hello world, I'm a Boost thread!" << std::endl;
}


int main()
{
	boost::thread thrd(&hello);
	thrd.join();

	system("pause");

    return 0;
}

