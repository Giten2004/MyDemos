import datetime

from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from 1_tabledef import *
 
engine = create_engine('sqlite:///student.db', echo=True)
 
# create a Session
Session = sessionmaker(bind=engine)
session = Session()
 
# Create objects  
for student in session.query(Student).order_by(Student.id):
    print student.firstname, student.lastname

# Select objects  
for student in session.query(Student).filter(Student.firstname == 'Eric'):
    print student.firstname, student.lastname