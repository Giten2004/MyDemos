#!/usr/bin/python
# -*- coding: utf-8 -*-
 
import sqlite3 as lite
import sys
 
con = lite.connect('user.db')
 
with con:
 
    cur = con.cursor()    
    cur.execute("CREATE TABLE Users(Id INT, Name TEXT)")

    cur.execute("INSERT INTO Users VALUES(1,'Michelle')")
    cur.execute("INSERT INTO Users VALUES(2,'Sonya')")
    cur.execute("INSERT INTO Users VALUES(3,'Greg')")

    cur.execute("CREATE TABLE Jobs(Id INT, Uid INT, Profession TEXT)")
    cur.execute("INSERT INTO Jobs VALUES(1,1,'Scientist')")
    cur.execute("INSERT INTO Jobs VALUES(2,2,'Marketeer')")
    cur.execute("INSERT INTO Jobs VALUES(3,3,'Developer')")