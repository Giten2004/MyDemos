#!/usr/bin/python
#-*-coding:utf-8-*-

# http://www.restran.net/2015/10/22/how-python-code-run/

import imp
import sys

def generate_pyc(name):
    #返回一个 tuple类型，来间接达到返回多个值
    fp, pathname, description = imp.find_module(name)
    try:
        imp.load_module(name, fp, pathname, description)    
    finally:
        if fp:
            fp.close()


if __name__ == '__main__':
    generate_pyc(sys.argv[1])