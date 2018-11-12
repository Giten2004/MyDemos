# -*- coding: utf-8 -*-
"""
https://stackoverflow.com/questions/21639788/difference-between-super-and-calling-superclass-directly

"""

class A2 (object):
    def __init__ (self):
        print('A2')

class B2 (A2):
    def __init__ (self):
        A2.__init__(self)
        print('B2')

class C2 (A2):
    def __init__ (self):
        A2.__init__(self)
        print('C2')

class D2 (C2, B2):
    def __init__ (self):
        B2.__init__(self)
        C2.__init__(self)
        print('D2')


def main():
    """
    As you can see, A2 occurs twice. This is usually not what you want. 
    It gets even messier when you manually call method of one of your base types that uses super. 
    So instead, you should just use super() to make sure everything works, 
    and also so you don’t have to worry about it too much.
    """
    newD = D2()

if __name__ == "__main__":
    main()
