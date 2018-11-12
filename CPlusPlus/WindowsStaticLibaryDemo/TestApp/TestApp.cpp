// TestApp.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include "StaticMath.h"
#include <iostream>
#include <cstdlib>

using namespace std;

int main()
{
	double a = 10;
	double b = 2;

	cout << "a + b = " << StaticMath::add(a, b) << endl;
	cout << "a - b = " << StaticMath::sub(a, b) << endl;
	cout << "a * b = " << StaticMath::mul(a, b) << endl;
	cout << "a / b = " << StaticMath::div(a, b) << endl;

	StaticMath sm;
	sm.print();

	system("pause");

	return 0;

}

