#include "stdafx.h"

#include "StaticMath.h"
#include <iostream>

StaticMath::StaticMath(void)
{

}
StaticMath::~StaticMath(void)
{

}

double StaticMath::add(double a, double b)
{
    return a + b;
}
double StaticMath::sub(double a, double b)
{
    return a - b;
}
double StaticMath::mul(double a, double b)
{
    return a*b;
}
double StaticMath::div(double a, double b)
{
    return a/b;
}

void StaticMath::print()
{
    std::cout << "Hello libary." << std::endl;
}
