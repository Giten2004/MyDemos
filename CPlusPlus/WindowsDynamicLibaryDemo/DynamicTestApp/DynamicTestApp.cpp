// DynamicTestApp.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include "DynamicMath.h"
#include <iostream>
using namespace std;


int main()
{
	double a = 10;
	double b = 2;

	cout << "a + b = " << DynamicMath::add(a, b) << endl;
	cout << "a - b = " << DynamicMath::sub(a, b) << endl;
	cout << "a * b = " << DynamicMath::mul(a, b) << endl;
	cout << "a / b = " << DynamicMath::div(a, b) << endl;

	DynamicMath dyn;
	dyn.print();

	system("pause");

    return 0;
}

