#!/usr/bin/python
#-*-coding:utf-8-*-


# 现在将其应用于 ndarray 对象  

import numpy as np 

dt = np.dtype([('age',np.int8)]) 
a = np.array([(10,),(20,),(30,)], dtype = dt)  

print a

print '############'
print '文件名称可用于访问 age 列的内容'
print a['age']
