#!/usr/bin/python
#-*-coding:utf-8-*-

try:
  fh = open("test", "r")
except IOError:
  print "Error: can not find the file or read data"
else:
  print "finally"
  fh.close()
