#!/usr/bin/anaconda2/bin/python
#-*-coding:utf-8-*-

import pandas as pd
from pandas import Series, DataFrame
import pandas.io.data as web

all_data = {} #空字典，存放各公司股票信息

#股票代码依次是苹果，IBM，微软，雅虎
for ticker in ['AAPL', 'IBM', 'MSFT', 'YHOO']:
    all_data[ticker] = web.get_data_yahoo(ticker, '1/1/2010', '1/1/2011')  #从Yahoo!Finance上获取相应股票代码的股票信息
    print("----------- {} Type: {} ----------".format(ticker, type(all_data[ticker])))
    print(all_data[ticker].columns)


"""
http://stackoverflow.com/questions/3294889/iterating-over-dictionaries-using-for-loops-in-python
To loop over both key and value you can use the following:

For Python 2.x:
  for key, value in d.iteritems():

For Python 3.x:
  for key, value in d.items():
"""

temp = {tic: data['Adj Close'] for tic, data in all_data.iteritems()}

print(temp.keys())

print("#######################################################################################")

price = DataFrame(temp)  #从all_data中获取历史价格
print(price)

volume = DataFrame({tic: data['Volume'] for tic, data in all_data.iteritems()}) #获取all_data中各公司的股票成交量


returns = price.pct_change()
# 计算价格的百分数变化
# 仅显示最后几个数据
returns.tail()


#Series 的 correct 方法用于计算两个 Series 中重叠的、非 NA 值的、按索引对齐的值的相关系数。与此类似，cov 用于计算协方差：
returns.MSFT.corr(returns.IBM)
returns.MSFT.cov(returns.IBM)

returns.corr()

returns.cov()

"""
利用DataFrame 的 corrwith 方法，可以计算其列或行跟另一个 Series 或 DataFrame 之间的相关系数。
传入一个 Series 将会返回一个相关系数值 Series；传入一个 DataFrame 则会计算按列名配对的相关系数：
"""
# 计算AAPL 与股票的相关系数
returns.corrwith(returns.AAPL)

# 计算百分比变化与成交量的相关系数
returns.corrwith(volume)

