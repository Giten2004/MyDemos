#!/usr/bin/python
#-*-coding:utf-8-*-

# 使用端记号  
# <意味着编码是小端（最小有效字节存储在最小地址中）。 >意味着编码是大端（最大有效字节存储在最小地址中）。



import numpy as np

dt = np.dtype('>i4')
print dt
