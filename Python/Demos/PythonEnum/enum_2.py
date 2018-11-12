def enum(**enums):
    return type('Enum', (), enums)
 
Numbers = enum(ONE=1,TWO=2, THREE=3, FOUR=4, FIVE=5, SIX=6)
print(Numbers.ONE)