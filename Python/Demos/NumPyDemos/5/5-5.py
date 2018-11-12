#!/usr/bin/python
#-*-coding:utf-8-*-


# 一维数组  
import numpy as np 

a = np.arange(24)
print a.ndim 

# 现在调整其大小
b = a.reshape(2,4,3)  
print b 

# b 现在拥有三个维度
