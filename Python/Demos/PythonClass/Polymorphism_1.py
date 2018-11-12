#!/usr/bin/env python

class Bear(object):
    def sound(self):
        print "Groarrr"
 
class Dog(object):
    def sound(self):
        print "Woof woof!"
 
def makeSound(animalType):
    animalType.sound()
 
 
bearObj = Bear()
dogObj = Dog()
 
makeSound(bearObj)
makeSound(dogObj)