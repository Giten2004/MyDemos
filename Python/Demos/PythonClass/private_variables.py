#!/usr/bin/env python
 
class Car:
    count = 0 # this is a class variable
    dogs = [] # this is a class variable

    def __init__(self):
        """在python中定义私有变量只需要在变量名或函数名前加上 "__"两个下划线
        https://shahriar.svbtle.com/underscores-in-python
        """
        # private instance attribute
        self.__maxspeed = 200 #self.xxx is an instance variable
        self.__name = "Supercar"
        # public instance attribute
        self.weight = "2t"
 
    def _internal_use(self):
        print("_internal_use")


    def __privateHello(self):
        """
        The __ intended behaviour here is almost equivalent to final methods in Java and normal (non-virtual) methods in C++.
        """
        print("does this private method can be called outside of class?")

    def drive(self): # this is an instance method
        print 'driving. maxspeed ' + str(self.__maxspeed)

    def setMaxSpeed(self,speed):
        self.__maxspeed = speed

    @staticmethod
    def the_static_method(x):
        print(x) 

    def hello(n):
        """
        You don't really need to use the @staticmethod decorator. 
        Just declaring a method (that doesn't expect the self parameter) and call it from the class. 
        The decorator is only there in case you want to be able to call it from an instance as well 
        (which was not what you wanted to do)
        """
        print("hello %s" % n)   
 
redcar = Car()
redcar.drive()

try:
    # Car instance has no attribute '__name'
    redcar.__privateHello()
except AttributeError as identifier:
    print(identifier)


# will not change variable because its private
redcar.__maxspeed = 10  

try:
    # Car instance has no attribute '__name'
    print("Access private instance virabla: %s" % redcar.__name)
except AttributeError as identifier:
    print(identifier)

try:
    # class Car has no attribute '__name'
    print("Access private instance virabla: %s" % Car.__name)
except AttributeError as identifier:
    print(identifier)    

print(redcar.weight)

redcar.drive()



redcar.setMaxSpeed(320)
redcar.drive()

Car.count = 10

print("redcar.count: %s" % redcar.count)

## will not change variable because its class attribute
redcar.count = 46

print("Car.count: %s" % Car.count)