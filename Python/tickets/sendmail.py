#!/usr/bin/python
#-*-coding:utf-8-*-

import smtplib
from prettytable import PrettyTable
from email.mime.text import MIMEText
from email.mime.multipart import MIMEMultipart

def sendmail():
    fromaddr = "xubo2004@126.com"
    toaddr = "398755692@qq.com"
    msg = MIMEMultipart()
    msg['From'] = fromaddr
    msg['To'] = toaddr
    msg['Subject'] = "Python email"

    x = PrettyTable()
    x._set_field_names(["City name", "Area", "Population", "Annual Rainfall"])
    x.add_row(["Adelaide",1295, 1158259, 600.5])
    x.add_row(["Brisbane",5905, 1857594, 1146.4])
    x.add_row(["Darwin", 112, 120900, 1714.7])
    x.add_row(["Hobart", 1357, 205556, 619.5])
    x.add_row(["Sydney", 2058, 4336374, 1214.8])
    x.add_row(["Melbourne", 1566, 3806092, 646.9])
    x.add_row(["Perth", 5386, 1554769, 869.4])

    body = "{}".format(x)
    msg.attach(MIMEText(body, 'plain'))
    #message =
    try:
        smtpObj = smtplib.SMTP('smtp.126.com')
        smtpObj.set_debuglevel(1)
        smtpObj.ehlo()
        smtpObj.starttls()
        smtpObj.ehlo()
        smtpObj.login("xubo2004@126.com", "=========================")
        smtpObj.sendmail(fromaddr, toaddr, msg.as_string())
        print("Successfully sent email")
    except:
        print("Error: unable to send email")

if __name__ == '__main__':
    sendmail()