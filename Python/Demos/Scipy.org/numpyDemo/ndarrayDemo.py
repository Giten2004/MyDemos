#!/usr/bin/env python
#-*-coding:utf-8-*-

from numpy import *

nArray = array([1, 2, 3])
print(type(nArray))
print("nArray.shape: %s" % nArray.shape)
print(nArray)

bArray = array([(1, 2, 3), (4, 5, 6)])
print(bArray)
print("sin(bArray)")
print(sin(bArray))

print("bArray.reshape(3,2): %s" % bArray.reshape(3, 2))

cArray = zeros((2, 2))
print(cArray)

dArray = arange(1, 5, 0.5)
print(dArray)
