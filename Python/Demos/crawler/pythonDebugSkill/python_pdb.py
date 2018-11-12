#!/usr/bin/env python
#-*-coding:utf-8-*-

import pdb 

"""
Ref: http://www.ibm.com/developerworks/cn/linux/l-cn-pythondebugger/index.html
2) https://www.zhihu.com/question/21572891
"""

a = "aaa"
pdb.set_trace() 
b = "bbb"
c = "ccc"
final = a + b + c 
print final
