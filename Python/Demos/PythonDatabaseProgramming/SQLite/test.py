#!/usr/bin/python
# -*- coding: utf-8 -*-
 
import sqlite3 as lite
import sys
 
connection = None
 
try:
    connection = lite.connect('test.db')
    cursor = connection.cursor()    
    cursor.execute('SELECT SQLITE_VERSION()')
    data = cursor.fetchone()
    print "SQLite version: %s" % data                
except lite.Error, e:   
    print "Error %s:" % e.args[0]
    sys.exit(1)
finally:    
    if connection:
        connection.close()