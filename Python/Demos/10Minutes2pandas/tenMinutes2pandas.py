#!/usr/bin/env python
#-*-coding:utf-8-*-

import pandas as pd
import numpy as np
import matplotlib.pyplot as plt

s = pd.Series([1, 3, 5, np.nan, 6, 8])
print(s)

dates = pd.date_range('20130101', periods=6)
print(dates)

df = pd.DataFrame(np.random.randn(6, 4), index = dates, columns=list('ABCD'))
print(df)

df2 = pd.DataFrame({'A' : 1.,
                    'B' : pd.Timestamp('20130102'),
                    'C' : pd.Series(1, index=list(range(4)), dtype='float32'),
                    'D' : np.array([3] * 4, dtype='int32'),
                    'E' : pd.Categorical(["test", "train", "test", "train"]),
                    'F' : 'foo'})
print(df2)
print(df2.dtypes)

print()

print(df.head())
print()

print(df.tail(3))
print()

print(df.index)
print

print(df.columns)
print
print(df.values)

print
print(df.describe())

print('Transpose')
print(df.T)

print("sort by an axis")
print(df.sort_index(axis=1, ascending=False))

print("sort by values")
print(df.sort_values(by='B'))

