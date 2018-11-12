#include "DynamicMath.h"
#include <iostream>
#include <cstdlib>

using namespace std;
 
int main(int argc, char* argv[])
{
    double a = 10;
    double b = 2;
 
    cout << "a + b = " << DynamicMath::add(a, b) << endl;
    cout << "a - b = " << DynamicMath::sub(a, b) << endl;
    cout << "a * b = " << DynamicMath::mul(a, b) << endl;
    cout << "a / b = " << DynamicMath::div(a, b) << endl;
 
    DynamicMath sm;
    sm.print();
 
    system("pause");
    return 0;
}
