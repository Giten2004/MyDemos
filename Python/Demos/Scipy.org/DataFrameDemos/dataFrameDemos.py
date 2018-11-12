#!/usr/bin/env python
#-*-coding:utf-8-*-

import pandas as pd

data = {'name':['Wangdachui', 'LinLing', 'Niuyun'], 'pay':[4000,30,6000]}
dataframe = pd.DataFrame(data)

print(dataframe)

print("dataframe['name']")
print(dataframe['name'])
print("dataframe.pay")
print(dataframe.pay)
