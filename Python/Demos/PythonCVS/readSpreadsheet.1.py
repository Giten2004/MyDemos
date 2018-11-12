import csv
 
# create list holders for our data.
names = []
jobs = []
 
# open file
with open('persons.csv', 'rb') as f:
    reader = csv.reader(f)
 
    # read file row by row
    rowNr = 0
    for row in reader:
        # Skip the header row.
        if rowNr >= 1:
            names.append(row[0])
            jobs.append(row[1])
 
        # Increase the row number
        rowNr = rowNr + 1
 
# Print data 
print names
print jobs