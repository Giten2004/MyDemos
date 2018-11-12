#include "DynamicMath.h"
#include <iostream>

DynamicMath::DynamicMath(void)
{

}
DynamicMath::~DynamicMath(void)
{

}

double DynamicMath::add(double a, double b)
{
    return a + b;
}
double DynamicMath::sub(double a, double b)
{
    return a - b;
}
double DynamicMath::mul(double a, double b)
{
    return a*b;
}
double DynamicMath::div(double a, double b)
{
    return a/b;
}

void DynamicMath::print()
{
    std::cout << "Hello libary." << std::endl;
}
