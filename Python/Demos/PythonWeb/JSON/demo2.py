#Convert JSON to Python Object (List)


import json
 
array = '{"drinks": ["coffee", "tea", "water"]}'
data = json.loads(array)
 
for element in data['drinks']:
    print element