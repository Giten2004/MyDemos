#!/usr/bin/python
#-*-coding:utf-8-*-

# Function definition is here
def printme(str):
    "打印任何传入的字符串"
    print str;
    return;

def changeme(mylist):
    mylist.append([1,2,3,4]);
    print "internal:", mylist
    return

mylist = [10,20,30];
changeme(mylist);
print "outside:", mylist
# Now you can call printme function
printme("我要调用用户自定义函数!");
printme("再次调用同一函数");
