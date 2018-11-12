import csv
 
# open file
with open('persons.csv', 'rb') as f:
    reader = csv.reader(f)
 
    # read file row by row
    for row in reader:
        print row