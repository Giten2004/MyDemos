#pragma once
class DynamicMath
{
public:
	_declspec(dllexport) DynamicMath(void);
	_declspec(dllexport) ~DynamicMath(void);
 
    static _declspec(dllexport) double add(double a, double b);//加法
    static _declspec(dllexport) double sub(double a, double b);//减法
    static _declspec(dllexport) double mul(double a, double b);//乘法
    static _declspec(dllexport) double div(double a, double b);//除法
 
	_declspec(dllexport) void print();
};