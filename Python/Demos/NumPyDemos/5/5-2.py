#!/usr/bin/python
#-*-coding:utf-8-*-



# 这会调整数组大小  
import numpy as np 

a = np.array([[1,2,3],[4,5,6]])
print 'before adjust shape'
print a
print 'after adjusted shape'
a.shape =  (3,2)  
print a
