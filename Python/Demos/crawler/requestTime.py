#!/usr/bin/env python
 
# "charpter 1 test script"
 
import time
import requests
 
def getHTMLText(url):
    try:
        r = requests.get(url, timeout=30)
        r.raise_for_status()
        #r.encoding = r.apparent_encoding
        return 1
    except:
        return 0
 
if __name__ =="__main__":
    url = "http://www.icourse163.org/"
    print "start test"
    start_time = time.time()
    success_times = 0
    for i in range(100):
        if getHTMLText(url):
            success_times += 1
            print ("get HTML text %d times!") % (success_times)
        else:
            print "get HTML text failed!"
    end_time = time.time()
    print ("use time:%dS") % (end_time - start_time)
