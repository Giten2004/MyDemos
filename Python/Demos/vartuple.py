#!/usr/bin/python
#-*-coding:utf-8-*-

def printinfo(arg1, *vartuple):
    print "output:"
    print arg1
    for var in vartuple:
      print var
    return

printinfo(10);
printinfo(70,60,50);
printinfo("test", '234', 500);



# ###########
#可写函数说明
sum=lambda arg1, arg2:arg1 + arg2;

#调用sum函数
print "Value of total : ",sum(10,20)
print "Value of total : ",sum(20,20)
