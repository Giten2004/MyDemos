#!/usr/bin/python
#-*-coding:utf-8-*-

class Employee:
   'class doc'
   empCount = 0

   def __init__(self, name, salary):
      self.name = name
      self.salary = salary
      Employee.empCount += 1

   def displayCount(self):
      print "Total Employee %d" % Employee.empCount

   def displayEmployee(self):
      print "Name : ", self.name, ", Salary: ", self.salary


emp1 = Employee("Zara", 2000)
emp2 = Employee("Manni", 5000)
emp1.displayEmployee()
emp2.displayEmployee()

emp1.age = 7  # 添加一个 'age' 属性
emp1.age = 8  # 修改 'age' 属性
print "emp1.age: ", emp1.age
del emp1.age  # 删除 'age' 属性

print "Total Employee %d" % Employee.empCount

print"Employee.__doc__:",Employee.__doc__
print"Employee.__name__:",Employee.__name__
print"Employee.__module__:",Employee.__module__
print"Employee.__bases__:",Employee.__bases__
print"Employee.__dict__:",Employee.__dict__
