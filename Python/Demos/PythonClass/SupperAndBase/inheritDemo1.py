# -*- coding: utf-8 -*-
"""
https://stackoverflow.com/questions/21639788/difference-between-super-and-calling-superclass-directly

"""

class A (object):
    def __init__ (self):
        super(A, self).__init__()
        print('A')

class B (A):
    def __init__ (self):
        super(B, self).__init__()
        print('B')

class C (A):
    def __init__ (self):
        super(C, self).__init__()
        print('C')

class D (C, B):
    def __init__ (self):
        super(D, self).__init__()
        print('D')


def main():
    newD = D()

if __name__ == "__main__":
    main()
