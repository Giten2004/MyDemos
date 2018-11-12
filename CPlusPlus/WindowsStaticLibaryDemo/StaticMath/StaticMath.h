#pragma once
class StaticMath
{
public:
    StaticMath(void);
    ~StaticMath(void);
 
    static double add(double a, double b);//加法
    static double sub(double a, double b);//减法
    static double mul(double a, double b);//乘法
    static double div(double a, double b);//除法
 
    void print();
};