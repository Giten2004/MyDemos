# Convert JSON to Python Object (float)

import json
from decimal import Decimal
 
jsondata = '{"number": 1.573937639}'
 
x = json.loads(jsondata, parse_float=Decimal)

print x['number']