#Convert Python Object (Dict) to JSON

import json
from decimal import Decimal
 
d = {}
d["Name"] = "Luke"
d["Country"] = "Canada"
 
print json.dumps(d, ensure_ascii=False)
# result {"Country": "Canada", "Name": "Luke"}