#!/usr/bin/python

## Animal is-a object (yes, sort of confusing) look at the extra credit
class Animal(object):
    pass

## ??
class Dog(Animal):

    def __init__(self, name):
        ## ??
        self.name = name
        print("new Dog")

## ??
class Cat(Animal):

    def __init__(self, name):
        ## ??
        self.name = name
        print("new Cat")

## ??
class Person(object):

    def __init__(self, name):
        ## ??
        self.name = name

        ## Person has-a pet of some kind
        self.pet = None
        print("new Person")

## ??
class Employee(Person):

    def __init__(self, name, salary):
        ## ?? hmm what is this strange magic? 
        # call the base class's constructor
        super(Employee, self).__init__(name)
        ## ??
        self.salary = salary
        print("new Employee")

## ??
class Fish(object):
    pass

## ??
class Salmon(Fish):
    pass

## ??
class Halibut(Fish):
    pass


## rover is-a Dog
rover = Dog("Rover")

## ??
satan = Cat("Satan")

## ??
mary = Person("Mary")

## ??
mary.pet = satan

## ??
frank = Employee("Frank", 120000)

## ??
frank.pet = rover

## ??
flipper = Fish()

## ??
crouse = Salmon()

## ??
harry = Halibut()
