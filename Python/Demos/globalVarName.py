#/usr/bin/python

Money = 2000
def AddMoney():
    # if you want to fix, just uncomment the follow
    global Money
    Money = Money + 1


print Money
AddMoney()
print Money
