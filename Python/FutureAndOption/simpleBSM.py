import math
import numpy as np
import scipy.stats as stats


# Ref: https://en.wikipedia.org/wiki/Black%E2%80%93Scholes_model

def d1(S, K, r, sigma, T, t):
    d1value = (math.log(S/K) + (r + sigma * sigma * 0.5)*(T - t)) / (sigma * math.sqrt(T - t))
    return d1value

def BSM_call(S, K, r, sigma, T, t):
    d1Value = d1(S, K, r, sigma, T, t)
    d2Value = d1Value - sigma * math.sqrt(T - t)
    callprice = stats.norm.cdf(d1Value) * S - stats.norm.cdf(d2Value) * K * math.exp(-r * (T - t))
    return callprice

def BSM_put(S, K, r, sigma, T, t):
    d1Value = d1(S, K, r, sigma, T, t)
    d2Value = d1Value - sigma * math.sqrt(T - t)
    putPrice = stats.norm.cdf(-d2Value) * K * math.exp(-r * (T - t)) - stats.norm.cdf(-d1Value) * S
    return putPrice

S = 100.0
K = 320.0
r = 0.06
sigma = 0.375
T = 3.3
t = 1.0

eta = 0.0001

callPrice = BSM_call(S, K, r, sigma, T, t)

callPrice_f = BSM_call(S+eta, K, r, sigma, T, t)
callPrice_b = BSM_call(S-eta, K, r, sigma, T, t)

callDelta = (callPrice_f - callPrice_b) / (2 * eta)

print 'call Price: ' , callPrice
print 'call Delata: ' , callDelta

print ''

putPrice = BSM_put(S, K, r, sigma, T, t)

putPrice_f = BSM_put(S + eta, K, r, sigma, T, t)
putPrice_b = BSM_put(S - eta, K, r, sigma, T, t)
putDelta = (putPrice_f - putPrice_b) / (2 * eta)

print 'put Price: ' , putPrice
print 'put Delata: ' , putDelta
