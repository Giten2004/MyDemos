#!/usr/bin/env python
 
# Define a filename.
filename = "bestand.py"
 
# Open the file as f.
# The function readlines() reads the file.             
with open(filename) as f:
    content = f.readlines()
 
# Show the file contents line by line.
# We added the comma to print single newlines and not double newlines.
# This is because the lines contain the newline character '\n'. 
for line in content:
    print(line),