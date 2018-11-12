#!/usr/bin/python
#-*-coding:utf-8-*-

# http://www.restran.net/2015/10/22/how-python-code-run/

import foo

a = [1, 'python']
a = 'a string'

def func():
    a = 1
    b = 257
    print(a + b)

print(a)

if __name__ == '__main__':
    func()
    foo.add(1, 2)
