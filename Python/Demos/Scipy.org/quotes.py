#!/usr/bin/env python
#-*-coding:utf-8-*-

from matplotlib.finance import quotes_historical_yahoo_ochl
from datetime import date
import pandas as pd

today = date.today()
start = (today.year-1, today.month, today.day)
quotes = quotes_historical_yahoo_ochl('AXP', start, today)
fields = ['date', 'open', 'close', 'high', 'low', 'volumn']
df = pd.DataFrame(quotes,columns = fields)

print(df)
