#!/usr/bin/env python
#-*-coding:utf-8-*-


"""
Ref: https://www.youtube.com/watch?v=Z_Kxg-EYvxM
"""
try:
    import pyserial
except:
    print("python package pyserial is no installed, begin to download and install.")
    import pip
    pip.main(['install', 'pyserial'])
    import pyserial
    print("Install python package pyserial successfully")
