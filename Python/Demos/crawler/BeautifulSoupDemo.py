#!/usr/bin/python
#-*-coding:utf-8-*-

import requests
from bs4 import BeautifulSoup

def getHTMLText(url):
    try:
        r = requests.get(url, timeout=30)
        r.raise_for_status()
        r.encoding = r.apparent_encoding
        return r.text
    except:
        return "exception happened"

demo = getHTMLText("http://python123.io/ws/demo.html") 
soup = BeautifulSoup(demo, "html.parser")
print(soup.prettify())
