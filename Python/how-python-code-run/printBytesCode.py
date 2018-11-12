#!/usr/bin/python
#-*-coding:utf-8-*-

# http://www.restran.net/2015/10/22/how-python-code-run/

import dis

s = open('demo.py').read()
co = compile(s, 'demo.py', 'exec')

dis.dis(co)