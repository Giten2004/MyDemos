#!/usr/bin/python
#-*-coding:utf-8-*-



# 数组的 dtype 现在为 float32（四个字节）  
import numpy as np 
x = np.array([1,2,3,4,5], dtype = np.float32)  
print x.itemsize
