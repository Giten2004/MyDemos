#!/usr/bin/python

class Dog:

    kind = 'canine'         # class variable shared by all instances

    def __init__(self, name):
        self.name = name    # instance variable unique to each instance




a = Dog("Fido")

print(a.kind)
print()
print(a.name)

print("##################################")
print(Dog.kind)

print("###################################")
a.Age = 10
print("dynamic add Age property: %s" % a.Age)

print("Dog.Age %s" % Dog.Age)
