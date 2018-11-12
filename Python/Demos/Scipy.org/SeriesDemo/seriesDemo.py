#!/usr/bin/env python
#-*-codong:utf-8-*-

import pandas as pd 

aSer = pd.Series([1, 2.0, 'a'])
print(aSer)


bSer = pd.Series(['apple', 'peach', 'lemon'], index=[1,2,3])

print(bSer)
print(bSer.index)
print(bSer.values)
