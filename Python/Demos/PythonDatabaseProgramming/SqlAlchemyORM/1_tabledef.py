from sqlalchemy import *
from sqlalchemy import create_engine, ForeignKey
from sqlalchemy import Column, Date, Integer, String
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import relationship, backref
 
engine = create_engine('sqlite:///student.db', echo=True)
Base = declarative_base()
 
########################################################################
class Student(Base):
    """"""
    __tablename__ = "student"
 
    id = Column(Integer, primary_key=True)
    username = Column(String)
    firstname = Column(String)
    lastname = Column(String)
    university = Column(String)
 
    #----------------------------------------------------------------------
    def __init__(self, username, firstname, lastname, university):
        """"""
        self.username = username
        self.firstname = firstname
        self.lastname = lastname
        self.university = university
 
# create tables
Base.metadata.create_all(engine)