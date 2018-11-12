#!/usr/bin/python
#-*-coding:utf-8-*-


# 现在将其应用于 ndarray 对象  

import numpy as np 

dt = np.dtype([('age',np.int8)]) 
a = np.array([(10,),(20,),(30,)], dtype = dt)  

print a
