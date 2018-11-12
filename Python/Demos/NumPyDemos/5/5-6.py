#!/usr/bin/python
#-*-coding:utf-8-*-


# 数组的 dtype 为 int8（一个字节）  
import numpy as np 

x = np.array([1,2,3,4,5], dtype = np.int8)

print x.itemsize
