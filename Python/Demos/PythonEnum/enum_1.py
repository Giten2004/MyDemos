#from enum import Enum
from aenum import Enum
 
class Numbers(Enum):
    ONE = 1
    TWO = 2
    THREE = 3
    FOUR = 4
 
print(Numbers.ONE.value)
print(Numbers.THREE.value)