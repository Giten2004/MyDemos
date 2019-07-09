import math
import numpy as np
from scipy.stats import norm

import scipy.stats as sp

# mu = 0
# variance = 1
# x = np.linspace(mu-3*variance,mu+3*variance, 100)
# y = [sp.norm.pdf(i) for i in x]
# plt.plot(x,y)
# d = [-1]
# plt.plot(d*100,np.linspace(0,sp.norm.pdf(d), 100))

# Ref: https://en.wikipedia.org/wiki/Black%E2%80%93Scholes_model

class BsmModel:
      def __init__(self, option_type, price, strike, interest_rate, expiry, volatility, dividend_yield=0):
         self.s = price # Underlying asset price
         self.k = strike # Option strike K
         self.r = interest_rate # Continuous risk fee rate
         self.q = dividend_yield # Dividend continuous rate
         self.T = expiry # time to expiry (year)
         self.sigma = volatility # Underlying volatility
         self.type = option_type # option type "p" put option "c" call option

      def n(self, d):
         # cumulative probability distribution function of standard normal distribution
         return norm.cdf(d)

      def dn(self, d):
         # the first order derivative of n(d)
         return norm.pdf(d)

      def d1(self):
         d1 = (math.log(self.s / self.k) + (self.r - self.q + self.sigma ** 2 * 0.5) * self.T) / (self.sigma * math.sqrt(self.T))
         return d1

      def d2(self):
         d2 = (math.log(self.s / self.k) + (self.r - self.q - self.sigma ** 2 * 0.5) * self.T) / (self.sigma * math.sqrt(self.T))
         return d2

      def bsm_price(self):
         d1 = self.d1()
         d2 = d1 - self.sigma * math.sqrt(self.T)
         if self.type == 'c':
            price = math.exp(-self.r*self.T) * (self.s * math.exp((self.r - self.q)*self.T) * self.n(d1) - self.k * self.n(d2))
            return price
         elif self.type == 'p':
            price = math.exp(-self.r*self.T) * (self.k * self.n(-d2) - (self.s * math.exp((self.r - self.q)*self.T) * self.n(-d1)))
            return price
         else:
            print "option type can only be c or p"

a = BsmModel('c', 42, 35, 0.06, 90.0/365, 0.372)
callprice = a.bsm_price()

print callprice