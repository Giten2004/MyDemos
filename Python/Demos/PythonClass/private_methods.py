#!/usr/bin/env python
 
class Car: 
    def __init__(self):
        self.__updateSoftware()
 
    def drive(self):
        print 'driving'
 
    def __updateSoftware(self):
        print 'updating software'
 
redcar = Car()
redcar.drive()
print("The private attributes and methods are not really hidden, they are renamed adding '_Car' in the beginning of their name.")
redcar._Car__updateSoftware()
redcar.__updateSoftware()  # __updateSoftware is not accesible from object.