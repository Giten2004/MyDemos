#!/usr/bin/env python
#-*-coding:utf-8-*-

from numpy import *

cArray = array([1, 2, 3])
print(cArray[:1])

dArray = array([2, 4, 5])
eArray = array([7, 8, 9])

fArray = where(cArray > 2, dArray, eArray)

print(fArray)
