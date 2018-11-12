#Convert JSON to Python Object (Dict)

import json
 
json_data = '{"name": "Brian", "city": "Seattle"}'
python_obj = json.loads(json_data)

print python_obj["name"]
print python_obj["city"]