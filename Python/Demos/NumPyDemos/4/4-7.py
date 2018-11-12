#!/usr/bin/python
#-*-coding:utf-8-*-


import numpy as np

student = np.dtype([('name','S20'),  ('age',  'i1'),  ('marks',  'f4')])  
print student

print '###################################'

a = np.array([('abc',  21,  50),('xyz',  18,  75)], dtype = student)  
print a
