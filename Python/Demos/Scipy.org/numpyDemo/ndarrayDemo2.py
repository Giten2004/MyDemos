#!/usr/bin/env python
#-*-coding:utf-8-*-

from numpy import *

aArray = array([(5, 5, 5),(5, 5, 5)])
bArray = array([(2, 2, 2),(2, 2, 2)])
cArray = aArray * bArray

print(cArray)

aArray = aArray + bArray

print(aArray)

print(aArray> 5)
