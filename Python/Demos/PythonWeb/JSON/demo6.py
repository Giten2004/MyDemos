#Converting JSON data to Python objects 

import json
 
json_data = '{"name": "Brian", "city": "Seattle"}'
python_obj = json.loads(json_data)
print json.dumps(python_obj, sort_keys=True, indent=4)